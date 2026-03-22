using Microsoft.SemanticKernel;
using System.ComponentModel;
using NoName.Application.Abstractions;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace NoName.Infrastructure.AIPlugins
{
    public class AdminPlugin
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminPlugin(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [KernelFunction("get_revenue_by_date")]
        [Description("Lấy doanh thu theo ngày. Định dạng ngày: yyyy-MM-dd hoặc dd/MM/yyyy")]
        public async Task<string> GetRevenueByDate(
            [Description("Ngày cần xem, ví dụ: 2026-03-21 hoặc 21/03/2026")] string date)
        {
            if (!TryParseDate(date, out var parsedDate))
            {
                return "Ngày không hợp lệ. Vui lòng dùng yyyy-MM-ddhoặc dd/MM/yyyy.";
            }

            var revenue = await _unitOfWork.Orders.GetRevenueByDateAsync(parsedDate);
            return $"Doanh thu ngày {parsedDate:dd/MM/yyyy}: {revenue:N0} VNĐ";
        }

        [KernelFunction("get_order_count_by_date")]
        [Description("Lấy số lượng đơn hàng trong ngày. Định dạng ngày: yyyy-MM-dd hoặc dd/MM/yyyy")]
        public async Task<string> GetOrderCountByDate(
            [Description("Ngày cần xem, ví dụ: 2026-03-21 hoặc 21/03/2026")] string date)
        {
            if (!TryParseDate(date, out var parsedDate))
            {
                return "Ngày không hợp lệ. Vui lòng dùng yyyy-MM-dd hoặc dd/MM/yyyy.";
            }

            var count = await _unitOfWork.Orders.GetOrderCountByDateAsync(parsedDate);
            return $"Số đơn ngày {parsedDate:dd/MM/yyyy}: {count}";
        }

        [KernelFunction("get_revenue_by_month")]
        [Description("Lấy doanh thu theo tháng")]
        public async Task<string> GetRevenueByMonth(
            [Description("Năm, ví dụ: 2026")] int year,
            [Description("Tháng từ 1 đến 12")] int month)
        {
            if (year < 2000 || year > 2100 || month < 1 || month > 12)
            {
                return "Tham số không hợp lệ. Năm từ 2000-2100, tháng từ 1-12.";
            }

            var revenue = await _unitOfWork.Orders.GetRevenueByMonthAsync(year, month);
            return $"Doanh thu tháng {month:D2}/{year}: {revenue:N0} VNĐ";
        }

        [KernelFunction("get_revenue_by_quarter")]
        [Description("Lấy doanh thu theo quý")]
        public async Task<string> GetRevenueByQuarter(
            [Description("Năm, ví dụ: 2026")] int year,
            [Description("Quý từ 1 đến 4")] int quarter)
        {
            if (year < 2000 || year > 2100 || quarter < 1 || quarter > 4)
            {
                return "Tham số không hợp lệ. Năm từ 2000-2100, quý từ 1-4.";
            }

            var revenue = await _unitOfWork.Orders.GetRevenueByQuarterAsync(year, quarter);
            return $"Doanh thu quý {quarter}/{year}: {revenue:N0} VNĐ";
        }

        [KernelFunction("get_revenue_by_year")]
        [Description("Lấy doanh thu theo năm")]
        public async Task<string> GetRevenueByYear(
            [Description("Năm, ví dụ: 2026")] int year)
        {
            if (year < 2000 || year > 2100)
            {
                return "Năm không hợp lệ. Vui lòng nhập năm từ 2000 đến 2100.";
            }

            var revenue = await _unitOfWork.Orders.GetRevenueByYearAsync(year);
            return $"Doanh thu năm {year}: {revenue:N0} VNĐ";
        }

        private static bool TryParseDate(string input, out DateTime date)
        {
            return DateTime.TryParseExact(input, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out date)
                   || DateTime.TryParseExact(input, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
        }

    }
}