using MediatR;
using NoName.Application.Common;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NoName.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderItemRequest
    {
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateOrderCommand : IRequest<ApiResult<int>>
    {
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipEmail { get; set; }
        public string ShipPhoneNumber { get; set; }
        public List<CreateOrderItemRequest> Items { get; set; } = new();

        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}
