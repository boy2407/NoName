using System.Net.Http.Json;
using NoName.Shared.DTOs.Products;
using NoName.Shared.DTOs.Products.Admin;
using NoName.Shared.DTOs.Products.Guest;

namespace NoName.AdminApp.Services
{
    public interface IProductService
    {
        Task<PaginatedResponse<ProductAdminDto>> GetProductsPaging(int pageNumber = 1, int pageSize = 10);
        Task<ProductAdminDto> GetProductById(int id);
        Task<int> CreateProduct(CreateProductRequest request);
        Task<bool> UpdateProduct(int id, UpdateProductRequest request);
        Task<bool> DeleteProduct(int id);
    }

    public class ProductService(HttpClient httpClient) : IProductService
    {
        private const string BaseUrl = "api/products";

        public async Task<PaginatedResponse<ProductAdminDto>> GetProductsPaging(int pageNumber = 1, int pageSize = 10)
        {
            var response = await httpClient.GetAsync($"{BaseUrl}?pageNumber={pageNumber}&pageSize={pageSize}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<PaginatedResponse<ProductAdminDto>>();
        }

        public async Task<ProductAdminDto> GetProductById(int id)
        {
            var response = await httpClient.GetAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProductAdminDto>();
        }

        public async Task<int> CreateProduct(CreateProductRequest request)
        {
            var response = await httpClient.PostAsJsonAsync(BaseUrl, request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<CreateProductResponse>();
            return result.Id;
        }

        public async Task<bool> UpdateProduct(int id, UpdateProductRequest request)
        {
            var response = await httpClient.PutAsJsonAsync($"{BaseUrl}/{id}", request);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResult<bool>>();
            return result.Data;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var response = await httpClient.DeleteAsync($"{BaseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<ApiResult<bool>>();
            return result.Data;
        }
    }

    // Request/Response DTOs
    public class CreateProductRequest
    {
        public List<int> CategoryIds { get; set; } = new();
        public List<ProductTranslationDto> Translations { get; set; } = new();
        public bool IsActive { get; set; } = true;
    }

    public class UpdateProductRequest
    {
        public List<int> CategoryIds { get; set; } = new();
        public List<ProductTranslationDto> Translations { get; set; } = new();
        public bool IsActive { get; set; } = true;
    }

    public class CreateProductResponse
    {
        public int Id { get; set; }
    }

    public class ApiResult<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }

    public class PaginatedResponse<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
