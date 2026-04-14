using FluentValidation;

namespace NoName.Application.Features.Payments.Commands.CreatePayment
{
    public class CreatePaymentValidator : AbstractValidator<CreatePaymentCommand>
    {
        public CreatePaymentValidator()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0)
                .WithMessage("Order ID must be greater than 0");

            RuleFor(x => x.Provider)
                .NotEmpty()
                .WithMessage("Payment provider is required")
                .Must(x => x.Equals("MoMo", StringComparison.OrdinalIgnoreCase))
                .WithMessage("Payment provider must be 'MoMo'");
        }
    }
}
