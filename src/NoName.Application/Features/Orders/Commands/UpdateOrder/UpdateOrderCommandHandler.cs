using MediatR;
using NoName.Application.Abstractions;
using NoName.Application.Common;
using NoName.Domain.Enums;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ApiResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResult<bool>> Handle(UpdateOrderCommand request, CancellationToken ct)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(request.Id, ct);
            if (order == null)
            {
                return ApiResult<bool>.Failure("Order not found.");
            }

            if (order.UserId != request.UserId)
            {
                return ApiResult<bool>.Failure("You do not have permission to update this order.");
            }

            if (order.Status != OrderStatus.InProgress)
            {
                return ApiResult<bool>.Failure("Only in-progress orders can be updated.");
            }

            order.ShipName = request.ShipName;
            order.ShipAddress = request.ShipAddress;
            order.ShipEmail = request.ShipEmail;
            order.ShipPhoneNumber = request.ShipPhoneNumber;

            await _unitOfWork.Orders.UpdateAsync(order, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResult<bool>.Success(true, "Order updated.");
        }
    }
}
