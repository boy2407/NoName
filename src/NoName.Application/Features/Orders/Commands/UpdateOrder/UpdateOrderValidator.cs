using FluentValidation;

namespace NoName.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderValidator : AbstractValidator<UpdateOrderCommand>
    {
        public UpdateOrderValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);

            RuleFor(x => x.ShipName)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.ShipAddress)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.ShipEmail)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(50);

            RuleFor(x => x.ShipPhoneNumber)
                .NotEmpty()
                .MaximumLength(20);
        }
    }
}
