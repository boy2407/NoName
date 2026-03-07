using NoName.Application.Common;
using NoName.Application.Features.Product.Commands.Create;
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

        Task AddAsync(Product product, CancellationToken cancellationToken);

        Task UpdateAsync(Product product, CancellationToken cancellationToken);

        Task DeleteAsync(Product product, CancellationToken cancellationToken);
        IQueryable<Product> Query();
    
    }
}
