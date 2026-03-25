using MediatR;
using NoName.Application.Common;
using NoName.Application.Features.Orders.DTOs;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NoName.Application.Features.Orders.Queries.GetMyOrders
{
    public class GetMyOrdersQuery : IRequest<ApiResult<List<OrderDto>>>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}
