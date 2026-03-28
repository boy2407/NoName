using MediatR;
using NoName.Application.Common;
using NoName.Shared.DTOs.Carts;
using System;
using System.Text.Json.Serialization;

namespace NoName.Application.Features.Carts.Queries.GetCartById
{
    public class GetCartByIdQuery : IRequest<ApiResult<CartItemDto?>>
    {
        public int Id { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}
