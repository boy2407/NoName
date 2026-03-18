using MediatR;
using NoName.Application.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Features.Users.Commands.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<ApiResult<bool>>
    {
        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
