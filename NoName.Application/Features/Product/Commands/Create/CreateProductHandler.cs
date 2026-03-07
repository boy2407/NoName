using MediatR;
using NoName.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NoName.Domain.Entities;
using System.Linq;
namespace NoName.Application.Features.Product.Commands.Create
{
    public class CreateProductHandler : IRequestHandler<CreateProduct, int>
    {
        private readonly IProductRepository _productRepository;
        public CreateProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<int> Handle(CreateProduct request, CancellationToken ct)
        {
            // 1. Tạo Entity Product chính
            var product = new NoName.Domain.Entities.Product
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                DateCreated = DateTime.Now,
                IsActive = true,
                ViewCount = 0,

                // 2. Chỉ thêm DUY NHẤT một bản dịch vào List
                ProductTranslations = new List<ProductTranslation>
            {
                new ProductTranslation
                {
                    Name = request.Name,
                    Description = request.Description,
                    Details = request.Details,
                    SeoAlias = request.SeoAlias,
                    LanguageId = request.LanguageId // Lấy trực tiếp từ Request
                }
            },

                // Map danh mục
                ProductInCategories = request.CategoryIds.Select(cId => new ProductInCategory
                {
                    CategoryId = cId
                }).ToList()
            };

            // 3. Chuyển cho Repository lưu
            await _productRepository.AddAsync(product, ct);
            await _productRepository.SaveChangesAsync(ct);

            return product.Id;
        }
    }
}
