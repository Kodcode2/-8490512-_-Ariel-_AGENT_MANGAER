using MosadAPIServer.DTO;
using MosadAPIServer.Enums;
using MosadAPIServer.Services;
using Newtonsoft.Json;
using NuGet.Packaging;
using NuGet.Protocol;
using System.Collections.Concurrent;
using System.Globalization;
using System.Text;

namespace MosadAPIServer.MiddleWare
{
    public class AuthMiddleWare
    {
        private static readonly ConcurrentDictionary<AuthId, List<string>> _AuthIdRoutesPairs = []; 
            
        
        

        private readonly RequestDelegate _next;
        private static bool FirstInitHappend = false;

        public AuthMiddleWare(RequestDelegate next )
        {
            _next = next;

            if (FirstInitHappend) return;

            _AuthIdRoutesPairs[AuthId.SimulationServer] = 
                ["Get/agents", "Post/agents", "Put/agents/{id}/pin", "Put/agents/{id}/move",
                     "Get/targets", "Post/targets", "Put/targets/{id}/pin", "Put/targets/{id}/move",
                     "Post/missions/update"
                    ];

            _AuthIdRoutesPairs[AuthId.MVCServer] =  ["Get/agents", "Get/targets", "Put/missions/{id}"];

            FirstInitHappend=true;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            //for debug perpuses
            if (context.Request.Path.StartsWithSegments("/swagger", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            var path = context.Request.Path;
            // allow login for all
            if (context.Request.Path.StartsWithSegments("/login", StringComparison.OrdinalIgnoreCase))
            {
                await _next(context);
                return;
            }

            try
            {
                // extract id 
                context.Request.EnableBuffering();
                var bodyStr = await new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true).ReadToEndAsync();
                context.Request.Body.Position = 0;
                var tokenDTO = JsonConvert.DeserializeObject<TokenDTO>(bodyStr);


                if (tokenDTO == null || tokenDTO.Token == null)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized");
                    return;
                }

                var token = tokenDTO.Token;
                AuthId authId = TokenService._tokenIdPairs[token];
                
            }
            catch (Exception ex) {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsync("missing token");
                return ;
            }



            await _next(context);
        }
    }
}
