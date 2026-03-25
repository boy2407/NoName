using MediatR;
using NoName.Application.Abstractions;
using NoName.Application.Common;
using NoName.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Carts.Commands.CreateCart
{
    public class CreateCartCommandHandler : IRequestHandler<CreateCartCommand, ApiResult<int>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCartCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResult<int>> Handle(CreateCartCommand request, CancellationToken ct)
        {
            var variant = await _unitOfWork.ProductVariants.GetByIdAsync(request.ProductVariantId, ct);
            if (variant == null)
            {
                return ApiResult<int>.Failure("Product variant does not exist.");
            }

            var availableQuantity = Math.Max(variant.Inventory?.AvailableQuantity ?? 0, 0);

            var existingItem = await _unitOfWork.Carts.GetByUserAndVariantAsync(request.UserId, request.ProductVariantId, ct);
            var targetQuantity = request.Quantity + (existingItem?.Quantity ?? 0);

            if (targetQuantity > availableQuantity)
            {
                return ApiResult<int>.Failure($"Insufficient stock. Available quantity: {availableQuantity}.");
            }

            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
                existingItem.Price = variant.Price;
                await _unitOfWork.Carts.UpdateAsync(existingItem, ct);
                await _unitOfWork.SaveChangesAsync(ct);
                return ApiResult<int>.Success(existingItem.Id, "Cart item updated.");
            }

            var cart = new Cart
            {
                ProductVariantId = request.ProductVariantId,
                Quantity = request.Quantity,
                Price = variant.Price,
                UserId = request.UserId,
                DateCreated = DateTime.UtcNow
            };

            await _unitOfWork.Carts.AddAsync(cart, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return ApiResult<int>.Success(cart.Id, "Cart item created.");
        }
    }
}
