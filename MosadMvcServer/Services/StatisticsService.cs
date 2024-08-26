
using MosadMvcServer.Models;
using MosadMvcServer.Enums;
using System.Collections.Generic;
using MosadMvcServer.DTO;

namespace MosadMvcServer.Services
{
    public class StatisticsService
    {
        private readonly HttpJsonService _httpJsonService;
        public StatisticsService(HttpJsonService httpJsonService)
        {
            _httpJsonService = httpJsonService;
        }

        internal async Task<(int, int)> GetAgentsCount()
        {
            string path = "agents";

            var list = await _httpJsonService.GetAllAsync<Agent>(path);

            var agentsCount = list.Count();
            var activeCount = list.Count(a=>a.Status == AgentStatus.Active);

            return ( agentsCount,  activeCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>(targetsCount, aliveCount)</returns>
        internal async Task<(int, int)> GetTargetCount()
        {
            string path = "targets";

            var list = await _httpJsonService.GetAllAsync<Target>(path);

            var targetsCount = list.Count();
            var aliveCount = list.Count(t => t.Status == TargetStatus.Alive);

            return (targetsCount, aliveCount);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>( missionCount,  assinedCount)</returns>
        internal async Task<(int, int)> GetMissionCount()
        {

            var list = await GetAllMissions();

            var missionCount = list.Count();
            var assinedCount = list.Count(m => m.Status == MissionStatus.Assigned);

            return ( missionCount,  assinedCount);
        }
       

        private async Task<List<Mission>> GetAllMissions(bool fullInfo = false)
        {
            string path = "missions" + (fullInfo? "/fullInfo": "");

            var list = await _httpJsonService.GetAllAsync<Mission>(path);

            return list;
        }




        /// <summary>
        /// 
        /// </summary>
        /// <returns>(distinctAgentCount, distinctTargetCount)</returns>
        internal async Task<(int, int)> GetCompatiblePairsCount()
        {
            var missions = await GetAllMissions(true);

            var openForAssignmentMissions = missions.Where(m=>m.Status == MissionStatus.OpenForAssignment);

            var ofa = openForAssignmentMissions;

            var distinctAgentCount = (from mission in ofa select mission.Agent).Distinct().Count();

            var distinctTargetCount = (from mission in ofa select mission.Target).Distinct().Count();

            return (distinctAgentCount, distinctTargetCount);
        }

        internal async Task<List<AgentWithMissionIdDTO>> GetAgentsWithMissionId()
        {
            string path = "agents/agentsWithMissionId";

            var list = await _httpJsonService.GetAllAsync<AgentWithMissionIdDTO>(path);

            return list;
        }

        internal async Task<Mission> GetMission(int id)
        {
            string path = "missions";
            var mission = await _httpJsonService.GetAsync<Mission>(path,id);
            return mission;
        }

        internal async Task<List<Target>> GetAllTargets()
        {
            string path = "targets";

            var list = await _httpJsonService.GetAllAsync<Target>(path);

            return (list);
        }
    }
}
