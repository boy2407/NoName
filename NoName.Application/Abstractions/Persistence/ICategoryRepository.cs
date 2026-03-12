using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Abstractions.Persistence
{
    public interface  ICategoryRepository
    {
        Task<List<Category>> GetAllAsync(string languageId, CancellationToken ct);
        Task<Category> GetByIdAsync(int id, string languageId, CancellationToken ct);
        Task CreateAsync(Category category, CancellationToken ct);
        void Update(Category category);
        void Delete(Category category);
        Task<int> SaveChangesAsync(CancellationToken ct);
    }
}
