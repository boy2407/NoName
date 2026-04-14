using MediatR;
using NoName.Application.Common;
using NoName.Shared.DTOs.Carts;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NoName.Application.Features.Carts.Queries.GetMyCart
{
    public class GetMyCartQuery : IRequest<ApiResult<List<CartItemDto>>>
    {
        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}
