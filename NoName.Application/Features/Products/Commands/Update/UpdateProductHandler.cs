using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Abstractions.Services;
using NoName.Application.Common;
using NoName.Domain.Entities;

namespace NoName.Application.Features.Product.Commands.Update
{
    public class UpdateProductHandler : IRequestHandler<UpdateProduct, int>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMediaService _mediaService;
        public UpdateProductHandler(IProductRepository productRepository, IMediaService mediaService)
        {
            _productRepository = productRepository;
            _mediaService = mediaService;
        }

        public async Task<int> Handle(UpdateProduct request, CancellationToken ct)
        {
            var product = await _productRepository.GetByIdAsync(request.Id, ct);
            if (product == null)
            {
                throw new NotFoundException($"cannot find product with id : {request.Id}");
            }

            // Update main fields
            product.Price = request.Price;
            product.OriginalPrice = request.OriginalPrice;
            product.Stock = request.Stock;
            product.IsActive = request.IsActive;
            product.DateModified = DateTime.Now;

            // Update or add translation for specified language
            product.ProductTranslations = product.ProductTranslations ?? new System.Collections.Generic.List<ProductTranslation>();
            var translation = product.ProductTranslations.FirstOrDefault(t => t.LanguageId == request.LanguageId);
            if (translation == null)
            {
                translation = new ProductTranslation
                {
                    LanguageId = request.LanguageId,
                    Name = request.Name,
                    Description = request.Description,
                    Details = request.Details,
                    SeoAlias = request.SeoAlias
                };
                product.ProductTranslations.Add(translation);
            }
            else
            {
                translation.Name = request.Name;
                translation.Description = request.Description;
                translation.Details = request.Details;
                translation.SeoAlias = request.SeoAlias;
            }

            // Update categories
            product.ProductInCategories = product.ProductInCategories ?? new System.Collections.Generic.List<ProductInCategory>();
            // Remove categories not in request
            var toRemove = product.ProductInCategories.Where(pic => !request.CategoryIds.Contains(pic.CategoryId)).ToList();
            foreach (var rem in toRemove)
            {
                product.ProductInCategories.Remove(rem);
            }
            // Add new categories
            var existingIds = product.ProductInCategories.Select(pic => pic.CategoryId).ToList();
            var toAdd = request.CategoryIds.Where(id => !existingIds.Contains(id)).Select(id => new ProductInCategory { CategoryId = id }).ToList();
            foreach (var add in toAdd)
            {
                product.ProductInCategories.Add(add);
            }



            if (request.NewThumbnailImage != null)
            {
                // find old default image
                var oldImage = product.ProductImages.FirstOrDefault(x => x.IsDefault);

                if (oldImage != null)
                {
                    //delete old file on storage
                    await _mediaService.DeleteFileAsync(oldImage.ImagePath);

                    // save new file to storage
                    string newPath = await _mediaService.UploadFileAsync(request.NewThumbnailImage, "products");

                    //update new path and file size to db
                    oldImage.ImagePath = newPath;
                    oldImage.FileSize = request.NewThumbnailImage.Length;
                }
            }

            // handle new gallery images
            if (request.NewGalleryImages != null && request.NewGalleryImages.Any())
            {
                foreach (var file in request.NewGalleryImages)
                {
                    // Lưu từng file vật lý và lấy đường dẫn
                    string galleryPath = await _mediaService.UploadFileAsync(file, "products");

                    // Add thêm một bản ghi mới vào danh sách ảnh của Product
                    product.AddImage(galleryPath, file.Length, false, $"Gallery image {file.FileName}");

                }
            }

            await _productRepository.SaveChangesAsync(ct);

            return product.Id;
        }
    }
}
