using MediatR;
using NoName.Application.Common;
using System;
using System.Text.Json.Serialization;

namespace NoName.Application.Features.Carts.Commands.CreateCart
{
    public class CreateCartCommand : IRequest<ApiResult<int>>
    {
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}
