using Microsoft.AspNetCore.SignalR;
using NoName.Application.Abstractions.Services;
using NoName.BackendApi.Hubs;

namespace NoName.BackendApi.Services
{
    public class PaymentStatusNotifier(IHubContext<PaymentStatusHub> hubContext) : IPaymentStatusNotifier
    {
        public async Task NotifyAsync(int orderId, bool isSuccess, string? message = null, CancellationToken cancellationToken = default)
        {
            await hubContext.Clients
                .Group(PaymentStatusHub.GetGroupName(orderId.ToString()))
                .SendAsync(
                    "PaymentStatusUpdated",
                    new
                    {
                        OrderId = orderId,
                        IsSuccess = isSuccess,
                        Message = message,
                        UpdatedAt = DateTime.UtcNow
                    },
                    cancellationToken);
        }
    }
}
