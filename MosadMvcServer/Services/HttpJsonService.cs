using MosadMvcServer.Models;
namespace MosadMvcServer.Services
{
    public class HttpJsonService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl = "https://localhost:7044/";

        public HttpJsonService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T?> GetAsync<T>(string path , int id)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}{path}/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<T>();
            return result;
        }

        public async Task<List<T>> GetAllAsync<T>(string path)
        {
            var response = await _httpClient.GetAsync($"{_baseUrl}{path}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<List<T>>();
            return result;
        }
    }
}
