using MediatR;
using NoName.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace NoName.Application.Features.Users.Commands.RegisterUser
{
   

    public class RegisterUserCommand: IRequest<ApiResult<Guid>>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
        public DateTime Dob { get; set; }
    }
}
