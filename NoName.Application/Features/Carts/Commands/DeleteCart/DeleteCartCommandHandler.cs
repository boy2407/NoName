using MediatR;
using NoName.Application.Abstractions;
using NoName.Application.Common;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Carts.Commands.DeleteCart
{
    public class DeleteCartCommandHandler : IRequestHandler<DeleteCartCommand, ApiResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCartCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResult<bool>> Handle(DeleteCartCommand request, CancellationToken ct)
        {
            var cart = await _unitOfWork.Carts.GetByIdAsync(request.Id, ct);
            if (cart == null)
            {
                return ApiResult<bool>.Failure("Cart item not found.");
            }

            if (cart.UserId != request.UserId)
            {
                return ApiResult<bool>.Failure("You do not have permission to delete this cart item.");
            }

            await _unitOfWork.Carts.DeleteAsync(cart, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResult<bool>.Success(true, "Cart item deleted.");
        }
    }
}
