using MediatR;
using NoName.Application.Common;
using NoName.Application.Features.Users.Commands.Login;
using NoName.Shared.Contracts.Authentication;

namespace NoName.Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string AccessToken, string RefreshToken) : IRequest<ApiResult<AuthenticatedResponse>>;