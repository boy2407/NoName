using FluentValidation;

namespace NoName.Application.Features.Carts.Commands.CreateCart
{
    public class CreateCartValidator : AbstractValidator<CreateCartCommand>
    {
        public CreateCartValidator()
        {
            RuleFor(x => x.ProductVariantId)
                .GreaterThan(0).WithMessage("ProductVariantId must be greater than 0.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
        }
    }
}
