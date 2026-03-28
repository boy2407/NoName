using FluentValidation;

namespace NoName.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusValidator : AbstractValidator<UpdateOrderStatusCommand>
    {
        public UpdateOrderStatusValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Status).IsInEnum();
        }
    }
}
