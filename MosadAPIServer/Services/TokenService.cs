using MosadAPIServer.Enums;
using System.Xml.Linq;

namespace MosadAPIServer.Services
{
    public class TokenService
    {
        private readonly Dictionary<string,AuthId> _tokenIdPairs = new Dictionary<string, AuthId>()  { };   
        
        private readonly Dictionary<AuthId,List<string>> _idRoutesPairs = new Dictionary<AuthId,List<string>>() 
        { 

        };

        public TokenService() { }

        public string GenerateToken(string id)
        {
            var newToken = Guid.NewGuid().ToString();

            _tokenIdPairs[newToken] =   true switch =>
            {
                true => AuthId.SimulationServer,
            };

            if(Enum.TryParse(id, out AuthId result))
            {
                _tokenIdPairs[newToken] = result;
            }
            else
            {
                throw new InvalidCastException("id does not match AuthId option");
            }
        }
    }
}
