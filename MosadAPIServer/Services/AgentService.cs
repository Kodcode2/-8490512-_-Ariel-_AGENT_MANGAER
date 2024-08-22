using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MosadAPIServer.Data;
using MosadAPIServer.Models;

namespace MosadAPIServer.Services
{
    public class AgentService : ICruService<Agent>
    {
        private readonly MosadAPIServerContext _context;

        public AgentService(MosadAPIServerContext context)
        {
            _context = context;
        }

        public Task<int> CreateAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Agent>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task MoveAsync(int id, string dir)
        {
            throw new NotImplementedException();
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

        public Task PinLocatinAsync(int id, ModelsHelpers.Location pinLocation)
        {
            throw new NotImplementedException();
        }
    }
}
