using FluentValidation;

namespace NoName.Application.Features.Carts.Commands.DeleteCart
{
    public class DeleteCartValidator : AbstractValidator<DeleteCartCommand>
    {
        public DeleteCartValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}
