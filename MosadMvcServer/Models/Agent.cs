
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using MosadMvcServer.ModelsHelpers;
using MosadMvcServer.Enums;

namespace MosadMvcServer.Models
{
    public class Agent : IModel, ILocationable
    {
        public int Id { get; set; }
        public string PhotoUrl { get; set; }
        public int? LocationX { get; set; } = null;
        public int? LocationY { get; set; } = null;
        public string NickName { get; set; }
        public AgentStatus Status { get; set; } = AgentStatus.Idle;
        public int TotalKills { get; set; } = 0;

       /* [JsonIgnore]*/
        public List<Mission>? Missions { get; set; }

        public Agent() { }
       

        public Location? GetLocation()
        {
            if (LocationX == null || LocationY == null) return null;

            return new Location(LocationX ,LocationY );
        }
        public void SetLocation(Location newLocation)
        {
            LocationX = newLocation.X;
            LocationY = newLocation.Y;
        }

        public override bool Equals(object? obj)
        {
            var other = obj as Agent;

            if (other == null) return false;

            return Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
