using System.Text.Json.Serialization;

namespace MosadMvcServer.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum TargetStatus
    {
        Alive,
        Terminated
    }
}