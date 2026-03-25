using MediatR;
using NoName.Application.Abstractions;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Abstractions.Services;
using NoName.Application.Common;
using NoName.Domain.Entities;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Commands.Options
{
    public class UpdateProductOptionHandler : IRequestHandler<UpdateProductOptionCommand, ApiResult<bool>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILanguageService _languageService;

        public UpdateProductOptionHandler(IUnitOfWork unitOfWork, ILanguageService languageService)
        {
            _unitOfWork = unitOfWork;
            _languageService = languageService;
        }

        public async Task<ApiResult<bool>> Handle(UpdateProductOptionCommand request, CancellationToken cancellationToken)
        {
            var currentLang = await _languageService.GetCurrentLanguage();
            var product = await _unitOfWork.Products.GetProductForUpdateAsync(request.ProductId, cancellationToken);
            if (product == null) throw new NotFoundException(nameof(Product), request.ProductId);

            var option = product.Options.FirstOrDefault(o => o.Id == request.OptionId);
            if (option == null) throw new NotFoundException(nameof(ProductOption), request.OptionId);

            // check duplicate values between existing values and incoming ones
            var incomingValues = request.Values.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).Distinct(System.StringComparer.OrdinalIgnoreCase).ToList();

            if (incomingValues.Count != request.Values.Where(s => !string.IsNullOrWhiteSpace(s)).Count())
                throw new FluentValidation.ValidationException("Duplicate values are not allowed in the list");

            var optionTranslation = option.ProductOptionTranslations.FirstOrDefault(t => t.LanguageId == currentLang);
            if (optionTranslation == null)
            {
                option.ProductOptionTranslations.Add(new ProductOptionTranslation
                {
                    LanguageId = currentLang,
                    Name = request.Name?.Trim()
                });
            }
            else
            {
                optionTranslation.Name = request.Name?.Trim();
            }

            // Replace values: remove existing values not in incoming, add new ones
            var existing = option.Values.ToList();

            // remove values not in incoming
            foreach (var ex in existing)
            {
                var existingName = ex.ProductOptionValueTranslations.FirstOrDefault(t => t.LanguageId == currentLang)?.Name;
                if (!incomingValues.Any(v => string.Equals(v, existingName, System.StringComparison.OrdinalIgnoreCase)))
                {
                    option.Values.Remove(ex);
                }
            }

            // add new values
            foreach (var val in incomingValues)
            {
                if (!option.Values.Any(v => string.Equals(
                    v.ProductOptionValueTranslations.FirstOrDefault(t => t.LanguageId == currentLang)?.Name,
                    val,
                    System.StringComparison.OrdinalIgnoreCase)))
                {
                    option.Values.Add(new ProductOptionValue
                    {
                        ProductOptionValueTranslations = new System.Collections.Generic.List<ProductOptionValueTranslation>
                        {
                            new ProductOptionValueTranslation
                            {
                                LanguageId = currentLang,
                                Name = val
                            }
                        }
                    });
                }
            }

            await _unitOfWork.Products.UpdateAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ApiResult<bool>.Success(true);
        }
    }
}
