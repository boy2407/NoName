using MediatR;

namespace NoName.Application.Features.Payments.Commands.CreatePayment
{
    public record CreatePaymentCommand(int OrderId, string Provider) : IRequest<string>;
}
