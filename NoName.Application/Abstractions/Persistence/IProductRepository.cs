using NoName.Application.Common;
using NoName.Application.Features.Product.Queries.GetProductsPaging;
using NoName.Application.Features.Products.Commands.Create;
using NoName.Application.Features.Products.DTOs.Guest;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Abstractions.Persistence
{
    public interface IProductRepository
    {
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<PagedResult<ProductViewModel>> GetProductsPagingAsync(GetProductsPagingRequest request, CancellationToken ct = default);
        Task<Product> GetProductForUpdateAsync(int id, CancellationToken ct);
        Task<T> GetByIdWithDetailsAsync<T>(int id, string languageId, CancellationToken cancellationToken) where T : class;
        Task<Product?> GetProductWithImagesAsync(int id, CancellationToken ct);
        Task AddAsync(Product product, CancellationToken cancellationToken);
        Task UpdateAsync(Product product, CancellationToken cancellationToken);
        Task DeleteAsync(Product product, CancellationToken cancellationToken);
        Task <bool> ExistsAsync(int id, CancellationToken cancellationToken);
        IQueryable <Product> Query();
        Task<bool> CheckSkuExistsAsync(string sku, CancellationToken cancellationToken);
    }
}
