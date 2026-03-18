using Microsoft.EntityFrameworkCore;
using NoName.Application.Abstractions.Persistence;
using NoName.Domain.Entities;
using NoName.Infrastructure.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Infrastructure.Persistence
{
    public class ProductVariantRepository : IProductVariantRepository
    {
        private readonly NoNameDbContext _context;
        public ProductVariantRepository(NoNameDbContext context) => _context = context;

        public async Task<List<ProductVariant>> GetByProductIdAsync(int productId, CancellationToken ct)
        {
            return await _context.ProductVariants
                .Where(x => x.ProductId == productId)
                .Include(x =>x.Inventory)
                .ToListAsync(ct);
        }

       public Task<List<string>> GetExistingSKUsAsync(List<string> SKUs, CancellationToken ct)
        {
            return _context.ProductVariants
                .Select(x => x.SKU)
                .Where(sku => SKUs.Contains(sku))
                .ToListAsync(ct);
        }

        public void Add(ProductVariant variant) => _context.ProductVariants.Add(variant);

        public void Remove(ProductVariant variant) => _context.ProductVariants.Remove(variant);
    }
}
