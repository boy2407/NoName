using MediatR;
using System.Collections.Generic;

namespace NoName.Application.Features.Products.Commands.Options
{
    public class UpdateProductOptionCommand : IRequest<NoName.Application.Common.ApiResult<bool>>
    {
        public int ProductId { get; set; }
        public int OptionId { get; set; }
        public string Name { get; set; }
        public List<string> Values { get; set; } = new();
    }
}
