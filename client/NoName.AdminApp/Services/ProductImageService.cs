using System.Net.Http.Json;

namespace NoName.AdminApp.Services
{
    public interface IProductImageService
    {
        Task<List<ProductImageDto>> GetAll();
        Task<bool> Delete(int id);
    }

    public class ProductImageService(HttpClient httpClient) : IProductImageService
    {
        private const string BaseUrl = "api/products";

        public async Task<List<ProductImageDto>> GetAll()
        {
            var response = await httpClient.GetAsync($"{BaseUrl}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ProductImageDto>>() ?? new();
        }

        public async Task<bool> Delete(int id)
        {
            var response = await httpClient.DeleteAsync($"{BaseUrl}/images/{id}");
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }
    }

    public class ProductImageDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsThumbnail { get; set; }
        public string ProductName { get; set; }
    }
}
