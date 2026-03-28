using Microsoft.EntityFrameworkCore;
using NoName.Application.Abstractions.Persistence;
using NoName.Domain.Entities;
using NoName.Domain.Enums;
using NoName.Infrastructure.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Infrastructure.Persistence
{
    public class OrderRepository : IOrderRepository
    {
        private readonly NoNameDbContext _context;

        public OrderRepository(NoNameDbContext context)
        {
            _context = context;
        }

        public async Task<Order?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            return await _context.Orders
                .Include(x => x.OrderDetails)
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<List<Order>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            return await _context.Orders
                .AsNoTracking()
                .Include(x => x.OrderDetails)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.OrderDate)
                .ToListAsync(ct);
        }

        public async Task<decimal> GetRevenueByDateAsync(DateTime date, CancellationToken ct = default)
        {
            var start = date.Date;
            var end = start.AddDays(1);

            return await GetRevenueByRangeAsync(start, end, ct);
        }

        public async Task<decimal> GetRevenueByMonthAsync(int year, int month, CancellationToken ct = default)
        {
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);

            return await GetRevenueByRangeAsync(start, end, ct);
        }

        public async Task<decimal> GetRevenueByQuarterAsync(int year, int quarter, CancellationToken ct = default)
        {
            var startMonth = (quarter - 1) * 3 + 1;
            var start = new DateTime(year, startMonth, 1);
            var end = start.AddMonths(3);

            return await GetRevenueByRangeAsync(start, end, ct);
        }

        public async Task<decimal> GetRevenueByYearAsync(int year, CancellationToken ct = default)
        {
            var start = new DateTime(year, 1, 1);
            var end = start.AddYears(1);

            return await GetRevenueByRangeAsync(start, end, ct);
        }

        private async Task<decimal> GetRevenueByRangeAsync(DateTime start, DateTime end, CancellationToken ct)
        {
            var total = await _context.Orders
                .Where(x => x.OrderDate >= start && x.OrderDate < end && x.Status != OrderStatus.Canceled)
                .SelectMany(x => x.OrderDetails)
                .SumAsync(x => (decimal?)x.Price * x.Quantity, ct);

            return total ?? 0m;
        }

        public async Task<int> GetOrderCountByDateAsync(DateTime date, CancellationToken ct = default)
        {
            var start = date.Date;
            var end = start.AddDays(1);

            return await _context.Orders
                .CountAsync(x => x.OrderDate >= start && x.OrderDate < end && x.Status != OrderStatus.Canceled, ct);
        }

        public async Task AddAsync(Order order, CancellationToken ct = default)
        {
            await _context.Orders.AddAsync(order, ct);
        }

        public async Task UpdateAsync(Order order, CancellationToken ct = default)
        {
            _context.Orders.Update(order);
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(Order order, CancellationToken ct = default)
        {
            _context.Orders.Remove(order);
            await Task.CompletedTask;
        }
    }
}
