using MosadAPIServer.Data;

namespace MosadAPIServer.Services
{
    public class MissionService
    {
        private readonly MosadAPIServerContext _context;

        public MissionService(MosadAPIServerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// indicates that a agent moved  calculates if there are targets for agent
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task IdleAgentMoved(int agentId)
        {
            // run generateMissions
            throw new NotImplementedException();
        }


        /// <summary>
        /// indicates that a target moved , runs some logic based on that
        /// </summary>
        /// <param name="id">the target id</param>
        /// <exception cref="NotImplementedException"></exception>
        internal void TargetMoved(int id)
        {
            
            // check if connected to mission , if not run generateMissions
            throw new NotImplementedException();
        }

        internal void UpdateMissions()
        {
            throw new NotImplementedException();
        }
    }
}
