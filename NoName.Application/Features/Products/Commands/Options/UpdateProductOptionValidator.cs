using FluentValidation;
using NoName.Application.Abstractions;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Abstractions.Services;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Commands.Options
{
    public class UpdateProductOptionValidator : AbstractValidator<UpdateProductOptionCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILanguageService _languageService;

        public UpdateProductOptionValidator(IUnitOfWork unitOfWork, ILanguageService languageService)
        {
            _unitOfWork = unitOfWork;
            _languageService = languageService;

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId is required")
                .MustAsync(ProductExists).WithMessage("Product does not exist");

            RuleFor(x => x.OptionId)
                .GreaterThan(0).WithMessage("OptionId is required");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Option name is required")
                .MaximumLength(200);

            RuleFor(x => x.Values)
                .NotNull().WithMessage("Values must be provided (can be empty list)")
                .Must(v => v.Select(s => s?.Trim()).Where(s => !string.IsNullOrEmpty(s)).Distinct(System.StringComparer.OrdinalIgnoreCase).Count()
                          == v.Where(s => !string.IsNullOrWhiteSpace(s)).Count())
                .WithMessage("Duplicate values are not allowed in the list");

            RuleFor(x => x)
                .MustAsync(async (req, ct) =>
                {
                    var currentLang = await _languageService.GetCurrentLanguage();
                    var product = await _unitOfWork.Products.GetProductForUpdateAsync(req.ProductId, ct);
                    if (product == null) return false;
                    // check option exists
                    var option = product.Options.FirstOrDefault(o => o.Id == req.OptionId);
                    if (option == null) return false;

                    // check name not duplicate among other options
                    return !product.Options.Any(o =>
                        o.Id != req.OptionId &&
                        o.ProductOptionTranslations.Any(t =>
                            t.LanguageId == currentLang &&
                            string.Equals(t.Name?.Trim(), req.Name?.Trim(), System.StringComparison.OrdinalIgnoreCase)));
                }).WithMessage("An option with the same name already exists for this product");
        }

        private async Task<bool> ProductExists(int productId, CancellationToken ct)
        {
            return await _unitOfWork.Products.ExistsAsync(productId, ct);
        }
    }
}
