using System.Text.Json.Serialization;

namespace MosadAPIServer.DTO
{
    public interface IDTOModel
    {
        public string Token { get; set; }
        [JsonPropertyName("photo_url")]
        public string PhotoUrl { get; set; }
    }
}
