using MediatR;

namespace NoName.Application.Features.Product.Commands.Update
{
    public class UpdateProductPrice : IRequest<int>
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
    }
}
