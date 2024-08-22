using MosadAPIServer.ModelsHelpers;
using Microsoft.EntityFrameworkCore;
using MosadAPIServer.Data;
using MosadAPIServer.DTO;
using MosadAPIServer.Models;

namespace MosadAPIServer.Services
{
    public class AgentService : ICruService<Agent,AgentDTO>
    {
        private readonly MosadAPIServerContext _context;

        public AgentService(MosadAPIServerContext context)
        {
            _context = context;
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



        public Task MoveAsync(int id, string dir)
        {
            var agent = _context.Agent.Find(id);

            DirectionsService

        }

        public async Task PinLocatinAsync(int id, Location pinLocation)
        {
            var agent = await _context.Agent.FindAsync(id);

            if (agent == null)
            {
                throw new NullReferenceException("agent not found");
            }

            _context.Entry(agent).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public bool IsExist(int id)
        {
            return _context.Agent.Any(e => e.Id == id);
        }
       
    }
}
