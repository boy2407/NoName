using Microsoft.EntityFrameworkCore;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Common;
using NoName.Application.Features.Product.DTO;
using NoName.Application.Models;
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
        //private readonly IStorageService _storageService;
        //private const string USER_CONTENT_FOLDER_NAME = "user-content";

        public ProductRepository(NoNameDbContext context)
        {
            _context = context;
        }

        public IQueryable<Product> Query() => _context.Products.AsQueryable();
        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
          
            return await _context.SaveChangesAsync(ct);
        }
        public async Task AddAsync(Product product, CancellationToken cancellationToken)
        {
            await _context.Products.AddAsync(product, cancellationToken);
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(Product product, CancellationToken cancellationToken)
        {
            _context.Products.Update(product);
        }

        public async Task DeleteAsync(Product product, CancellationToken cancellationToken)
        {
            _context.Products.Remove(product);
        }


    }
}
