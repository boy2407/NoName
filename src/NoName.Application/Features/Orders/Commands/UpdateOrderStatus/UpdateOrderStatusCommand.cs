using MediatR;
using NoName.Application.Common;
using NoName.Domain.Enums;
using System.Text.Json.Serialization;

namespace NoName.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommand : IRequest<ApiResult<bool>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public OrderStatus Status { get; set; }
    }
}
