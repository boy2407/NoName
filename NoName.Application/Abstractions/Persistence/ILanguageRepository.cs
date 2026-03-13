using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Abstractions.Persistence
{
    public interface ILanguageRepository
    {
        Task<bool> CheckIfLanguageIsUsedAsync(string languageId);
        Task<List<Language>> GetAllAsync(CancellationToken ct = default);
        Task<Language?> GetByIdAsync(string id, CancellationToken ct = default);
        void Add(Language language);
        void Delete(Language language);
        Task<bool> ExistsAsync(string id, CancellationToken ct = default);
        Task<int> SaveChangesAsync(CancellationToken ct = default);
    }
}
