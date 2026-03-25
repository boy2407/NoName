using MediatR;
using NoName.Application.Abstractions;
using NoName.Application.Common;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, ApiResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOrderCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResult<bool>> Handle(DeleteOrderCommand request, CancellationToken ct)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(request.Id, ct);
            if (order == null)
            {
                return ApiResult<bool>.Failure("Order not found.");
            }

            await _unitOfWork.Orders.DeleteAsync(order, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResult<bool>.Success(true, "Order deleted.");
        }
    }
}
