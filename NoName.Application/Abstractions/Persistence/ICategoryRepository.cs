using NoName.Application.Features.Categories.Command.UpdateCategory;
using NoName.Application.Features.Categories.DTOs;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Abstractions.Persistence
{
    public interface ICategoryRepository
    {
     
        Task CreateAsync(Category category, CancellationToken ct = default);
        void DeleteAsync(Category category, CancellationToken ct = default);
        Task<bool> AreAllIdsExistAsync(List<int> ids, CancellationToken ct);
        Task<bool> ExistsAsync(int id, CancellationToken ct);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
        Task<Category> GetByIdWithTranslationsAsync(int id, CancellationToken ct);
        Task<CategoryViewModel> GetByIdWithLanguageAsync(int id, string languageId, CancellationToken ct);
        Task<List<Category>> GetAllAsync(string languageId, CancellationToken ct = default);
        Task<Category> GetByIdAsync(int id, CancellationToken ct = default);
        Task<List<Category>> GetByParentIdAsync(int? parentId, string languageId, CancellationToken ct);
        Task<bool> HasChildrenAsync(int id, CancellationToken ct);
        Task<List<int>> GetExistingIdsAsync(List<int> ids, CancellationToken ct);
    }

}
