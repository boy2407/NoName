using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Abstractions.Persistence
{
    public interface ICartRepository
    {
        Task<Cart?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<Cart?> GetByUserAndVariantAsync(Guid userId, int productVariantId, CancellationToken ct = default);
        Task<List<Cart>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task AddAsync(Cart cart, CancellationToken ct = default);
        Task UpdateAsync(Cart cart, CancellationToken ct = default);
        Task DeleteAsync(Cart cart, CancellationToken ct = default);
    }
}
