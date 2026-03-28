using NoName.Application.Common;
using NoName.Application.Features.Products.Queries.GetProductsPaging;
using NoName.Application.Features.Products.Commands.Create;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NoName.Shared.DTOs.Products.Guest;
using NoName.Shared.DTOs.Chatbot;

namespace NoName.Application.Abstractions.Persistence
{
    public interface IProductRepository
    {
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<PagedResult<ProductViewDto>> GetProductsPagingAsync(GetProductsPagingQuery request, CancellationToken ct = default);
        Task<Product> GetProductForUpdateAsync(int id, CancellationToken ct);
        Task<T> GetByIdWithDetailsAsync<T>(int id, string languageId, CancellationToken cancellationToken) where T : class;
        Task<Product?> GetProductWithImagesAsync(int id, CancellationToken ct);
        Task AddAsync(Product product, CancellationToken cancellationToken);
        Task UpdateAsync(Product product, CancellationToken cancellationToken);
        Task DeleteAsync(Product product, CancellationToken cancellationToken);
        Task <bool> ExistsAsync(int id, CancellationToken cancellationToken);
        IQueryable <Product> Query();
        IQueryable<Product> GetProductQuery(int id);
        Task<bool> CheckSkuExistsAsync(string sku, CancellationToken cancellationToken);


        //-------------- AI 
        Task<List<Product>> SearchByAiCriteriaAsync(AiSearchCriteria criteria);
        Task<Product?> GetByNameAsync(string productName);
    }
}
