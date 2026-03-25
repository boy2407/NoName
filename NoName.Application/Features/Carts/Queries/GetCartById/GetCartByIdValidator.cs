using FluentValidation;

namespace NoName.Application.Features.Carts.Queries.GetCartById
{
    public class GetCartByIdValidator : AbstractValidator<GetCartByIdQuery>
    {
        public GetCartByIdValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");
        }
    }
}
