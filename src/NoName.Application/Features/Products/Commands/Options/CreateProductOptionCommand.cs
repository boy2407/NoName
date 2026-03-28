using MediatR;
using System.Collections.Generic;

namespace NoName.Application.Features.Products.Commands.Options
{
    public class CreateProductOptionCommand : IRequest<int>
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public List<string> Values { get; set; } = new();
    }
}
