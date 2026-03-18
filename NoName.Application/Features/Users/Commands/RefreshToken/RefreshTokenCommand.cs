using MediatR;
using NoName.Application.Common;
using NoName.Application.Features.Users.Commands.Login;

namespace NoName.Application.Features.Auth.Commands.RefreshToken
{

    public class RefreshTokenCommand : IRequest<ApiResult<AuthenticatedResponse>>
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}