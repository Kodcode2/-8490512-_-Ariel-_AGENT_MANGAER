using MosadAPIServer.DTO;
using MosadAPIServer.Enums;
using MosadAPIServer.ModelsHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MosadAPIServer.Models
{
    public class Agent : IModel, ILocationable
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; }
        public int? LocationX { get; set; }
        public int? LocationY { get; set; }
        public string NickName { get; set; }
        public AgentStatus Status { get; set; } = AgentStatus.Idle;
        public int TotalKills { get; set; } = 0;
        List<Mission>? Missions { get; set; }

        public Agent() { }
        public Agent(AgentDTO agentDTO)
        {
            PhotoUrl = agentDTO.PhotoUrl;
            NickName = agentDTO.nickname;
        }

        public Location GetLocation()
        {
            return new Location(LocationX ?? 0,LocationY ?? 0);
        }
    }
}
