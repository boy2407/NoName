using System.Net.Http.Json;
using NoName.Shared.DTOs.Categories;

namespace NoName.AdminApp.Services
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetAll();
        Task<CategoryDto> GetById(int id);
        Task<int> Create(CreateCategoryRequest request);
        Task<bool> Update(int id, UpdateCategoryRequest request);
        Task<bool> Delete(int id);
    }

    public class CategoryService(HttpClient httpClient) : ICategoryService
    {
        private const string BaseUrl = "api/categories";

        public async Task<List<CategoryDto>> GetAll()
        {
            var response = await httpClient.GetAsync($"{BaseUrl}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<CategoryDto>>() ?? new();
        }

        public async Task<CategoryDto> GetById(int id)
        {
            var response = await httpClient.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<CategoryDto>();
        }

        public async Task<int> Create(CreateCategoryRequest request)
        {
            var response = await httpClient.PostAsJsonAsync(BaseUrl, request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<CreateCategoryResponse>();
            return result.Id;
        }

        public async Task<bool> Update(int id, UpdateCategoryRequest request)
        {
            var response = await httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", request);
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Delete(int id)
        {
            var response = await httpClient.DeleteAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }
    }

    public class CreateCategoryRequest
    {
        public int? ParentId { get; set; }
        public List<CategoryTranslationDto> Translations { get; set; } = new();
    }

    public class UpdateCategoryRequest
    {
        public int? ParentId { get; set; }
        public List<CategoryTranslationDto> Translations { get; set; } = new();
    }

    public class CreateCategoryResponse
    {
        public int Id { get; set; }
    }
}
