using MediatR;
using NoName.Application.Abstractions;
using NoName.Application.Abstractions.Services;
using NoName.Application.Common;
using NoName.Application.Features.Products.Commands.Update.common;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Commands.Update.Variants
{
    public class UpdateProductVariantHandle : IRequestHandler<UpdateProductVariant, ApiResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductVariantHandle(IUnitOfWork unitOfWork, ILanguageService languageService)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResult<bool>> Handle(UpdateProductVariant request, CancellationToken ct)
        {
            try
            {
                var product = await _unitOfWork.Products.ExistsAsync(request.ProductId, ct);
                if (!product) return ApiResult<bool>.Failure("Not Product Found ");


                var currentVariants = await _unitOfWork.ProductVariants.GetByProductIdAsync(request.ProductId, ct);
                currentVariants ??= new List<ProductVariant>();

                var requestIds = request.Variants.Where(v => v.Id.HasValue).Select(v => v.Id!.Value).ToList();

                var variantsToRemove = currentVariants.Where(v => !requestIds.Contains(v.Id)).ToList();
                foreach (var v in variantsToRemove) _unitOfWork.ProductVariants.Remove(v);


                foreach (var vDto in request.Variants)
                {
                    // TÌM: Dựa vào Id gửi lên để bốc đối tượng cũ từ DB ra
                    var existing = currentVariants.FirstOrDefault(x => x.Id == vDto.Id);

                    if (existing != null)
                    {
                        // UPDATE
                        existing.SKU = vDto.SKU;
                        existing.Price = vDto.Price;
                        existing.OriginalPrice = vDto.OriginalPrice;

                        if (existing.Inventory != null)
                        {
                            existing.Inventory.PhysicalQuantity = vDto.PhysicalStock;
                            existing.Inventory.LastUpdated = DateTime.Now;
                        }
                    }
                    else
                    {
                        // ADD
                        _unitOfWork.ProductVariants.Add(new ProductVariant
                        {
                            ProductId = request.ProductId,
                            SKU = vDto.SKU,
                            Price = vDto.Price,
                            OriginalPrice = vDto.OriginalPrice,
                            Inventory = new Inventory
                            {
                                PhysicalQuantity = vDto.PhysicalStock,
                                LastUpdated = DateTime.Now
                            }
                        });
                    }
                }


                await _unitOfWork.SaveChangesAsync(ct);
                return ApiResult<bool>.Success(true, "Product variant updated successfully.");
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException?.Message ?? ex.Message;
                if (msg.Contains("Unique") || msg.Contains("Duplicate"))// SKU exist on other product. Plese choose another SKU.
                {
                    return ApiResult<bool>.Failure("SKU existed on other product. Plese choose another SKU.");
                }

                return ApiResult<bool>.Failure("Data error. Please try again later.: " + msg);// Data error. Please try again later.
            }
        }
    }
}