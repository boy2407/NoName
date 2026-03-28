using MediatR;
using NoName.Application.Abstractions;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Common;
using NoName.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Commands.Options
{
    public class DeleteProductOptionHandler : IRequestHandler<DeleteProductOptionCommand, ApiResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductOptionHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResult<bool>> Handle(DeleteProductOptionCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetProductForUpdateAsync(request.ProductId, cancellationToken);
            if (product == null) throw new NotFoundException(nameof(Product), request.ProductId);

            var option = product.Options.FirstOrDefault(o => o.Id == request.OptionId);
            if (option == null) throw new NotFoundException(nameof(ProductOption), request.OptionId);

            product.Options.Remove(option);

            await _unitOfWork.Products.UpdateAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResult<bool>.Success(true);
        }
    }
}
