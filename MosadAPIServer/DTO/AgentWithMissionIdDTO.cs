using MosadAPIServer.Models;
namespace MosadAPIServer.DTO
{
    public class AgentWithMissionIdDTO : Agent
    {
        public int? AssignedMissionId { get; set; } = null;
        public TimeSpan? TimeToKill { get; set; } = null;

        public AgentWithMissionIdDTO() { }

        public AgentWithMissionIdDTO(Agent agent , int? assinedMissionId , TimeSpan? timeToKill)
        {
            Id = agent.Id;
            PhotoUrl = agent.PhotoUrl;
            LocationX = agent.LocationX;
            LocationY = agent.LocationY;
            NickName  = agent.NickName;
            Status = agent.Status;
            TotalKills = agent.TotalKills;
            Missions = agent.Missions;
            AssignedMissionId = assinedMissionId;
            TimeToKill = timeToKill;
        }
    }
}
