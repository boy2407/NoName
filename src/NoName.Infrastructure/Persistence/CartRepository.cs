using Microsoft.EntityFrameworkCore;
using NoName.Application.Abstractions.Persistence;
using NoName.Domain.Entities;
using NoName.Infrastructure.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Infrastructure.Persistence
{
    public class CartRepository : ICartRepository
    {
        private readonly NoNameDbContext _context;

        public CartRepository(NoNameDbContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Carts
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<Cart?> GetByUserAndVariantAsync(Guid userId, int productVariantId, CancellationToken ct = default)
        {
            return await _context.Carts
                .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductVariantId == productVariantId, ct);
        }

        public async Task<List<Cart>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await _context.Carts
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.DateCreated)
                .ToListAsync(ct);
        }

        public async Task AddAsync(Cart cart, CancellationToken ct = default)
        {
            await _context.Carts.AddAsync(cart, ct);
        }

        public async Task UpdateAsync(Cart cart, CancellationToken ct = default)
        {
            _context.Carts.Update(cart);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Cart cart, CancellationToken ct = default)
        {
            _context.Carts.Remove(cart);
            await Task.CompletedTask;
        }
    }
}
