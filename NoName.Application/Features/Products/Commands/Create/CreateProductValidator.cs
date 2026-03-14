using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NoName.Application.Abstractions.Persistence;
using NoName.Application.Features.Product.DTOs;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Commands.Create
{
    public class CreateProductValidator : AbstractValidator<CreateProduct>
    {
        ICategoryRepository _categoryRepository;
        ILanguageRepository _languageRepository;
        public CreateProductValidator(ICategoryRepository categoryRepository, ILanguageRepository languageRepository)
        {
            _categoryRepository = categoryRepository;
            _languageRepository = languageRepository;

            RuleFor(x => x.Price)
                 .GreaterThan(0).WithMessage("Price must be greater than 0");
            RuleFor(x => x.OriginalPrice)
                .GreaterThan(0).WithMessage("Original price must be greater than 0");
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative");


            //// category rules
            RuleForEach(x => x.CategoryIds)
                .MustAsync(async (id, ct) =>
                {
                    var exists = await _categoryRepository.ExistsAsync(id, ct);
                    return exists;
                }).WithMessage("Category does not exist.")
                .NotEmpty().WithMessage("Please select at least one category for the product");


            RuleFor(x => x.CategoryIds)
                .Must(t => t.Select(ct => ct).Distinct().Count() == t.Count)
                .WithMessage("Category already exists.");


            //// LAnguage translation rules
            ///
            RuleFor(x => x.Translations)
              .NotEmpty().WithMessage("Product must have at least one language translation");

            RuleForEach(x => x.Translations).ChildRules(t =>
            {
                t.RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Product name is required")
                    .MaximumLength(200).WithMessage("Product name cannot exceed 200 characters");
                t.RuleFor(x => x.SeoAlias)
                    .NotEmpty().WithMessage("SEO Alias is required");
                t.RuleFor(x => x.SeoDescription)
                     .NotEmpty().WithMessage("SEO Alias is required");
                t.RuleFor(x => x.SeoTitle)
                     .NotEmpty().WithMessage("SEO Alias is required");
                t.RuleFor(x => x.Description)
                    .NotEmpty().WithMessage("Description is required");
                t.RuleFor(x => x.Details)
                    .NotEmpty().WithMessage("Product details are required");
                t.RuleFor(x => x.LanguageId)
                    .MustAsync(async (id, ct) =>
                    {
                        var exists = await _languageRepository.ExistsAsync(id, ct);
                        return exists;
                    }).WithMessage("'{PropertyValue}'Language does not exist.")
                    .NotEmpty().WithMessage("Language ID is required");
        

            });

            RuleFor(x => x.Translations)
                .Must(t => t.Select(l => l.LanguageId).Distinct().Count() == t.Count)
                .WithMessage("Language already exists.");

            RuleFor(x => x.Translations)
                .Must(x => x != null && x.Any(t=>string.Equals(t.LanguageId, "vi-VN", StringComparison.OrdinalIgnoreCase)))
                .WithMessage("Product information in Vietnamese (vi-VN) is mandatory");
        }
    }

}
