using MediatR;
using NoName.Application.Common;
using System;
using System.Text.Json.Serialization;

namespace NoName.Application.Features.Carts.Commands.DeleteCart
{
    public class DeleteCartCommand : IRequest<ApiResult<bool>>
    {
        public int Id { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}
