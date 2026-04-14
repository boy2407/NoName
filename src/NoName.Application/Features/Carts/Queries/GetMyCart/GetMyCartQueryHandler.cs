using MediatR;
using NoName.Application.Abstractions;
using NoName.Application.Common;
using NoName.Shared.DTOs.Carts;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Carts.Queries.GetMyCart
{
    public class GetMyCartQueryHandler : IRequestHandler<GetMyCartQuery, ApiResult<List<CartItemDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMyCartQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResult<List<CartItemDto>>> Handle(GetMyCartQuery request, CancellationToken ct)
        {
            var items = await _unitOfWork.Carts.GetByUserIdAsync(request.UserId, ct);

            var result = items.Select(x => new CartItemDto
            {
                Id = x.Id,
                ProductVariantId = x.ProductVariantId,
                Quantity = x.Quantity,
                Price = x.Price,
                DateCreated = x.DateCreated
            }).ToList();

            return ApiResult<List<CartItemDto>>.Success(result);
        }
    }
}
