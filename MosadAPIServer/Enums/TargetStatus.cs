using System.Text.Json.Serialization;

namespace MosadAPIServer.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum TargetStatus
    {
        Alive,
        Terminated
    }
}