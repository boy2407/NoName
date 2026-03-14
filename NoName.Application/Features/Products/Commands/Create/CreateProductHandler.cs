    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using NoName.Application.Abstractions.Persistence;
    using NoName.Application.Abstractions.Services;
    using NoName.Domain.Entities;
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
namespace NoName.Application.Features.Products.Commands.Create
{
    public class CreateProductHandler : IRequestHandler<CreateProduct, bool>, IRequestHandler<AddProductImage, bool>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediaService _mediaService;
        private readonly IMapper _mapper;

        public CreateProductHandler(IProductRepository productRepository, IMediaService mediaService, IMapper mapper)
        {
            _productRepository = productRepository;
            _mediaService = mediaService;
            _mapper = mapper;
        }
        public async Task<bool> Handle(CreateProduct request, CancellationToken ct)
        {
            //auto mapping
            var product = _mapper.Map<NoName.Domain.Entities.Product>(request);
            product.DateCreated = DateTime.Now;
            product.DateModified = DateTime.Now;
            product.ViewCount = 0;
            //Save Database
            await _productRepository.AddAsync(product, ct);
            await _productRepository.SaveChangesAsync(ct);

            return true;
        }
        public async Task<bool> Handle(AddProductImage request, CancellationToken cancellationToken)
        {

            var product = await _productRepository.GetProductWithImagesAsync(request.ProductId, cancellationToken);
            if (product == null) return false;
            var caption = product.ProductTranslations
                                 .FirstOrDefault(x => string.Equals(x.LanguageId, "vi-VN", StringComparison.OrdinalIgnoreCase))
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

            await _productRepository.SaveChangesAsync(cancellationToken);
            return true;
        }

    }
}







