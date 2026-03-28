using FluentValidation;

namespace NoName.Application.Features.Orders.Queries.GetMyOrders
{
    public class GetMyOrdersValidator : AbstractValidator<GetMyOrdersQuery>
    {
        public GetMyOrdersValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
