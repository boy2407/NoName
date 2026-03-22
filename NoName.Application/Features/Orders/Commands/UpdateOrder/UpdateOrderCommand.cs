using MediatR;
using NoName.Application.Common;
using System;
using System.Text.Json.Serialization;

namespace NoName.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand : IRequest<ApiResult<bool>>
    {
        [JsonIgnore]
        public int Id { get; set; }

        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipEmail { get; set; }
        public string ShipPhoneNumber { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}
