namespace MosadMvcServer.Services
{
    public interface IHttpJsonService<T>
    {
        public Task<T?> GetAsync(string path);
        public Task<List<T>> GetAllAsync(string path);
       
    }
}
