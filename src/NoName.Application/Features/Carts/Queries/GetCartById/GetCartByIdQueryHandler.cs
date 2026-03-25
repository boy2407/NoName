using MediatR;
using NoName.Application.Abstractions;
using NoName.Application.Common;
using NoName.Application.Features.Carts.DTOs;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Carts.Queries.GetCartById
{
    public class GetCartByIdQueryHandler : IRequestHandler<GetCartByIdQuery, ApiResult<CartItemDto?>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCartByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResult<CartItemDto?>> Handle(GetCartByIdQuery request, CancellationToken ct)
        {
            var item = await _unitOfWork.Carts.GetByIdAsync(request.Id, ct);
            if (item == null)
            {
                return ApiResult<CartItemDto?>.Failure("Cart item not found.");
            }

            if (item.UserId != request.UserId)
            {
                return ApiResult<CartItemDto?>.Failure("You do not have permission to view this cart item.");
            }

            var result = new CartItemDto
            {
                Id = item.Id,
                ProductVariantId = item.ProductVariantId,
                Quantity = item.Quantity,
                Price = item.Price,
                DateCreated = item.DateCreated
            };

            return ApiResult<CartItemDto?>.Success(result);
        }
    }
}
