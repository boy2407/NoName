using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Features.Product.DTOs;

namespace NoName.Application.Features.Product.Queries.GetProductsByCategory
{
    public class GetProductsByCategoryHandler : IRequestHandler<GetProductsByCategory, List<ProductViewModel>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductsByCategoryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductViewModel>> Handle(GetProductsByCategory request, CancellationToken cancellationToken)
        {
            var languageId = request.LanguageId ?? "vi";

            var query = _productRepository.Query()
                .AsNoTracking()
                .Where(p => p.IsActive && p.ProductInCategories.Any(pc => pc.CategoryId == request.CategoryId));

            var productQuery = query.Select(p => new
            {
                Product = p,
                Translation = p.ProductTranslations.FirstOrDefault(t => t.LanguageId == languageId)
            });

            var items = await productQuery
                .Select(x => new ProductViewModel
                {
                    //Id = x.Product.Id,
                    //Name = x.Translation != null ? x.Translation.Name : string.Empty,
                    //Description = x.Translation != null ? x.Translation.Description : string.Empty,
                    //Details = x.Translation != null ? x.Translation.Details : string.Empty,
                    //SeoAlias = x.Translation != null ? x.Translation.SeoAlias : string.Empty,
                    //Price = x.Product.Price,
                    //OriginalPrice = x.Product.OriginalPrice,
                    //Stock = x.Product.Stock,
                    //CategoryIds = x.Product.ProductInCategories.Select(pc => pc.CategoryId).ToList(),
                    //LanguageId = languageId
                })
                .ToListAsync(cancellationToken);

            return items;
        }
    }
}
