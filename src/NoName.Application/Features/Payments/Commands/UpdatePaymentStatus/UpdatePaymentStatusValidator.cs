using FluentValidation;

namespace NoName.Application.Features.Payments.Commands.UpdatePaymentStatus
{
    public class UpdatePaymentStatusValidator : AbstractValidator<UpdatePaymentStatusCommand>
    {
        public UpdatePaymentStatusValidator()
        {
            RuleFor(x => x.CallbackData)
                .NotNull()
                .WithMessage("Callback data is required");

            RuleFor(x => x.Provider)
                .NotEmpty()
                .WithMessage("Payment provider is required");
        }
    }
}
