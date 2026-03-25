using MediatR;
using NoName.Application.Common;

namespace NoName.Application.Features.Products.Commands.Delete
{
    public class DeleteProductCommand : IRequest<ApiResult<bool>>
    {
        public int Id { get; set; }
    }
}
