using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NoName.Application.Abstractions;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Abstractions.Services;
using NoName.Domain.Entities;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace NoName.Application.Features.Products.Commands.Create
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>, IRequestHandler<AddProductImageCommand, bool>, IRequestHandler<AddProductVariantCommand, bool>
    {
        private readonly IUnitOfWork _unitOfWork; // Quản lý chung các Repositories
        private readonly IMediaService _mediaService;
        private readonly IMapper _mapper;

        public CreateProductCommandHandler(IUnitOfWork unitOfWork, IMediaService mediaService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mediaService = mediaService;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateProductCommand request, CancellationToken ct)
        {
            var product = _mapper.Map<NoName.Domain.Entities.Product>(request);
            product.DateCreated = DateTime.Now;
            product.ViewCount = 0;
            await _unitOfWork.Products.AddAsync(product, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return product.Id;
        }


        // ADD VARIANT & SKU
        public async Task<bool> Handle(AddProductVariantCommand request, CancellationToken ct)
        {

            var product = await _unitOfWork.Products.GetByIdAsync(request.ProductId, ct);
            if (product == null) return false;


            product.AddVariant(request.SKU, request.Price, request.OriginalPrice, request.OptionValueIds);

            var newVariant = product.ProductVariants.LastOrDefault(x => x.SKU == request.SKU);

            if (newVariant != null)
            {
                if (newVariant.Inventory == null)
                {
                    newVariant.Inventory = new Inventory();
                }

                newVariant.Inventory.PhysicalQuantity = request.Stock;
                newVariant.Inventory.ReservedQuantity = 0;
                newVariant.Inventory.LastUpdated = DateTime.Now;
            }

            return await _unitOfWork.SaveChangesAsync(ct) > 0;
        }

        public async Task<bool> Handle(AddProductImageCommand request, CancellationToken ct)
        {
            var product = await _unitOfWork.Products.GetProductWithImagesAsync(request.ProductId, ct);
            if (product == null) return false;

            var caption = product.ProductTranslations
                                 .FirstOrDefault(x => x.LanguageId.Equals("vi-VN", StringComparison.OrdinalIgnoreCase))
                                 ?.Name ?? "Product Image";

            if (request.ThumbnailImage != null)
            {
                var path = await _mediaService.UploadFileAsync(request.ThumbnailImage, "products");
                product.AddImage(path, request.ThumbnailImage.Length, isDefault: true, caption: caption);
            }

            if (request.GalleryImages != null && request.GalleryImages.Any())
            {
                foreach (var file in request.GalleryImages)
                {
                    var path = await _mediaService.UploadFileAsync(file, "products");
                    product.AddImage(path, file.Length, isDefault: false, caption: caption);
                }
            }

            return await _unitOfWork.SaveChangesAsync(ct) > 0;
        }

    }
}







