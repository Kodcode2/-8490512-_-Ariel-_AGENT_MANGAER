using MosadAPIServer.Enums;
using MosadAPIServer.Exceptions;
using System.Collections.Concurrent;
using System.Xml.Linq;

namespace MosadAPIServer.Services
{
    public  class TokenService 
    {
        public static readonly ConcurrentDictionary<string, AuthId> _tokenIdPairs = [];


        public TokenService() 
        {
            _tokenIdPairs["debug"] = AuthId.SimulationServer;
        }
        public static string GenerateToken(string id)
        {
            var newToken = Guid.NewGuid().ToString();


            bool parsed = false;
            foreach (var en in Enum.GetNames(typeof(AuthId)))
            {
                if(Enum.TryParse(id, out AuthId result))
                {
                    _tokenIdPairs[newToken] = result;
                    parsed = true;
                    break;
                }
            }

            if(!parsed)
                throw new UnauthorizedIdException();

            return newToken;
        }

        
    }
}
