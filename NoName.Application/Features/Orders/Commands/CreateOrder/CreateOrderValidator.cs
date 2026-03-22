using FluentValidation;

namespace NoName.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderValidator()
        {
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

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one order item is required.");

            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(i => i.ProductVariantId).GreaterThan(0);
                item.RuleFor(i => i.Quantity).GreaterThan(0);
            });
        }
    }
}
