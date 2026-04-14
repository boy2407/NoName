using MediatR;
using Microsoft.Extensions.Logging;
using NoName.Application.Abstractions;
using NoName.Application.Abstractions.Services;
using NoName.Domain.Enums;

namespace NoName.Application.Features.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommandHandler(
        IEnumerable<IPaymentService> paymentServices,
        IUnitOfWork unitOfWork,
        ILogger<CreatePaymentCommandHandler> logger) 
        : IRequestHandler<CreatePaymentCommand, string>
    {
        public async Task<string> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Creating payment for Order {OrderId} with provider {Provider}", request.OrderId, request.Provider);

            var order = await unitOfWork.Orders.GetByIdAsync(request.OrderId);
            if (order == null)
                throw new Exception("Order not found");

            // Kiểm tra xem order đã có payment pending hoặc success chưa
            var existingTransactions = await unitOfWork.Transactions.GetByOrderIdAsync(request.OrderId, cancellationToken);
            var pendingOrSuccessTransaction = existingTransactions.FirstOrDefault(t =>  t.Status == TransactionStatus.Pending || t.Status == TransactionStatus.Success);

            if (pendingOrSuccessTransaction != null)
            {
                // ✅ Check if PayUrl is still valid (MoMo URLs expire after ~30-60 minutes)
                if (!string.IsNullOrEmpty(pendingOrSuccessTransaction.PayUrl))
                {
                    var ageInMinutes = (DateTime.UtcNow - pendingOrSuccessTransaction.TransactionDate).TotalMinutes;
                    const int payUrlValidityMinutes = 25;  // Reuse if younger than 25 minutes

                    if (ageInMinutes < payUrlValidityMinutes)
                    {
                        logger.LogWarning("Order {OrderId} has valid pending PayUrl (age: {AgeMinutes:F1} minutes, Status: {Status}). " +
                            "Returning existing PayUrl to prevent MoMo duplicate request.", 
                            request.OrderId, ageInMinutes, pendingOrSuccessTransaction.Status);

                        return pendingOrSuccessTransaction.PayUrl;
                    }
                    else
                    {
                        logger.LogWarning("Order {OrderId} has EXPIRED PayUrl (age: {AgeMinutes:F1} minutes, max: {MaxMinutes} minutes). " +
                            "Creating new payment to prevent MoMo 'transaction expired' error.", 
                            request.OrderId, ageInMinutes, payUrlValidityMinutes);

                        // ✅ Mark old transaction as expired/failed so we can create a new one
                        pendingOrSuccessTransaction.Status = TransactionStatus.Failed;
                        pendingOrSuccessTransaction.Message = "Payment URL expired, new payment created";
                        await unitOfWork.SaveChangesAsync(cancellationToken);

                        // Continue to create new payment below
                    }
                }

                // Nếu là success transaction, không cho phép tạo payment mới
                if (pendingOrSuccessTransaction.Status == TransactionStatus.Success)
                {
                    throw new Exception("Order has already been paid successfully. Cannot create new payment.");
                }
            }

            var paymentService = paymentServices.FirstOrDefault(s => 
                s.ProviderName.Equals(request.Provider, StringComparison.OrdinalIgnoreCase));

            if (paymentService == null)
                throw new Exception($"Payment provider '{request.Provider}' is not supported");

            var payUrl = await paymentService.CreatePaymentAsync(order);
            logger.LogInformation("Payment URL created successfully for Order {OrderId}", request.OrderId);

            return payUrl;
        }
    }
}
