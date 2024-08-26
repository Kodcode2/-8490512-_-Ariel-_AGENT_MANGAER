
using MosadMvcServer.Models;

namespace MosadMvcServer.Services
{
    public class ActionService
    {
        private readonly HttpJsonService _httpJsonService;
        public ActionService(HttpJsonService httpJsonService)
        {
            _httpJsonService = httpJsonService;
        }

        internal async Task AssignMission(int id)
        {
            string path = "missions";
            await _httpJsonService.PutAsync<Mission>(path,id);
        }
    }
}
