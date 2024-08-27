using System.Text.Json.Serialization;

namespace MosadAPIServer.DTO
{
    public class AgentDTO : IDTOModel
    {
        public string Token { get; set; }
        public string PhotoUrl { get; set; }
        public string nickname {  get; set; }
        
    }
}
