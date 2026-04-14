using AutoMapper;
using Azure.Core;
using Microsoft.EntityFrameworkCore;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Features.Categories.Commands.UpdateCategory;
using NoName.Domain.Entities;
using NoName.Infrastructure.EF;
using NoName.Shared.DTOs.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Infrastructure.Persistence
{
    public class CategoryRepository: ICategoryRepository
    {
        private readonly NoNameDbContext _context;
        private readonly IMapper _mapper;
        public CategoryRepository(NoNameDbContext context, IMapper mapper) { _context = context; _mapper = mapper; }
        
     
       public  async Task CreateAsync(Category category, CancellationToken ct){
             await _context.Categories.AddAsync(category, ct);
        }
        public  async void DeleteAsync(Category category,CancellationToken ct){

            _context.Categories.Remove(category);
            //await _context.Categories.Where(c => c.Id == category.Id).ExecuteDeleteAsync();
        }
        public async Task<bool> AreAllIdsExistAsync(List<int> ids, CancellationToken ct)
        {
            if (ids == null || !ids.Any()) return true;

            var count = await _context.Categories
                .Where(x => ids.Contains(x.Id))
                .CountAsync(ct);

            return count == ids.Distinct().Count();
        }
        public async Task<List<Category>> GetAllAsync(string languageId, CancellationToken ct)
        {

            return await _context.Categories
                .Include(x => x.CategoryTranslations.Where(t => t.LanguageId == languageId))
                .Include(x => x.ChildCategories)
                .AsNoTracking()
                .ToListAsync();
        }
        public async Task<bool> ExistsAsync(int id, CancellationToken ct)
        {
            return await _context.Categories.AnyAsync(x => x.Id == id, ct);
        }

        public async Task<List<Category>> GetByParentIdAsync(int? parentId, string languageId, CancellationToken ct)
        {
            return await _context.Categories
                .AsNoTracking()
                .Include(x => x.CategoryTranslations.Where(t => t.LanguageId == languageId))
                .Where(x => x.ParentId == parentId)
                .OrderBy(x => x.SortOrder)
                .ToListAsync(ct);
        }

        public async Task<Category?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _context.Categories
                .Include(x => x.CategoryTranslations)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }
        public async Task<bool> HasChildrenAsync(int id, CancellationToken ct)
        {
           
            return await _context.Categories
                .AnyAsync(x => x.ParentId == id, ct);
        }

        public async Task<Category> GetByIdWithTranslationsAsync(int id, CancellationToken ct)
        {
          return await _context.Categories.Include(x => x.CategoryTranslations).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
   
        }
        public async Task<CategoryDto> GetByIdWithLanguageAsync(int id, string languageId, CancellationToken ct)
        {
            var category = await _context.Categories
                .Include(x => x.CategoryTranslations)
                .AsNoTracking() 
                .FirstOrDefaultAsync(x => x.Id == id, ct);
            return _mapper.Map<CategoryDto>(category, opt => opt.Items["LanguageId"] = languageId);
        }

        public async Task<List<int>> GetExistingIdsAsync(List<int> ids, CancellationToken ct)
        {
            return await _context.Categories
                .Where(x => ids.Contains(x.Id))
                .Select(x => x.Id)
                .ToListAsync(ct);
        }

        public  async Task<int> SaveChangesAsync(CancellationToken ct){
            return await _context.SaveChangesAsync(ct);
        }
    }
}
