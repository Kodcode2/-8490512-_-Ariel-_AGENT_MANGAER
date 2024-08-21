using Microsoft.CodeAnalysis;
using MosadAPIServer.Data;

namespace MosadAPIServer.Services
{
    public class AgentService
    {
        private readonly MosadAPIServerContext _context;

        public AgentService(MosadAPIServerContext context)
        {
            _context = context;
        }

       
    }
}
