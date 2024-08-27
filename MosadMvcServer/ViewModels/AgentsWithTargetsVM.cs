using MosadMvcServer.DTO;
using MosadMvcServer.Models;

namespace MosadMvcServer.ViewModels
{
    public class AgentsWithTargetsVM
    {
        public List<AgentWithMissionIdDTO> AgentsWithMissionId { get; set; }
        public List<Target> targets { get; set; }
    }
}
