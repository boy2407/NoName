using MediatR;
using Microsoft.AspNetCore.Identity;
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

namespace NoName.Application.Features.Users.Commands.Logout
{
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, ApiResult<string>>
    {
        UserManager<User> _userManager;
        public LogoutCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<ApiResult<string>> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user == null) return ApiResult<string>.Failure("Invalid User .");

            user.RefreshToken = null;
            user.RefreshTokenExpiryTime = null; // Nhớ cho phép Nullable ở class User

            await _userManager.UpdateAsync(user);

            return ApiResult<string>.Success("Đã đăng xuất thành công.");
        }

    }
}
