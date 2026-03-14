using NoName.Application.Common;
using NoName.Application.Features.Products.Commands.Create;
using NoName.Application.Features.Product.DTOs;
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
        Task<ProductViewModel> GetByIdWithDetailsAsync(int id,string languageId, CancellationToken cancellationToken);
        Task<Product?> GetProductWithImagesAsync(int id, CancellationToken ct);
        Task AddAsync(Product product, CancellationToken cancellationToken);
        Task UpdateAsync(Product product, CancellationToken cancellationToken);
        Task DeleteAsync(Product product, CancellationToken cancellationToken);
        Task <bool> ExistsAsync(int id, CancellationToken cancellationToken);
        IQueryable <Product> Query();
    
    }
}
