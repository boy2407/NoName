using MediatR;
using NoName.Application.Abstractions;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Abstractions.Services;
using NoName.Application.Common;
using NoName.Application.Features.Products.Commands.Update.common;
using NoName.Application.Features.Products.Events;
using NoName.Domain.Entities;
using NoName.Shared.DTOs.Products.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Commands.Update.common
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ApiResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILanguageService _languageService;
        private readonly IMediator _mediator;
        public UpdateProductCommandHandler(IMediator mediator, IUnitOfWork unitOfWork, ILanguageService languageService)
        {
            _mediator = mediator;
            _unitOfWork = unitOfWork;
            _languageService = languageService;
        }

        public async Task<ApiResult<bool>> Handle(UpdateProductCommand request, CancellationToken ct)
        {

            var product = await _unitOfWork.Products.GetProductForUpdateAsync(request.Id, ct);
            if (product == null||!product.IsActive) return ApiResult<bool>.Failure("Not Product Found ");

            var distinctLangIds = request.Translations.Select(x => x.LanguageId).Distinct().ToList();
            var distinctCategoryIds = request.CategoryIds.Distinct().ToList();

            var langTask = await _unitOfWork.Languages.GetExistingIdsAsync(distinctLangIds, ct);
            var catTask = await _unitOfWork.Categories.GetExistingIdsAsync(distinctCategoryIds, ct);

            if (langTask.Count != distinctLangIds.Count)
                throw new Exception("One or more Languages are invalid.");

            if (catTask.Count != distinctCategoryIds.Count)
                throw new Exception("One or more categories are invalid..");

            product.IsActive = request.IsActive;
            UpdateProductCategories(product, request.CategoryIds);
            UpdateProductTranslations(product, request.Translations);
            var result = await _unitOfWork.SaveChangesAsync(ct);

            if (result>0)
            {
                await _mediator.Publish(new ProductChangedEvent(request.Id), ct);
                return ApiResult<bool>.Success(true, "Product information updated successfully.");
            }    


            return ApiResult<bool>.Failure("Product information update failed.");
        }


        private void UpdateProductCategories(NoName.Domain.Entities.Product product, List<int> newCategoryIds)
        {

            var categoriesToRemove = product.ProductInCategories.Where(pic => !newCategoryIds.Contains(pic.CategoryId)).ToList();

            foreach (var pic in categoriesToRemove)
            {
                product.ProductInCategories.Remove(pic);
            }
            var currentCategoryIds = product.ProductInCategories.Select(pic => pic.CategoryId);
            foreach (var categoryId in newCategoryIds.Where(id => !currentCategoryIds.Contains(id)))
            {
                product.ProductInCategories.Add(new ProductInCategory { CategoryId = categoryId });
            }
        }

        private void UpdateProductTranslations(NoName.Domain.Entities.Product product, List<ProductTranslationDto> requestTranslations)
        {


            var requestLanguageIds = requestTranslations.Select(x => x.LanguageId).ToList();

            var translationsToRemove = product.ProductTranslations.Where(pt => !requestLanguageIds.Contains(pt.LanguageId)).ToList();

            foreach (var pt in translationsToRemove)
            {
                product.ProductTranslations.Remove(pt);
            }

            foreach (var dto in requestTranslations)
            {
                var existing = product.ProductTranslations
                    .FirstOrDefault(x => x.LanguageId == dto.LanguageId);

                if (existing != null)
                {

                    existing.Name = dto.Name;
                    existing.Description = dto.Description;
                    existing.SeoAlias = dto.SeoAlias;
                    existing.Details = dto.Details;
                    existing.SeoDescription = dto.SeoDescription;
                    existing.SeoTitle = dto.SeoTitle;
                }
                else
                {

                    product.ProductTranslations.Add(new ProductTranslation
                    {
                        LanguageId = dto.LanguageId,
                        Name = dto.Name,
                        Description = dto.Description,
                        SeoAlias = dto.SeoAlias,
                        Details = dto.Details,
                        SeoDescription = dto.SeoDescription,
                        SeoTitle = dto.SeoTitle
                    });
                }
            }
        }
    }
}
