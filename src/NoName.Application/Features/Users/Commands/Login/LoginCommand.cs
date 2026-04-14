using MediatR;
using NoName.Application.Common;
using NoName.Shared.Contracts.Authentication;

namespace NoName.Application.Features.Users.Commands.Login;

public record LoginCommand(string Username, string Password) : IRequest<ApiResult<AuthenticatedResponse>>;
