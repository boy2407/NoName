using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Languages.Commands.UpdateLanguage
{
    public class UpdateLanguageValidator : AbstractValidator<UpdateLanguageCommand>
    {
        public UpdateLanguageValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(50);
        }
    }
}
