using MediatR;
using NoName.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Users.Commands.Logout
{
    public class LogoutCommand : IRequest<ApiResult<string>>
    {
        public string Username { get; set; }
    }
}
