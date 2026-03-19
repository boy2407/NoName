using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using NoName.Application.Abstractions.Services;
using NoName.Application.Common;
using NoName.Application.Features.Auth.Commands.RefreshToken;
using NoName.Application.Features.Users.Commands.Login;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Users.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, ApiResult<AuthenticatedResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        public RefreshTokenCommandHandler(UserManager<User> userManager, ITokenService tokenService, IConfiguration config)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _config = config;
        }

        public async Task<ApiResult<AuthenticatedResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var expirationDays = double.Parse(_config["Jwt:RefreshTokenExpirationDays"] ?? "7");

                var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
                if (principal?.Identity?.Name == null)
                {
                    return ApiResult<AuthenticatedResponse>.Failure("Access Token không hợp lệ.");
                }

                var username = principal.Identity.Name;
                var user = await _userManager.FindByNameAsync(username);

               
                if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                {
                    return ApiResult<AuthenticatedResponse>.Failure("Phiên làm việc đã hết hạn hoặc mã không khớp.");
                }

               
                var newAccessToken =  await _tokenService.CreateJwtToken(user);
                var newRefreshToken = await _tokenService.GenerateRefreshToken();

                user.RefreshToken = newRefreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(expirationDays);

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    return ApiResult<AuthenticatedResponse>.Failure("Không thể cập nhật thông tin User vào Database.");
                }

                return ApiResult<AuthenticatedResponse>.Success(new AuthenticatedResponse
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                });
            }
            catch (Exception ex)
            {
             
                return ApiResult<AuthenticatedResponse>.Failure($"Lỗi hệ thống: {ex.Message}");
            }
        }
    }
}
