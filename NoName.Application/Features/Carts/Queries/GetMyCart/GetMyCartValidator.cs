using FluentValidation;

namespace NoName.Application.Features.Carts.Queries.GetMyCart
{
    public class GetMyCartValidator : AbstractValidator<GetMyCartQuery>
    {
        public GetMyCartValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required.");
        }
    }
}
