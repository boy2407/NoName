using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Products.Commands.Update.common
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductValidator()
        {
         
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Translations)
              .NotEmpty().WithMessage("There must be at least one language translation. ");

            RuleFor(x => x.CategoryIds)
                .NotEmpty().WithMessage("The product muse be at least one Category.")
                .Must(ids => ids.Count == ids.Distinct().Count())
                .WithMessage("The list categories must not contain duplicate ID.");

            RuleFor(x => x.CategoryIds)
                .Must(t => t.Select(ct => ct).Distinct().Count() == t.Count)
                .WithMessage("Each category allow to have one translation.");

            RuleForEach(x => x.Translations).ChildRules(trans =>
            {
                trans.RuleFor(t => t.LanguageId).NotEmpty().WithMessage("Language Id is required .");
                trans.RuleFor(t => t.Name).NotEmpty().MaximumLength(250).WithMessage("Product name is required and least 250 chacate.");
                trans.RuleFor(t => t.SeoAlias).NotEmpty().WithMessage("Seo Alias is required.");
            });

            RuleFor(x => x.Translations)
                .Must(t => t.Select(x => x.LanguageId).Count() == t.Select(x => x.LanguageId).Distinct().Count())
                .WithMessage("Each language allow to have one translation");
        }
    }
}
