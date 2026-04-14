namespace NoName.Application.Abstractions.Services
{
    public interface IPaymentStatusNotifier
    {
        Task NotifyAsync(int orderId, bool isSuccess, string? message = null, CancellationToken cancellationToken = default);
    }
}
