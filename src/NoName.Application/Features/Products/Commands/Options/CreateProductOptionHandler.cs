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
    public class CreateProductOptionHandler : IRequestHandler<CreateProductOptionCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILanguageService _languageService;

        public CreateProductOptionHandler(IUnitOfWork unitOfWork, ILanguageService languageService)
        {
            _unitOfWork = unitOfWork;
            _languageService = languageService;
        }

        public async Task<int> Handle(CreateProductOptionCommand request, CancellationToken cancellationToken)
        {
            var currentLang = await _languageService.GetCurrentLanguage();
            var product = await _unitOfWork.Products.GetProductForUpdateAsync(request.ProductId, cancellationToken);
            if (product == null) throw new NotFoundException(nameof(Product), request.ProductId);
            // Double-check duplicate option name
            if (product.Options.Any(o => o.ProductOptionTranslations.Any(t =>
                t.LanguageId == currentLang &&
                string.Equals(t.Name?.Trim(), request.Name?.Trim(), System.StringComparison.OrdinalIgnoreCase))))
                throw new FluentValidation.ValidationException("An option with the same name already exists for this product");

            var option = new ProductOption
            {
                ProductId = request.ProductId
            };

            option.ProductOptionTranslations.Add(new ProductOptionTranslation
            {
                LanguageId = currentLang,
                Name = request.Name?.Trim()
            });

            foreach (var val in request.Values.Where(s => !string.IsNullOrWhiteSpace(s)).Select(s => s.Trim()).Distinct(System.StringComparer.OrdinalIgnoreCase))
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

            product.Options.Add(option);

            await _unitOfWork.Products.UpdateAsync(product, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return option.Id;
        }
    }
}
