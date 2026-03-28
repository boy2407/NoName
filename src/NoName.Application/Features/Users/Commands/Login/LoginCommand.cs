using MediatR;
using NoName.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Users.Commands.Login
{
    public record LoginCommand : IRequest<ApiResult<AuthenticatedResponse>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public LoginCommand(string username, string password)
        {
            Username = username;
            Password = password;
        }

    }

    public class AuthenticatedResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
