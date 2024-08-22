using System.Globalization;

namespace MosadAPIServer.MiddleWare
{
    public class AuthMiddleWare
    {
        private static Dictionary<string ,List<string>> TokenRoutes = new Dictionary<string, List<string>>()
        {
            {Guid.NewGuid().ToString(),[] }
        };
        private readonly RequestDelegate _next;

        public AuthMiddleWare(RequestDelegate next)
        {
            _next = next;

        }
        public async Task InvokeAsync(HttpContext context)
        {
           
            // Call the next delegate/middleware in the pipeline.
            await _next(context);
        }
    }
}
