using MediatR;
using NoName.Application.Common;
using System;
using System.Text.Json.Serialization;

namespace NoName.Application.Features.Carts.Commands.UpdateCart
{
    public class UpdateCartCommand : IRequest<ApiResult<bool>>
    {
        [JsonIgnore]
        public int Id { get; set; }
        public int Quantity { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}
