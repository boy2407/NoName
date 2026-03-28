using MediatR;
using NoName.Application.Common;

namespace NoName.Application.Features.Products.Commands.Options
{
    public class DeleteProductOptionCommand : IRequest<ApiResult<bool>>
    {
        public int ProductId { get; set; }
        public int OptionId { get; set; }
    }
}
