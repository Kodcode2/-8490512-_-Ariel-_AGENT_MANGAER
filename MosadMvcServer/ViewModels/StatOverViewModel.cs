namespace MosadMvcServer.ViewModels
{
    public class StatOverViewModel
    {
        public int TotalAgents { get; set; }
        public int ActiveAgents { get; set; }
        public int totalTargets { get; set; }
        public int activeTargets { get; set; }
        public int totalMissions { get; set; }
        public int activeMissions { get; set; }
        public int idleAgentCount { get; set; }
        public int unassignedTargetCount { get; set; }

    }
}
