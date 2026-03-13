using FluentValidation;
using NoName.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.Command.UpdateCategory
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategory>
    {   ICategoryRepository _categoryRepository;
        ILanguageRepository _languageRepository;
        public UpdateCategoryValidator(ICategoryRepository categoryRepository, ILanguageRepository languageRepository)
        {
            _categoryRepository = categoryRepository;  
            _languageRepository = languageRepository;


            RuleFor(x => x.Id)
                .MustAsync(async (id, ct) =>
                {
                    var exists = await _categoryRepository.ExistsAsync(id, ct);
                    return exists;
                }).WithMessage("Category does not exist.")
                .NotNull();

            RuleFor(x => x)
                .Must(x => x.ParentId != x.Id)
                .WithMessage("A category cannot be its own parent.");

            RuleFor(x => x.ParentId)
                .MustAsync(async (parentId, ct) =>
                {  
                    if (!parentId.HasValue)
                        return true;

                    var exists = await _categoryRepository.ExistsAsync(parentId.Value, ct);
                    return exists;
                }).WithMessage("Parent Category does not exist.");


            RuleFor(x => x.Translations)
                .NotNull().WithMessage("There must be at least one language translation.")
                .MustAsync(async (translations, ct) =>
                {
                    foreach (var translation in translations)
                    {
                        var languageExists = await _languageRepository.ExistsAsync(translation.LanguageId, ct);

                        if (!languageExists)
                        {
                            return false;
                        }
                    }
                    return true;
                }).WithMessage($"one or more languages do not exist in the system.")
                .Must(t => t.Select(l => l.LanguageId).Distinct().Count() == t.Count)
                .WithMessage("Language already exists.");

            RuleForEach(x => x.Translations).ChildRules(t => {
                t.RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Translations name is required.")
                    .MaximumLength(200);
                t.RuleFor(x => x.LanguageId)
                    .NotEmpty().WithMessage("Language ID is required.").Length(2, 5);
                t.RuleFor(x => x.SeoAlias)
                  .NotEmpty().WithMessage("SeoAlias is required. / Validator");
                t.RuleFor(x => x.SeoTitle)
                   .NotEmpty().WithMessage("SeoAlias is required. / Validator");
                t.RuleFor(x => x.SeoDescription)
                   .NotEmpty().WithMessage("SeoAlias is required. / Validator");
            });
        }
    }
}
