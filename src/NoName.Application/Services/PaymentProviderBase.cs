using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NoName.Application.Abstractions;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Abstractions.Services;
using NoName.Application.Common;
using NoName.Domain.Entities;
using NoName.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Services
{
    public abstract class PaymentProviderBase : IPaymentService
    {
        protected readonly ILogger _logger;
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IConfiguration _configuration;
        protected readonly IHttpClientFactory _httpClientFactory;

        protected PaymentProviderBase(ILogger logger, IUnitOfWork unitOfWork, IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public abstract string ProviderName { get; }

        public async Task<string> CreatePaymentAsync(Order order)
        {
            _logger.LogInformation("[{0}] Đang tạo yêu cầu thanh toán cho đơn hàng {1}", this.ProviderName, order.Id);

            try
            {
                var result = await BuildProviderSpecificUrl(order);
                await _unitOfWork.Transactions.AddAsync(new Transaction
                {
                    OrderId = order.Id,
                    UserId = order.UserId,
                    Amount = order.TotalAmount,
                    Provider = this.ProviderName, 
                    Status = TransactionStatus.Pending,
                    TransactionDate = DateTime.UtcNow,
                    PayUrl = result.PayUrl,
                    Message = result.Message,
                    ExternalTransactionId = result.RequestId,
                    Result = result.ResultCode == 0 ? "Success" : "Failed",
                    Fee = 0 

                });

                await _unitOfWork.SaveChangesAsync();
                return result.PayUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[{0}] Lỗi khi tạo thanh toán cho đơn hàng {1}", this.ProviderName, order.Id);
                throw;
            }
        }
        protected abstract Task<MomoResponse> BuildProviderSpecificUrl(Order order);

        public abstract Task<bool> ValidateCallback(IDictionary<string, string> queryParams);

    }
}
