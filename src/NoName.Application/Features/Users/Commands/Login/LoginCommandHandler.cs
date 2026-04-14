using MediatR;
using Microsoft.AspNetCore.Identity;
using NoName.Application.Abstractions.Services;
using NoName.Application.Common;
using NoName.Domain.Entities;
using NoName.Shared.Contracts.Authentication;

namespace NoName.Application.Features.Users.Commands.Login;

public class LoginCommandHandler(UserManager<User> userManager, ITokenService tokenService) : IRequestHandler<LoginCommand, ApiResult<AuthenticatedResponse>>
{
    public async Task<ApiResult<AuthenticatedResponse>> Handle( LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByNameAsync(request.Username);
        if (user == null || !await userManager.CheckPasswordAsync(user, request.Password))
        {
            return ApiResult<AuthenticatedResponse>.Failure("Invalid credentials");
        }

        if (!user.EmailConfirmed)
        {
            return ApiResult<AuthenticatedResponse>.Failure("Unverified email");
        }

        var accessToken = await tokenService.CreateJwtToken(user);
        var refreshToken = await tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await userManager.UpdateAsync(user);

        return ApiResult<AuthenticatedResponse>.Success(
            new AuthenticatedResponse(accessToken, refreshToken));
    }
}
