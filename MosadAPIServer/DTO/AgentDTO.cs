using System.Text.Json.Serialization;

namespace MosadAPIServer.DTO
{
    public class AgentDTO : IDTOModel
    {
        public string Token { get; set; }
        [JsonPropertyName("photo_url")]
        public string PhotoUrl { get; set; }
        public string nickname {  get; set; }
        
    }
}
