using Microsoft.EntityFrameworkCore;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Common;
using NoName.Application.Features.Product.DTOs;
using NoName.Domain.Entities;
using NoName.Infrastructure.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Infrastructure.Persistence
{
    public class ProductRepository : IProductRepository
    {
        private readonly NoNameDbContext _context;

        public ProductRepository(NoNameDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(Product product, CancellationToken cancellationToken)
        {
            await _context.Products.AddAsync(product, cancellationToken);
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        public async Task<Product?> GetProductWithImagesAsync(int id, CancellationToken ct)
        {
            return await _context.Products
                .Include(p => p.ProductTranslations)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<ProductViewModel> GetByIdWithDetailsAsync(int id, string languageId, CancellationToken ct)
        {
            var query = _context.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductViewModel
                {
                    Id = p.Id,
                    Price = p.Price,
                    OriginalPrice = p.OriginalPrice,
                    Stock = p.Stock,

                    productTranslation = p.ProductTranslations
                        .Where(t => t.LanguageId == languageId)
                        .Select(t => new ProductTranslationViewModel
                        {
                            Name = t.Name,
                            Description = t.Description,
                            Details = t.Details,
                            SeoDescription = t.SeoDescription,
                            SeoTitle = t.SeoTitle,
                            SeoAlias = t.SeoAlias,
                            LanguageId = t.LanguageId
                        }).FirstOrDefault()
                        ?? p.ProductTranslations 
                        .Where(t => t.LanguageId == "vi-VN")
                        .Select(t => new ProductTranslationViewModel
                        {
                            Name = t.Name,
                            Description = t.Description,
                            Details = t.Details,
                            SeoDescription = t.SeoDescription,
                            SeoTitle = t.SeoTitle,
                            SeoAlias = t.SeoAlias,
                            LanguageId = t.LanguageId
                        }).FirstOrDefault(),

                    ThumbnailImage = p.ProductImages.Where(i => i.IsDefault).Select(i => i.ImagePath).FirstOrDefault(),
                    GalleryImages = p.ProductImages.Where(i => !i.IsDefault).Select(i => i.ImagePath).ToList(),

                    CategoryNames = p.ProductInCategories
                        .Select(pc => pc.Category.CategoryTranslations
                            .Where(ctran => ctran.LanguageId == languageId)
                            .Select(ctran => ctran.Name).FirstOrDefault()
                            ?? pc.Category.CategoryTranslations
                            .Where(ctran => ctran.LanguageId == "vi-VN")
                            .Select(ctran => ctran.Name).FirstOrDefault())
                        .ToList()
                })
                .FirstOrDefaultAsync(ct);


         return await query;
        }

        public async Task UpdateAsync(Product product, CancellationToken cancellationToken)
        {
            // If the entity is already being tracked, update its current values
            var tracked = _context.Products.Local.FirstOrDefault(p => p.Id == product.Id);
            if (tracked == null)
            {
                // Attach and mark modified so changes are saved on SaveChangesAsync
                _context.Products.Attach(product);
                _context.Entry(product).State = EntityState.Modified;
            }
            else
            {
                // Copy values to the tracked entity to avoid duplicate tracking
                _context.Entry(tracked).CurrentValues.SetValues(product);
            }

            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Product product, CancellationToken cancellationToken)
        {
            _context.Products.Remove(product);
        }
        public Task<bool> ExistsAsync(int id, CancellationToken cancellationToken)
        {
            return _context.Products.AnyAsync(x => x.Id == id, cancellationToken);
        }

        public IQueryable<Product> Query() => _context.Products.AsQueryable();
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {

            return await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
