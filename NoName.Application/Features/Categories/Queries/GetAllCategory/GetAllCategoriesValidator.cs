using FluentValidation;
using NoName.Application.Abstractions.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Categories.Queries.GetCategory
{
    public class GetAllCategoriesValidator : AbstractValidator<GetAllCategoriesQuery>
    {
        private readonly ILanguageRepository _languageRepository;


        public GetAllCategoriesValidator(  ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;

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
