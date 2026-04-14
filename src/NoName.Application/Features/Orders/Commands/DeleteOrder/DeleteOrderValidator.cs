using FluentValidation;

namespace NoName.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderValidator : AbstractValidator<DeleteOrderCommand>
    {
        public DeleteOrderValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0);
        }
    }
}
