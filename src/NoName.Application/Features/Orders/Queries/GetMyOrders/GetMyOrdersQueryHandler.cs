using MediatR;
using NoName.Application.Abstractions;
using NoName.Application.Common;
using NoName.Application.Features.Orders.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Orders.Queries.GetMyOrders
{
    public class GetMyOrdersQueryHandler : IRequestHandler<GetMyOrdersQuery, ApiResult<List<OrderDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetMyOrdersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResult<List<OrderDto>>> Handle(GetMyOrdersQuery request, CancellationToken ct)
        {
            var orders = await _unitOfWork.Orders.GetByUserIdAsync(request.UserId, ct);

            var result = orders.Select(order => new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                ShipName = order.ShipName,
                ShipAddress = order.ShipAddress,
                ShipEmail = order.ShipEmail,
                ShipPhoneNumber = order.ShipPhoneNumber,
                Status = order.Status,
                Details = order.OrderDetails.Select(d => new OrderDetailDto
                {
                    ProductVariantId = d.ProductVariantId,
                    Quantity = d.Quantity,
                    Price = d.Price
                }).ToList()
            }).ToList();

            return ApiResult<List<OrderDto>>.Success(result);
        }
    }
}
