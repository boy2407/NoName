using MediatR;
using NoName.Application.Common;
using NoName.Application.Abstractions;
using NoName.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Commands.Delete
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, ApiResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResult<bool>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(request.Id, cancellationToken);
            if (product == null) return ApiResult<bool>.Failure("Product not found.");

            await _unitOfWork.Products.DeleteAsync(product, cancellationToken);
            var changed = await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (changed > 0) return ApiResult<bool>.Success(true, "Product deleted successfully.");
            return ApiResult<bool>.Failure("Failed to delete product.");
        }
    }
}
