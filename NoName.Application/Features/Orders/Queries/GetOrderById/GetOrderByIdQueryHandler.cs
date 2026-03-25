using MediatR;
using NoName.Application.Abstractions;
using NoName.Application.Common;
using NoName.Application.Features.Orders.DTOs;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, ApiResult<OrderDto?>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetOrderByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResult<OrderDto?>> Handle(GetOrderByIdQuery request, CancellationToken ct)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(request.Id, ct);
            if (order == null)
            {
                return ApiResult<OrderDto?>.Failure("Order not found.");
            }

            if (!request.IsManagement && order.UserId != request.UserId)
            {
                return ApiResult<OrderDto?>.Failure("You do not have permission to view this order.");
            }

            var result = new OrderDto
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
            };

            return ApiResult<OrderDto?>.Success(result);
        }
    }
}
