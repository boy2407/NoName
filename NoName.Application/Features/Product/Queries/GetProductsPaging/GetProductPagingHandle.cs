using MediatR;
using Microsoft.EntityFrameworkCore;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Common;
using NoName.Application.Features.Product.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Product.Queries.GetProductsPaging
{
    public class GetProductPagingHandle : IRequestHandler<GetProductPaging, PageResult<ProductViewModel>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductPagingHandle(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<PageResult<ProductViewModel>> Handle(GetProductPaging request,CancellationToken cancellationToken)
        {
            var languageId = request.LanguageId ?? "vi";

            var query = _productRepository
                .Query()
                .AsNoTracking()
                .Where(p => p.IsActive);

            // Keyword filter
            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                var keyword = request.Keyword.Trim();

                query = query.Where(p =>
                    p.ProductTranslations.Any(t =>
                        t.LanguageId == languageId && t.Name.Contains(keyword)) ||

                    p.ProductInCategories.Any(pc =>
                        pc.Category.CategoryTranslations.Any(ct =>
                            ct.LanguageId == languageId && ct.Name.Contains(keyword)))
                );
            }

            // Category filter
            if (request.CategoryId.HasValue)
            {
                query = query.Where(p =>
                    p.ProductInCategories.Any(pc =>
                        pc.CategoryId == request.CategoryId));
            }

            var totalRecords = await query.CountAsync(cancellationToken);

            // Projection
            var productQuery = query.Select(p => new
            {
                Product = p,
                Translation = p.ProductTranslations.FirstOrDefault(t => t.LanguageId == languageId)
            });

            // Sorting
            if (request.SortBy?.Equals("price", StringComparison.OrdinalIgnoreCase) == true)
            {
                productQuery = request.IsDescending
                    ? productQuery.OrderByDescending(x => x.Product.Price)
                    : productQuery.OrderBy(x => x.Product.Price);
            }
            else
            {
                productQuery = request.IsDescending
                    ? productQuery.OrderByDescending(x => x.Translation!.Name)
                    : productQuery.OrderBy(x => x.Translation!.Name);
            }

            int pageIndex = request.PageIndex > 0 ? request.PageIndex : 1;
            int pageSize = request.PageSize > 0 ? request.PageSize : 10;

            var items = await productQuery
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ProductViewModel
                {
                    Id = x.Product.Id,
                    Name = x.Translation!.Name,
                    Description = x.Translation.Description,
                    Details = x.Translation.Details,
                    SeoAlias = x.Translation.SeoAlias,
                    Price = x.Product.Price,
                    OriginalPrice = x.Product.OriginalPrice,
                    Stock = x.Product.Stock,
                    CategoryIds = x.Product.ProductInCategories
                        .Select(pc => pc.CategoryId)
                        .ToList(),
                    LanguageId = languageId
                })
                .ToListAsync(cancellationToken);

            return new PageResult<ProductViewModel>
            {
                Items = items,
                TotalRecords = totalRecords,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

    }

}

