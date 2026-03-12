using Microsoft.EntityFrameworkCore;
using NoName.Application.Abstractions.Persistence;
using NoName.Infrastructure.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoName.Domain.Entities;
namespace NoName.Infrastructure.Persistence
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly NoNameDbContext _context;

        public LanguageRepository(NoNameDbContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckIfLanguageIsUsedAsync(string languageId)
        {
          
            var isUsedInProduct = await _context.ProductTranslations.AnyAsync(x => x.LanguageId == languageId);
            var isUsedInCategory = await _context.CategoryTranslations.AnyAsync(x => x.LanguageId == languageId);

            return isUsedInProduct || isUsedInCategory;
        }
        public IQueryable<Language> Query() => _context.Languages.AsQueryable();
        public async Task AddAsync(Language language, CancellationToken ct = default)
        {
            await _context.Languages.AddAsync(language, ct);
        }

        public async Task<bool> ExistsAsync(string id, CancellationToken ct = default)
        {
            return await _context.Languages.AnyAsync(l => l.Id == id, ct);
        }

       public async Task DeleteAsync (Language language, CancellationToken ct = default)
        {
            _context.Languages.Remove(language);
           await _context.SaveChangesAsync(ct);
        }
        public async Task<List<Language>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Languages.ToListAsync(ct);
        }

        public async Task<Language?> GetByIdAsync(string id, CancellationToken ct = default)
        {
            return await _context.Languages.FindAsync(new object[] { id }, ct);
        }
        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }
    }
}
