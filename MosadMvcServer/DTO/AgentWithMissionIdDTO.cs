using MosadMvcServer.Models;
namespace MosadMvcServer.DTO
{
    public class AgentWithMissionIdDTO : Agent
    {
        public int? AssignedMissionId { get; set; } = null;
        public TimeSpan? TimeToKill { get; set; } = null;
    }
}
