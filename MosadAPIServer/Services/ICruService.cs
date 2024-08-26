using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MosadAPIServer.DTO;
using MosadAPIServer.ModelsHelpers;

namespace MosadAPIServer.Services
{
    public interface ICruService<T,DTO> 
    {
        public Task<List<T>> GetAllAsync();
        public Task<int> CreateAsync(DTO DTOModel);
        public Task<T?> GetById(int id);
        public Task PinLocatinAsync(int id, Location pinLocation);
        public Task MoveAsync(int id , string dir);
        public bool IsExists(int id);
        
    }
}
