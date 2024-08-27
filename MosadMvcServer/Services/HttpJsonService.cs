using MosadMvcServer.Models;
using Newtonsoft.Json;
using System.Text;
using MosadMvcServer.DTO;
using System.IO;

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

        public async Task<TokenDTO> GetToken(string id)
        {
            var json = JsonConvert.SerializeObject(new IdForTokenDTO() { Id = id }, Formatting.Indented);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_baseUrl}login",content);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<TokenDTO>();
            return result;
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

        public async Task PutAsync<T>(string path, int id)
        {
            var json = JsonConvert.SerializeObject(new TokenDTO() { Token = TokenService.Token }, Formatting.Indented);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{_baseUrl}{path}/{id}", content);
            response.EnsureSuccessStatusCode();
        }
    }
}
