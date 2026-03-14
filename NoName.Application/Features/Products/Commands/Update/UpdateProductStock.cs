using MediatR;

namespace NoName.Application.Features.Product.Commands.Update
{
    public class UpdateProductStock : IRequest<int>
    {
        public int Id { get; set; }
        public int Stock { get; set; }
    }
}
