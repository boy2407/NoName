using MediatR;
using NoName.Application.Abstractions;
using NoName.Application.Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Carts.Commands.UpdateCart
{
    public class UpdateCartCommandHandler : IRequestHandler<UpdateCartCommand, ApiResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCartCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResult<bool>> Handle(UpdateCartCommand request, CancellationToken ct)
        {
            var cart = await _unitOfWork.Carts.GetByIdAsync(request.Id, ct);
            if (cart == null)
            {
                return ApiResult<bool>.Failure("Cart item not found.");
            }

            if (cart.UserId != request.UserId)
            {
                return ApiResult<bool>.Failure("You do not have permission to update this cart item.");
            }

            var variant = await _unitOfWork.ProductVariants.GetByIdAsync(cart.ProductVariantId, ct);
            if (variant == null)
            {
                return ApiResult<bool>.Failure("Product variant does not exist.");
            }

            var availableQuantity = Math.Max(variant.Inventory?.AvailableQuantity ?? 0, 0);

            if (request.Quantity > availableQuantity)
            {
                return ApiResult<bool>.Failure($"Insufficient stock. Available quantity: {availableQuantity}.");
            }

            cart.Quantity = request.Quantity;
            cart.Price = variant.Price;

            await _unitOfWork.Carts.UpdateAsync(cart, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResult<bool>.Success(true, "Cart item updated.");
        }
    }
}
