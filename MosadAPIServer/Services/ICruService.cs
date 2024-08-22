using MosadAPIServer.ModelsHelpers;
namespace MosadAPIServer.Services
{
    public interface ICruService<T> 
    {
        public Task<List<T>> GetAllAsync();
        public Task<int> CreateAsync();
        public Task PinLocatinAsync(int id, Location pinLocation);
        public Task MoveAsync(int id , string dir);
    }
}
