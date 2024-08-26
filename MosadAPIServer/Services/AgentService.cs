using MosadAPIServer.ModelsHelpers;
using Microsoft.EntityFrameworkCore;
using MosadAPIServer.Data;
using MosadAPIServer.DTO;
using MosadAPIServer.Models;
using MosadAPIServer.Enums;
using MosadAPIServer.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace MosadAPIServer.Services
{
    public class AgentService : ICruService<Agent, AgentDTO>
    {
        private readonly MosadAPIServerContext _context;
        private readonly MissionService _missionService;

        public AgentService(MosadAPIServerContext context, MissionService missionService)
        {
            _context = context;
            _missionService = missionService;
        }

        public async Task<int> CreateAsync(AgentDTO agentDTO)
        {
            var newAgent = new Agent(agentDTO);
            _context.Agent.Add(newAgent);
            await _context.SaveChangesAsync();
            return newAgent.Id;
            
        }

        public async Task<List<Agent>> GetAllAsync()
        {
            return await _context.Agent.ToListAsync();
        }

       

        public async Task MoveAsync(int id, string dir)
        {
            var agent = _context.Agent.Find(id);  

            if (agent.Status == AgentStatus.Active)
                throw new InvalidOperationException("cannot move active agent");

            Location? newLocation;
            try
            {
                 newLocation = DirectionsService.Move(agent.GetLocation(), dir);
               
            }
            catch (Exception ex) { throw new InvalidOperationException(ex.Message); }

            agent.SetLocation(newLocation);

            _context.Entry(agent).State = EntityState.Modified;
            await _context.SaveChangesAsync();


            await _missionService.IdleAgentMoved(agent);
        }

        

        /// <summary>
        /// Pins an agent - sets its location and calls the IdleAgentMoved to calculate compatibleMissions
        /// </summary>
        /// <param name="agentId"></param>
        /// <param name="pinLocation"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="NullReferenceException"></exception>
        public async Task PinLocatinAsync(int agentId, Location pinLocation)
        {
            var agent = await _context.Agent.FindAsync(agentId);

            if (agent.GetLocation() != null)
                throw new InvalidOperationException("cannot pin pinned agent.");

            if (agent == null)
            {
                throw new NullReferenceException("agent not found");
            }

            agent.SetLocation(pinLocation);
            _context.Entry(agent).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            await _missionService.IdleAgentMoved(agent);
        }

        public bool IsExists(int id)
        {
            return _context.Agent.Any(e => e.Id == id);
        }

        public async Task<Agent> GetById(int id)
        {
            return await _context.Agent.FindAsync(id);
        }

        internal async Task<IEnumerable<AgentWithMissionIdDTO>> GetAllAgentsWithMissionId()
        {
           var agents = await  _context.Agent.Include(a=>a.Missions).ToListAsync();   
                            
               

            List<AgentWithMissionIdDTO> agentWithMissionId = [];

            foreach (var agent in agents)
            {
                var assignedMission = agent?.Missions?.FirstOrDefault(m => m.Status == MissionStatus.Assigned);
                agentWithMissionId.Add(new AgentWithMissionIdDTO
                    (agent, assignedMission?.Id, assignedMission?.TimeToKill));
            }
            return agentWithMissionId;
        }

        
    }
}
