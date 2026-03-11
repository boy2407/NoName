using FluentValidation;
using NoName.Application.Features.Product.Commands.Create;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class CreateProductValidator : AbstractValidator<CreateProduct>
{
    public CreateProductValidator()
    {

    
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Product name is required.")
            .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters.");

        RuleFor(x => x.Price)
            .NotNull().WithMessage("Price is required.")
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.OriginalPrice)
            .NotNull().WithMessage("Original price is required.")
            .GreaterThan(0).WithMessage("Original price must be greater than 0.");

        RuleFor(x => x.Stock)
            .NotNull().WithMessage("Stock quantity is required.")
            .GreaterThanOrEqualTo(0).WithMessage("Stock cannot be a negative number.");

        RuleFor(x => x.CategoryIds)
            .NotEmpty().WithMessage("At least one category must be selected.");

        RuleFor(x => x.LanguageId)
            .NotEmpty().WithMessage("Language ID is required.");

        RuleFor(x => x.SeoAlias)
            .NotEmpty().WithMessage("SEO Alias is required.");

        RuleFor(x => x.ThumbnailImage)
            .NotNull().WithMessage("A thumbnail image is required.");
    }
}