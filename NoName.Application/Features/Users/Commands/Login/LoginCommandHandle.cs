using MediatR;
using Microsoft.AspNetCore.Identity;
using NoName.Application.Abstractions.Services;
using NoName.Application.Common;
using NoName.Application.Features.Users.Commands.Login;
using NoName.Domain.Entities;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResult<AuthenticatedResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<ApiResult<AuthenticatedResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
            {
                return ApiResult<AuthenticatedResponse>.Failure("Invalid credentials");
            }

            var accessToken = _tokenService.CreateJwtToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return ApiResult<AuthenticatedResponse>.Success(new AuthenticatedResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
    }
}