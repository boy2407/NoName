using MediatR;
using NoName.Application.Abstractions;
using NoName.Application.Common;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, ApiResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderStatusCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResult<bool>> Handle(UpdateOrderStatusCommand request, CancellationToken ct)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(request.Id, ct);
            if (order == null)
            {
                return ApiResult<bool>.Failure("Order not found.");
            }

            order.Status = request.Status;

            await _unitOfWork.Orders.UpdateAsync(order, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResult<bool>.Success(true, "Order status updated.");
        }
    }
}
