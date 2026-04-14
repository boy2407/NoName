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
    public class TransactionRepository : ITransactionRepository
    {
        private readonly NoNameDbContext _context;

        public TransactionRepository(NoNameDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Transaction transaction, CancellationToken ct = default)
        {
            await _context.Transactions.AddAsync(transaction, ct);
        }

        public async Task DeleteAsync(Transaction transaction, CancellationToken ct = default)
        {
            _context.Transactions.Remove(transaction);
            await Task.CompletedTask;
        }

        public async Task<Transaction?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Transactions.FindAsync(id, ct);
        }

        public async Task<List<Transaction>> GetByOrderIdAsync(int orderId, CancellationToken ct = default)
        {
            return await _context.Transactions.Where(x => x.OrderId == orderId).ToListAsync(ct);
        }

        public async Task<List<Transaction>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await _context.Transactions.Where(x => x.UserId == userId).ToListAsync(ct);
        }

        public async Task UpdateAsync(Transaction transaction, CancellationToken ct = default)
        {
            _context.Transactions.Update(transaction);
            await Task.CompletedTask;
        }
    }
}
