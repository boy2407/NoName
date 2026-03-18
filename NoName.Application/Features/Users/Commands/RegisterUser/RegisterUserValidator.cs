using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Users.Commands.RegisterUser
{
    public class RegisterUserValidator : AbstractValidator<RegisterUserCommand>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.LastName).NotEmpty().MaximumLength(200);
            RuleFor(x => x.UserName).NotEmpty().MinimumLength(6).WithMessage("Username must be at least 6 characters long.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid Gmail format.");
            RuleFor(x => x.Password).NotEmpty().MinimumLength(6).WithMessage("The password must be at least 6 characters long.");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("The verification password does not match.");
            RuleFor(x => x.Dob).NotEmpty().LessThan(DateTime.Now.AddYears(-16)).WithMessage("Invalid date of birth.");
        }
    }
}
