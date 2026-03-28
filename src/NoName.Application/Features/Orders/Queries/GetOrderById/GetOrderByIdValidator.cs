using FluentValidation;

namespace NoName.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdValidator : AbstractValidator<GetOrderByIdQuery>
    {
        public GetOrderByIdValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}
