using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Languages.Commands.CreateLanguage
{
    public class CreateLanguageValidator : AbstractValidator<CreateLanguage>
    {
        public CreateLanguageValidator()
        {
           
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Language ID is required.")
                .Length(2).WithMessage("Language ID must be exactly 2 characters (e.g., 'vi', 'en').");

         
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Language name is required.")
                .MaximumLength(50).WithMessage("Language name cannot exceed 50 characters.");

        }
    }
}
