using Microsoft.EntityFrameworkCore;
using MosadAPIServer.Data;
using MosadAPIServer.DTO;
using MosadAPIServer.Models;
using MosadAPIServer.ModelsHelpers;

namespace MosadAPIServer.Services
{
    public class TargetService : ICruService<Target,TargetDTO>
    {
        private readonly MosadAPIServerContext _context;

        public TargetService(MosadAPIServerContext context)
        {
            _context = context;
        }
        public async Task<int> CreateAsync(TargetDTO targetDTO)
        {
            
            var newTarget = new Target(targetDTO);
            _context.Target.Add(newTarget);
            await _context.SaveChangesAsync();
            return newTarget.Id;
            
        }

        public async Task<List<Target>> GetAllAsync()
        {
            return await _context.Target.ToListAsync();
        }


        public Task MoveAsync(int id, string dir)
        {
            throw new NotImplementedException();
        }

        public async Task PinLocatinAsync(int id, Location pinLocation)
        {
            var target = await _context.Target.FindAsync(id);

            if (target == null)
            {
                throw new NullReferenceException("target not found");
            }

            _context.Entry(target).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public bool IsExist(int id)
        {
            return _context.Target.Any(e => e.Id == id);
        }
    }
}
