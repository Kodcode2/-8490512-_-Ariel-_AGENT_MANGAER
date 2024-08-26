using MosadAPIServer.Data;
using MosadAPIServer.Models;
using MosadAPIServer.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MosadAPIServer.Services
{
    public class MissionService
    {
        private readonly MosadAPIServerContext _context;

        public MissionService(MosadAPIServerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// indicates that a agent moved  calculates if there are targets for agent
        /// </summary>
        /// <param name="agentId"the agent's id</param>
        /// <returns></returns>
        public async Task IdleAgentMoved(Agent agent)
        {
            await RemoveUnCompatableTargets(agent);

            await CreateCompatableMissions(agent);
        }

        private async Task RemoveUnCompatableTargets(Agent agent)
        {
            var uncompatibleMatches = await _context.Mission
                .Include(m => m.Agent)
                .Include(m => m.Target)
                .Where(m =>
            m.AgentId == agent.Id &&
            m.Status == MissionStatus.OpenForAssignment 
            ).ToListAsync();

            if (uncompatibleMatches.IsNullOrEmpty())
            {
                return;
            }

            foreach (var m in uncompatibleMatches)
            {
                if (DirectionsService.AreOutOfAssignRange(m.Agent.GetLocation(), m.Target.GetLocation()))
                {
                    _context.RemoveRange(uncompatibleMatches);
                    await _context.SaveChangesAsync();

                }

            }
        }

        private async Task CreateCompatableMissions(Agent agent)
        {
            var compatableTargets = await
                _context.Target.Where(t =>

            t.AssignedToMission == false                    &&
            t.Status            == TargetStatus.Alive       &&
            t.LocationX         != null                     &&
            t.LocationY         != null 
            ).ToListAsync();


           
            var existingCompMissions = await GetAllMatchingMissions(agent.Id);

             

            foreach (var ct in compatableTargets)
            {
                var newMission = new Mission() { AgentId = agent.Id, TargetId = ct.Id, Status = MissionStatus.OpenForAssignment };
                if (!existingCompMissions.Any(m=>m.TargetId == ct.Id) &&
                    DirectionsService.IsInRange(agent.GetLocation(), ct.GetLocation()))
                    _context.Add(newMission);
            }
            await _context.SaveChangesAsync();
        }

        private async Task CreateCompatableMissions(Target target)
        {
            var compatableAgents = await _context.Agent.Where(a =>
           a.Status == AgentStatus.Idle
           ).ToListAsync();

            // filter only those how are in range
            compatableAgents = compatableAgents.Where(a => DirectionsService.IsInRange(a.GetLocation(), target.GetLocation())).ToList();

            foreach (var compatableAgent in compatableAgents)
            {
                var newMission = new Mission() { 
                    AgentId = compatableAgent.Id,
                    TargetId = target.Id,
                    Status = MissionStatus.OpenForAssignment };

                var existingCompMissions = await GetAllMatchingMissions(compatableAgent.Id);
                if (!existingCompMissions.Any(m => m.TargetId == target.Id))
                    _context.Add(newMission);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<List<Mission>>  GetAllMatchingMissions(int agentId)
        {
            if (!_context.Agent.Any(a => a.Id == agentId)) throw new Exception("wrong id");

            return  await _context.Mission.Where(m=>m.AgentId == agentId && m.Status == MissionStatus.OpenForAssignment).ToListAsync();
        }

       

        /// <summary>
        /// indicates that a target moved , runs some logic based on that
        /// </summary>
        /// <param name="id">the target id</param>
        /// <exception cref="NotImplementedException"></exception>
        public async Task TargetMoved(Target target)
        {
            
            if(target.Status == TargetStatus.Terminated  ) return;

            if (target.AssignedToMission)
            {
                await AssignedTargetMoved(target);
                return;
            }

            await removeOldUnCompatableMissions(target);
            await CreateCompatableMissions(target);
        }

        private async Task removeOldUnCompatableMissions(Target target)
        {
            var filledTarget = await _context.Target.Include(t=>t.Missions).ThenInclude(m=>m.Agent).SingleAsync(t=>t.Id == target.Id);

            foreach (var mission in filledTarget.Missions) 
            {
                var agent = mission.Agent;
                if (agent.Status == AgentStatus.Active ||
                    DirectionsService.AreOutOfAssignRange(agent.GetLocation(), target.GetLocation())){
                    _context.Mission.Remove(mission);
                }
            }
            await _context.SaveChangesAsync();
        }

        private async Task AssignedTargetMoved(Target target)
        {
            // we can add some logic if we want
            var targetMission = _context.Mission
                .Include(m => m.Target)
                .Include(m => m.Agent)
                .Where(m => m.TargetId == target.Id && m.Target.AssignedToMission).Single();

            await CheckKill(targetMission);   
        }


        /// <summary>
        /// checks if the target an agent are on the same spot
        /// </summary>
        /// <param name="targetMission"> a mission with target an agent filled in it</param>
        /// <exception cref="ArgumentNullException"></exception>
        private async Task<bool> CheckKill(Mission mission)
        {
            if (mission.Agent == null || mission.Target == null)
                throw new ArgumentNullException("mission should include target and agent");

            if(mission.Target.GetLocation() == mission.Agent.GetLocation())
            {// a kill was found
                await MissionFinished(mission);
                return true;
            }
            return false;
        }

        /// <summary>
        /// closes a <paramref name="mission"/> and marks filleds and statuses of linked objects
        /// </summary>
        /// <param name="mission"></param>
        private async Task MissionFinished(Mission mission)
        {
            mission.Duration = DateTime.Now - mission.AssignedTime;
            mission.TimeToKill = null;
            mission.Status = MissionStatus.Finished;

            mission.Target.Status = TargetStatus.Terminated;

            mission.Agent.Status = AgentStatus.Idle;
            mission.Agent.TotalKills++;

            _context.Entry(mission).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// moves assigned agents and updates location and time to kill
        /// </summary>
        public async Task UpdateMissions()
        {
            var activeMissions = await _context.Mission
                 .Include(m => m.Agent)
                 .Include(m => m.Target)
                 .Where(m => m.Status == MissionStatus.Assigned).ToListAsync();

            foreach (var mission in activeMissions)
            {
                // move agent
                mission?.Agent?.SetLocation(
                    DirectionsService.MoveTowards(
                        mission.Agent.GetLocation(),
                    mission.Target.GetLocation()
                    ));

                if(await CheckKill(mission)) continue;

                mission.Duration = DateTime.Now - mission.AssignedTime;
                mission.TimeToKill = CalculateTimeToKill(mission.Agent, mission.Target);
                _context.Update(mission);
            }
            await _context.SaveChangesAsync();
        }

       

        internal async Task<IEnumerable<Mission>> GetAllMissions(string? status)
        {
            
            if (Enum.TryParse<MissionStatus>(status, out var missionStatus))
                return await _context.Mission.Where(m=>m.Status == missionStatus).ToListAsync();
           
           
            return await _context.Mission.ToListAsync();
        }

        internal async Task<IEnumerable<Mission>> GetFullInfoMissions()
        {
            return await _context.Mission.Include(m=>m.Agent).Include(m=>m.Target).ToListAsync();
        }

        internal async Task AssignMission(int missionId)
        {
            var mission = await _context.Mission.FindAsync(missionId);
            if (mission == null) throw new ArgumentNullException("mission id was null");

            var currMission = _context.Mission.Include(m=>m.Agent).Include(m=>m.Target).First(m=>m.Id == mission.Id);

            // fill mission details
            currMission.Status = MissionStatus.Assigned;
            currMission.AssignedTime = DateTime.Now;
            currMission.TimeToKill = CalculateTimeToKill(mission.Agent , mission.Target);
            currMission.Duration = TimeSpan.Zero;

            //assign target to mission
            mission.Target.AssignedToMission = true;

            // activate agent
            mission.Agent.Status = AgentStatus.Active;

            // remove all compatable with same target id
            var missionList = _context.Mission.Include(m=>m.Agent).Include(m=>m.Target).
                Where(m=>m.Id == mission.Id && m.TargetId == mission.TargetId && m.AgentId != mission.AgentId);
            _context.RemoveRange(missionList);


            _context.Entry(mission).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        private static TimeSpan? CalculateTimeToKill(Agent agent , Target target)
        {
            double tilesDis = DirectionsService.GetAirDistance(agent.GetLocation(), target.GetLocation());
            return new TimeSpan((int)(tilesDis / 5 ),0,0);
        }

        
    }
}
