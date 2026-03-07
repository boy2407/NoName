using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NoName.Application.Abstractions.Persistence;
using NoName.Domain.Entities;
using NoName.Application.Common;

namespace NoName.Application.Features.Product.Commands.Update
{
    public class UpdateProductPriceHandler : IRequestHandler<UpdateProductPrice, int>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductPriceHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<int> Handle(UpdateProductPrice request, CancellationToken ct)
        {
            var product = await _productRepository.GetByIdAsync(request.Id, ct);
            if (product == null)
            {
                throw new NotFoundException($"cannot find product with id : {request.Id}");
            }

            product.Price = request.Price;
            product.OriginalPrice = request.OriginalPrice;
            product.DateModified = DateTime.Now;

            await _productRepository.UpdateAsync(product, ct);
            await _productRepository.SaveChangesAsync(ct);

            return product.Id;
        }
    }
}
