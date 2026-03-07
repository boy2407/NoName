using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Common;

namespace NoName.Application.Features.Product.Commands.Update
{
    public class UpdateProductStockHandler : IRequestHandler<UpdateProductStock, int>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductStockHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<int> Handle(UpdateProductStock request, CancellationToken ct)
        {
            var product = await _productRepository.GetByIdAsync(request.Id, ct);
            if (product == null)
            {
                throw new NotFoundException($"cannot find product with id : {request.Id}");
            }

            product.Stock = request.Stock;
            product.DateModified = DateTime.Now;

            await _productRepository.UpdateAsync(product, ct);
            await _productRepository.SaveChangesAsync(ct);

            return product.Id;
        }
    }
}
