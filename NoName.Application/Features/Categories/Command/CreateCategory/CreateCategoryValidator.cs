using AutoMapper;
using FluentValidation;
using NoName.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.Command.CreateCategory
{
    public class CreateCategoryValidator : AbstractValidator<CreateCategory>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILanguageRepository _languageRepository;
      
        public CreateCategoryValidator(ICategoryRepository categoryRepository, ILanguageRepository languageRepository)
        {
            _categoryRepository = categoryRepository;
            _languageRepository = languageRepository;
            RuleLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.SortOrder).GreaterThanOrEqualTo(0);
            RuleFor(x => x.Status).IsInEnum()
            .WithMessage("Status is invalid");
            RuleFor(x => x.ParentId)
                .MustAsync(async (parentId, ct) =>
                { 
                    if (parentId == null) return true;
                    if (parentId <= 0) return false; 
                    return await _categoryRepository.ExistsAsync(parentId.Value, ct);
                  
                })
                .WithMessage("Parent Category does not exist. / Validator");

            RuleFor(x => x.Translations)
                .NotEmpty().WithMessage("There must be at least one language translation. / Validator");

            RuleForEach(x => x.Translations).ChildRules(t =>
            {
                t.RuleFor(x => x.LanguageId)
                    .NotEmpty().WithMessage("Language ID is required. / Validator").Length(2, 5)
                    .MustAsync(async (langId, ct) =>
                    {
                        var language = await _languageRepository.GetByIdAsync(langId, ct);
                        return language != null;
                    }).WithMessage("Language does not exist");

                t.RuleFor(x => x.Name)

                    .NotEmpty().WithMessage("Translations name is required. / Validator")
                    .MaximumLength(200);
                t.RuleFor(x => x.SeoAlias)
                    .NotEmpty().WithMessage("SeoAlias is required. / Validator");
                t.RuleFor(x => x.SeoTitle)
                   .NotEmpty().WithMessage("SeoAlias is required. / Validator");
                t.RuleFor(x => x.SeoDescription)
                   .NotEmpty().WithMessage("SeoAlias is required. / Validator");
            });

           RuleFor(x => x.Translations)
              .Must(x => x != null && x.Any(t => string.Equals(t.LanguageId, "vi-VN", StringComparison.OrdinalIgnoreCase)))
              .WithMessage("Category information in Vietnamese (vi-VN) is mandatory");
        }
    }
}
