using System.Net.Http.Json;
using NoName.Shared.DTOs.Products.Admin;

namespace NoName.AdminApp.Services
{
    public interface IProductVariantService
    {
        Task<List<ProductVariantAdminDto>> GetAll();
        Task<bool> Delete(int productId, int variantId);
    }

    public class ProductVariantService(HttpClient httpClient) : IProductVariantService
    {
        private const string BaseUrl = "api/products";

        public async Task<List<ProductVariantAdminDto>> GetAll()
        {
            var response = await httpClient.GetAsync($"{BaseUrl}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<List<ProductVariantAdminDto>>() ?? new();
        }

        public async Task<bool> Delete(int productId, int variantId)
        {
            var response = await httpClient.DeleteAsync($"{BaseUrl}/{productId}/variants/{variantId}");
            response.EnsureSuccessStatusCode();
            return response.IsSuccessStatusCode;
        }
    }
}
