using MediatR;

namespace NoName.Application.Features.Payments.Commands.UpdatePaymentStatus
{
    public record UpdatePaymentStatusResult(
        bool Processed,
        bool IsSuccess,
        string? Message = null);

    public record UpdatePaymentStatusCommand(IDictionary<string, string> CallbackData,string Provider) : IRequest<UpdatePaymentStatusResult>;
}
