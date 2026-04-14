using System.Net.Http.Json;
using NoName.Shared.DTOs.Languages;

namespace NoName.AdminApp.Services
{
    public interface ILanguageService
    {
        Task<List<LanguageDto>> GetAll();
        Task<LanguageDto> GetById(string id);
        Task<bool> Create(CreateLanguageRequest request);
        Task<bool> Update(string id, UpdateLanguageRequest request);
        Task<bool> Delete(string id);
    }

    public class LanguageService(HttpClient httpClient) : ILanguageService
    {
        private const string BaseUrl = "api/languages";

        public async Task<List<LanguageDto>> GetAll()
        {
            var response = await httpClient.GetAsync($"{BaseUrl}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<LanguageDto>>() ?? new();
        }

        public async Task<LanguageDto> GetById(string id)
        {
            var response = await httpClient.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<LanguageDto>();
        }

        public async Task<bool> Create(CreateLanguageRequest request)
        {
            var response = await httpClient.PostAsJsonAsync(BaseUrl, request);
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Update(string id, UpdateLanguageRequest request)
        {
            var response = await httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", request);
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(string id)
        {
            var response = await httpClient.DeleteAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }
    }

    public class CreateLanguageRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateLanguageRequest
    {
        public string Name { get; set; }
    }
}
