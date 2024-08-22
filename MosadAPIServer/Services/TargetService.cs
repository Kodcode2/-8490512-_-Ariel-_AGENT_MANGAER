using MosadAPIServer.Models;
using MosadAPIServer.ModelsHelpers;

namespace MosadAPIServer.Services
{
    public class TargetService : ICruService<Target>
    {
        public Task<int> CreateAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Target>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task MoveAsync(int id, string dir)
        {
            throw new NotImplementedException();
        }

        public Task PinLocatinAsync(int id, Location pinLocation)
        {
            throw new NotImplementedException();
        }
    }
}
