using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MosadMvcServer.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]

    public enum MissionStatus
    {
        OpenForAssignment,
        Assigned,
        Finished
    }
}
