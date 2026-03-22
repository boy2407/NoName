using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Abstractions.Persistence
{
    public interface IOrderRepository
    {
        Task<Order?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<List<Order>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task<decimal> GetRevenueByDateAsync(DateTime date, CancellationToken ct = default);
        Task<decimal> GetRevenueByMonthAsync(int year, int month, CancellationToken ct = default);
        Task<decimal> GetRevenueByQuarterAsync(int year, int quarter, CancellationToken ct = default);
        Task<decimal> GetRevenueByYearAsync(int year, CancellationToken ct = default);
        Task<int> GetOrderCountByDateAsync(DateTime date, CancellationToken ct = default);
        Task AddAsync(Order order, CancellationToken ct = default);
        Task UpdateAsync(Order order, CancellationToken ct = default);
        Task DeleteAsync(Order order, CancellationToken ct = default);
    }
}
