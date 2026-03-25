using System.ComponentModel;
using System.Text.Json;
using Microsoft.SemanticKernel;
using NoName.Application.Abstractions;
using NoName.Application.Features.Chatbot.DTOs;
using System.Linq;
using System.Threading.Tasks;

namespace NoName.Infrastructure.AIPlugins
{
    public class ProductPlugin
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductPlugin(IUnitOfWork unitOfWork)
        {   
            _unitOfWork = unitOfWork;
        }

        [KernelFunction("search_fashion_products")]
        [Description("Tìm kiếm sản phẩm thời trang theo nhiều tiêu chí (màu sắc, giá, chất liệu...)")]
        [return: Description("Danh sách sản phẩm tìm thấy dưới dạng JSON")]
        public async Task<string> SearchProductsAsync(
            [Description("Ngôn ngữ của người dùng (mặc định: vi-VN)")] string languageId = "vi-VN",
            [Description("Tên hoặc loại sản phẩm (ví dụ: Nam,Nữ,Áo thun, Quần jean)")] string? category = null,
            [Description("Màu sắc (ví dụ: Đen, Trắng)")] string? color = null,
            [Description("Chất liệu (ví dụ: Cotton, Lụa)")] string? material = null,
            [Description("Mức giá tối đa mong muốn")] decimal? maxPrice = null
        )
        {
            var criteria = new AiSearchCriteria
            {
                LanguageId = languageId,
                Category = category ?? "",
                Colors = string.IsNullOrEmpty(color) ? new List<string>() : new List<string> { color },
                Materials = string.IsNullOrEmpty(material) ? new List<string>() : new List<string> { material },
                MaxPrice = maxPrice
            };


            var products = await _unitOfWork.Products.SearchByAiCriteriaAsync(criteria);

            if (products == null || !products.Any())
            {
                return "Không tìm thấy sản phẩm nào phù hợp.";
            }

            var result = products.Select(p => new
            {
                ProductId = p.Id,
                Name = p.ProductTranslations.FirstOrDefault(x => x.LanguageId == languageId)?.Name ?? "Sản phẩm",
                PriceFrom = p.ProductVariants.Any() ? p.ProductVariants.Min(v => v.Price) : 0,
                PriceTo = p.ProductVariants.Any() ? p.ProductVariants.Max(v => v.Price) : 0
            });

            return JsonSerializer.Serialize(result);
        }

        [KernelFunction("get_stock_quantity")]
        [Description("Kiểm tra tổng số lượng tồn kho khả dụng của một sản phẩm theo tên")]
        public async Task<string> GetStockAsync(
            [Description("Tên chính xác của sản phẩm cần kiểm tra")] string productName
        )
        {
            var product = await _unitOfWork.Products.GetByNameAsync(productName);
            if (product == null)
            {
                return "Không tìm thấy sản phẩm.";
            }

            var variants = await _unitOfWork.ProductVariants.GetByProductIdAsync(product.Id, default);
            var available = variants
                .Where(v => v.Inventory != null)
                .Sum(v => v.Inventory.AvailableQuantity);

            var productNameVi = product.ProductTranslations.FirstOrDefault()?.Name ?? productName;
            return $"Tồn kho khả dụng của '{productNameVi}': {available}";
        }

        [KernelFunction("get_variant_price")]
        [Description("Lấy giá bán theo mã biến thể (VariantId)")]
        public async Task<string> GetVariantPriceAsync(
            [Description("Id của biến thể sản phẩm")] int variantId)
        {
            var variant = await _unitOfWork.ProductVariants.GetByIdAsync(variantId, default);
            if (variant == null)
            {
                return "Không tìm thấy biến thể sản phẩm.";
            }

            return $"Giá biến thể {variantId}: {variant.Price:N0} VNĐ";
        }
    }
}