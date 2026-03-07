using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Common;
using NoName.Domain.Entities;

namespace NoName.Application.Features.Product.Commands.Update
{
    public class UpdateProductHandler : IRequestHandler<UpdateProduct, int>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<int> Handle(UpdateProduct request, CancellationToken ct)
        {
            var product = await _productRepository.GetByIdWithDetailsAsync(request.Id, ct);
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

            await _productRepository.SaveChangesAsync(ct);

            return product.Id;
        }
    }
}
