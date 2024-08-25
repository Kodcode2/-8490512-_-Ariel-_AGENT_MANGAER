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
            await RemoveOutOfRangeCompatableTargets(agent);

            await CreateCompatableMissions(agent);
        }

        private async Task RemoveOutOfRangeCompatableTargets(Agent agent)
        {
            var uncompatibleMatches = _context.Mission.Include(m=>m.Agent).Include(m=>m.Target).Where(m=>
            m.AgentId == agent.Id   &&
            m.Status == MissionStatus.OpenForAssignment
            )
            .Where(m=>DirectionsService.AreOutOfAssignRange(m.Agent.GetLocation(), m.Target.GetLocation()));

            if (!uncompatibleMatches.IsNullOrEmpty())
            {
                _context.RemoveRange(uncompatibleMatches);
                await _context.SaveChangesAsync();
            }
        }

        private async Task CreateCompatableMissions(Agent agent)
        {
            var compatableTargets =
                _context.Target.Where(t =>

            t.AssignedToMission == false                    &&
            t.Status            == TargetStatus.Alive       &&
            t.GetLocation()     != null                    
            ).Where(t =>
            DirectionsService.IsInRange(agent.GetLocation(), t.GetLocation())) .ToList();

            var existingCompMissions = GetAllMatchingMissions(agent.Id);

             

            foreach (var compatableTarget in compatableTargets)
            {
                var newMission = new Mission() { AgentId = agent.Id, TargetId = compatableTarget.Id, Status = MissionStatus.OpenForAssignment };
                if (!existingCompMissions.Any(m=>m.TargetId == compatableTarget.Id))
                    _context.Add(newMission);
            }
            await _context.SaveChangesAsync();
        }

        private async Task CreateCompatableMissions(Target target)
        {
            var compatableAgents = _context.Agent.Where(a =>
           a.Status == AgentStatus.Idle &&
           DirectionsService.IsInRange(a.GetLocation(), target.GetLocation())
           ).ToList();


            foreach (var compatableAgent in compatableAgents)
            {
                var newMission = new Mission() { 
                    AgentId = compatableAgent.Id,
                    TargetId = target.Id,
                    Status = MissionStatus.OpenForAssignment };

                var existingCompMissions = GetAllMatchingMissions(compatableAgent.Id);
                if (!existingCompMissions.Any(m => m.TargetId == target.Id))
                    _context.Add(newMission);
            }
            _context.SaveChanges();
        }

        public List<Mission>  GetAllMatchingMissions(int agentId)
        {
            if (!_context.Agent.Any(a => a.Id == agentId)) throw new Exception("wrong id");

            return  _context.Mission.Where(m=>m.AgentId == agentId && m.Status == MissionStatus.OpenForAssignment).ToList();
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

            await CreateCompatableMissions(target);
        }

        private async Task AssignedTargetMoved(Target target)
        {
            // we can add some logic if we want
            var targetMission = _context.Mission
                .Include(m => m.Target)
                .Include(m => m.Agent)
                .Where(m => m.TargetId == target.Id && m.Target.AssignedToMission).Single();

            CheckKill(targetMission);   
        }


        /// <summary>
        /// checks if the target an agent are on the same spot
        /// </summary>
        /// <param name="targetMission"> a mission with target an agent filled in it</param>
        /// <exception cref="NotImplementedException"></exception>
        private async Task CheckKill(Mission mission)
        {
            if (mission.Agent == null || mission.Target == null)
                throw new ArgumentNullException("mission should include target and agent");

            if(mission.Target.GetLocation() == mission.Agent.GetLocation())
            {// a kill was found
                MissionFinished(mission);
            }
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
            _context.SaveChanges();
        }


        /// <summary>
        /// moves assigned agents and updates location and time to kill
        /// </summary>
        public async Task UpdateMissions()
        {
            var activeMissions = _context.Mission
                 .Include(m => m.Agent)
                 .Include(m => m.Target)
                 .Where(m => m.Status == MissionStatus.Assigned);

            foreach (var mission in activeMissions)
            {
                mission.Agent.SetLocation(
                    DirectionsService.MoveTowards(
                        mission.Agent.GetLocation(),
                    mission.Target.GetLocation()
                    ));

                mission.Duration = DateTime.Now - mission.AssignedTime;
                mission.TimeToKill = CalculateTimeToKill(mission.Agent, mission.Target);
                _context.Update(mission);
            }
            _context.SaveChanges();
        }

       

        internal async Task<IEnumerable<Mission>> getAllMissions(string? status)
        {
            
            
            if (Enum.TryParse<MissionStatus>(status, out var missionStatus))
                return _context.Mission.Where(m=>m.Status == missionStatus).ToList();
           
           
            return _context.Mission.ToList();
        }

        internal async Task AssignMission(Mission mission)
        {
            var currMission = _context.Mission.Include(m=>m.Agent).Include(m=>m.Target).First(m=>m.Id == mission.Id);

            // fill mission details
            currMission.Status = MissionStatus.Assigned;
            currMission.AssignedTime = DateTime.Now;
            currMission.TimeToKill = CalculateTimeToKill(mission.Agent , mission.Target);
            currMission.Duration = TimeSpan.Zero;

            //assign target to mission
            mission.Target.AssignedToMission = true;

            // remove all compatable with same target id
            var missionList = _context.Mission.Include(m=>m.Agent).Include(m=>m.Target).
                Where(m=>m.Id == mission.Id && m.TargetId == mission.TargetId && m.AgentId != mission.AgentId);
            _context.RemoveRange(missionList);


            _context.Entry(mission).State = EntityState.Modified;
            _context.SaveChanges();
        }

        private TimeSpan? CalculateTimeToKill(Agent agent , Target target)
        {
            double tilesDis = DirectionsService.GetAirDistance(agent.GetLocation(), target.GetLocation());
            return new TimeSpan((int)(tilesDis / 5 ),0,0);
        }
    }
}
