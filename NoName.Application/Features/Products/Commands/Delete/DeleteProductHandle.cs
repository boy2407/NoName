using MediatR;
using NoName.Application.Abstractions;
using NoName.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Commands.Delete
{
    public class DeleteProductHandle :IRequestHandler<DeleteProduct, ApiResult<bool>>
    {
        IUnitOfWork _unitOfWork;
        public DeleteProductHandle (IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiResult<bool>> Handle(DeleteProduct command, CancellationToken ct)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(command.Id,ct);
            if (product == null) return ApiResult<bool>.Failure("No product found");

            product.IsActive = false;
            product.DateModified = DateTime.Now;

            await _unitOfWork.SaveChangesAsync(ct);
            return ApiResult<bool>.Success(true, "Product Deleted Successfully");
        }
    }
}
