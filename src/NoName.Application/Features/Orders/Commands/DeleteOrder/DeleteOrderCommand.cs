using MediatR;
using NoName.Application.Common;

namespace NoName.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommand : IRequest<ApiResult<bool>>
    {
        public int Id { get; set; }
    }
}
