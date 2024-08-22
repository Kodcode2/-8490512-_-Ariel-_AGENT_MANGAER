using MosadAPIServer.ModelsHelpers;
using Microsoft.EntityFrameworkCore;
using MosadAPIServer.Data;
using MosadAPIServer.DTO;
using MosadAPIServer.Models;
using MosadAPIServer.Enums;

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
      
            Location? newLocation = DirectionsService.Move(agent.GetLocation(), dir);

            if (newLocation == null)
                throw new InvalidOperationException("cannot move beyond bounds.");
            
            agent.SetLocation(newLocation);

            _context.Entry(agent).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            // no await
            _missionService.IdleAgentMoved(agent.Id);
        }

        


        public async Task PinLocatinAsync(int id, Location pinLocation)
        {


            var agent = await _context.Agent.FindAsync(id);

            if (agent.GetLocation() != null)
                throw new InvalidOperationException("cannot pin pinned agent.");

            if (agent == null)
            {
                throw new NullReferenceException("agent not found");
            }

            _context.Entry(agent).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            _missionService.IdleAgentMoved(agent.Id);
        }

        public bool IsExists(int id)
        {
            return _context.Agent.Any(e => e.Id == id);
        }

        
    }
}
