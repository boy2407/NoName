using MediatR;
using NoName.Application.Common;
using NoName.Shared.DTOs.Orders;
using System;
using System.Text.Json.Serialization;

namespace NoName.Application.Features.Orders.Queries.GetOrderById
{
    public class GetOrderByIdQuery : IRequest<ApiResult<OrderDto?>>
    {
        public int Id { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public bool IsManagement { get; set; }
    }
}
