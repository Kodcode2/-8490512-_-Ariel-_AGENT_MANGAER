using MosadAPIServer.ModelsHelpers;
using Microsoft.EntityFrameworkCore;
using MosadAPIServer.Data;
using MosadAPIServer.DTO;
using MosadAPIServer.Models;
using MosadAPIServer.Enums;

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
            Location newLocation = agent.GetLocation();//if agent null the controller catches

            if (agent.Status == AgentStatus.Idle)
            {
                newLocation = DirectionsService.Move(agent.GetLocation(), dir); 

            }
            else if (agent.Status != AgentStatus.Active)
            {
                var target = _context.Agent.Where(a=>a.Id == id).Include(a=>a.Missions.Where(m=>m.Status == MissionStatus.Assigned) ?? new List<Mission>());
                newLocation = DirectionsService.MoveTowards(agent.GetLocation(), dir);
            }
            
            agent.SetLocation(newLocation);


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
