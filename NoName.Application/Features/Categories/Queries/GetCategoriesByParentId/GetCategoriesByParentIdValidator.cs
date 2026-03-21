using FluentValidation;
using NoName.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.Queries.GetCategoriesByParentId
{
    public class GetCategoriesByParentIdValidator : AbstractValidator<GetCategoriesByParentIdQuery>
    {
        private readonly ILanguageRepository _languageRepository;
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoriesByParentIdValidator( ICategoryRepository categoryRepository, ILanguageRepository languageRepository)
        {
            _categoryRepository = categoryRepository;
            _languageRepository = languageRepository;

            // check if ParentId is exist in the system 
            RuleFor(x => x.ParentId)
            .MustAsync(async (parentId, cancellation) =>
            {   // big root 
                if (parentId == null) return true;

                return await _categoryRepository.ExistsAsync(parentId.Value, cancellation);
            })
            .WithMessage(x=> $"Parent Category '{x.ParentId}' is not exist.");

            RuleFor(x => x.LanguageId)
             .NotEmpty().WithMessage("LanguageId is required") 
             .MustAsync(async (langId, cancellation) =>
             {
                 var language = await _languageRepository.GetByIdAsync(langId, cancellation);
                 return language != null;
             })
             .WithMessage(x => $"The language '{x.LanguageId}' does not exist in the system.");
        }
    }
}
