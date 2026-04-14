using MediatR;
using Microsoft.Extensions.Logging;
using NoName.Application.Abstractions;
using NoName.Application.Abstractions.Services;

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
