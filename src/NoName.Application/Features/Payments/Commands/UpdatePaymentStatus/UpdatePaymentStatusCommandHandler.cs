using MediatR;
using Microsoft.Extensions.Logging;
using NoName.Application.Abstractions;
using NoName.Application.Abstractions.Services;
using NoName.Domain.Enums;

namespace NoName.Application.Features.Payments.Commands.UpdatePaymentStatus
{
    public class UpdatePaymentStatusCommandHandler(
        IEnumerable<IPaymentService> paymentServices,
        IUnitOfWork unitOfWork,
        IDistributedLockService lockService,
        IPaymentStatusNotifier paymentStatusNotifier,
        ILogger<UpdatePaymentStatusCommandHandler> logger)
        : IRequestHandler<UpdatePaymentStatusCommand, UpdatePaymentStatusResult>
    {
        public async Task<UpdatePaymentStatusResult> Handle(UpdatePaymentStatusCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Processing payment IPN from {Provider}", request.Provider);

            var paymentService = paymentServices.FirstOrDefault(s => s.ProviderName.Equals(request.Provider, StringComparison.OrdinalIgnoreCase));

            if (paymentService == null)
            {
                logger.LogWarning("Payment provider {Provider} not found", request.Provider);
                return new UpdatePaymentStatusResult(false, false, $"Payment provider {request.Provider} not found");
            }

            var isValid = await paymentService.ValidateCallback(request.CallbackData);
            if (!isValid)
            {
                logger.LogWarning("Callback validation failed for {Provider}", request.Provider);
                return new UpdatePaymentStatusResult(false, false, "Callback validation failed");
            }

            // Extract order ID from callback data
            if (!request.CallbackData.TryGetValue("orderId", out var orderIdStr) || !int.TryParse(orderIdStr, out var orderId))
            {
                logger.LogError("Order ID not found or invalid in callback data");
                return new UpdatePaymentStatusResult(false, false, "Order ID not found or invalid in callback data");
            }

            var lockKey = new List<string> { $"lock:payment:order:{orderId}" };
            IDisposable? paymentLock = null;

            try
            {
                paymentLock = await lockService.AcquireLockAsync(
                    lockKey,
                    TimeSpan.FromSeconds(15),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromMilliseconds(50));
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Could not acquire payment lock for Order {OrderId}", orderId);
                return new UpdatePaymentStatusResult(false, false, "Payment is being processed, please retry");
            }

            using (paymentLock)
            {
                var order = await unitOfWork.Orders.GetByIdAsync(orderId);
                if (order == null)
                {
                    logger.LogError("Order {OrderId} not found", orderId);
                    return new UpdatePaymentStatusResult(false, false, $"Order {orderId} not found");
                }

                // Check if payment was successful (resultCode = 0 for MoMo)
                if (!request.CallbackData.TryGetValue("resultCode", out var resultCode))
                {
                    logger.LogWarning("resultCode is missing in callback data for Order {OrderId}", orderId);
                    return new UpdatePaymentStatusResult(false, false, "resultCode is missing in callback data");
                }

                var incomingTransactionId = request.CallbackData.TryGetValue("transactionId", out var txId) ? txId : null;
                var isSuccess = resultCode == "0";
                var isDuplicateOrderCode = resultCode == "1001";

                // Update transaction status
                var transactions = await unitOfWork.Transactions.GetByOrderIdAsync(orderId, cancellationToken);
                var transaction = transactions
                    .OrderByDescending(x => x.TransactionDate)
                    .FirstOrDefault();

                var isFinalSuccess = order.Status == OrderStatus.Confirmed
                    || order.Status == OrderStatus.Success
                    || transaction?.Status == TransactionStatus.Success;

                if (isDuplicateOrderCode)
                {
                    logger.LogInformation("Received duplicate payment notification (1001) for Order {OrderId}. Ignore DB update.", orderId);
                    await paymentStatusNotifier.NotifyAsync(orderId, isFinalSuccess, "Duplicate payment notification (1001) ignored", cancellationToken);
                    return new UpdatePaymentStatusResult(true, isFinalSuccess, "Duplicate payment notification (1001) ignored");
                }

                if (isFinalSuccess)
                {
                    if (isSuccess)
                    {
                        var isSameTransaction = !string.IsNullOrWhiteSpace(incomingTransactionId)
                            && string.Equals(transaction?.ExternalTransactionId, incomingTransactionId, StringComparison.OrdinalIgnoreCase);

                        if (isSameTransaction)
                        {
                            logger.LogInformation("Duplicate success callback/IPN for Order {OrderId} with same transactionId {TransactionId}. Ignore DB update.", orderId, incomingTransactionId);
                            await paymentStatusNotifier.NotifyAsync(orderId, true, "Duplicate success notification ignored", cancellationToken);
                            return new UpdatePaymentStatusResult(true, true, "Duplicate success notification ignored");
                        }

                        logger.LogWarning("Order {OrderId} already marked success. Incoming success transactionId {IncomingTransactionId} is different from current {CurrentTransactionId}. Ignore DB update.", orderId, incomingTransactionId, transaction?.ExternalTransactionId);
                        await paymentStatusNotifier.NotifyAsync(orderId, true, "Order already paid, duplicate success ignored", cancellationToken);
                        return new UpdatePaymentStatusResult(true, true, "Order already paid, duplicate success ignored");
                    }

                    logger.LogWarning("Ignoring status downgrade for Order {OrderId} because payment already finalized as success", orderId);
                    await paymentStatusNotifier.NotifyAsync(orderId, true, "Order already paid, downgrade ignored", cancellationToken);
                    return new UpdatePaymentStatusResult(true, true, "Order already paid, downgrade ignored");
                }

                if (transaction != null)
                {
                    transaction.Status = isSuccess ? TransactionStatus.Success : TransactionStatus.Failed;
                    transaction.ExternalTransactionId = incomingTransactionId;
                    transaction.Message = request.CallbackData.TryGetValue("message", out var msg) ? msg : null;
                    transaction.Result = resultCode;

                    await unitOfWork.Transactions.UpdateAsync(transaction, cancellationToken);

                    logger.LogInformation("Transaction {TransactionId} status updated to {Status}", transaction.Id, transaction.Status);
                }
                else
                {
                    logger.LogWarning("No transaction found for Order {OrderId}", orderId);
                }

                // Update order status
                if (isSuccess)
                {
                    order.Status = OrderStatus.Confirmed;
                    logger.LogInformation("Order {OrderId} status updated to Confirmed", orderId);
                }
                else
                {
                    order.Status = OrderStatus.Canceled;
                    logger.LogInformation("Order {OrderId} status updated to Canceled", orderId);
                }

                await unitOfWork.SaveChangesAsync(cancellationToken);
                logger.LogInformation("Payment IPN processed successfully for Order {OrderId}", orderId);
                await paymentStatusNotifier.NotifyAsync(orderId, isSuccess, "Payment status updated by IPN", cancellationToken);

                return new UpdatePaymentStatusResult(true, isSuccess);
            }
        }
    }
}
