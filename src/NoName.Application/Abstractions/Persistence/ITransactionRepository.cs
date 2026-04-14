using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Abstractions.Persistence
{
    public interface  ITransactionRepository
    {
        Task<Transaction?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<List<Transaction>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task<List<Transaction>> GetByOrderIdAsync(int orderId, CancellationToken ct = default);
        Task AddAsync(Transaction transaction, CancellationToken ct = default);
        Task UpdateAsync(Transaction transaction, CancellationToken ct = default);
        Task DeleteAsync(Transaction transaction, CancellationToken ct = default);
    }
}
