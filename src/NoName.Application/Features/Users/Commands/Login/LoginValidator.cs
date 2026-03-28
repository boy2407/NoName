using FluentValidation;
using NoName.Application.Features.Users.Commands.Login;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Auth.Commands.Login
{
    public class LoginValidator : AbstractValidator<LoginCommand>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("User name is required")
                .MinimumLength(3).WithMessage("// User name must be at least taken 6 characters long");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Pass word must not be empty")
                .MinimumLength(6).WithMessage("//Pass word must be at least taken 6 characters long");
        }
    }
}
