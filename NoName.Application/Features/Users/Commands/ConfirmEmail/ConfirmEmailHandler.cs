using MediatR;
using Microsoft.AspNetCore.Identity;
using NoName.Application.Common;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NoName.Application.Features.Users.Commands.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, ApiResult<bool>>
    {
        private readonly UserManager<User> _userManager;

        public ConfirmEmailCommandHandler(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ApiResult<bool>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.UserId) || string.IsNullOrWhiteSpace(request.Token))
            {
                return ApiResult<bool>.Failure("UserId hoặc Token Invalid.");
            }

            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                return ApiResult<bool>.Failure($"No user with this id : {request.UserId}");
            }

            if (user.EmailConfirmed)
            {
                return ApiResult<bool>.Success(true, "// Email already confirmed.");
            }

      
            var result = await _userManager.ConfirmEmailAsync(user, request.Token);

            if (result.Succeeded)
            {
                return ApiResult<bool>.Success(true, "Email verification  successful!");
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return ApiResult<bool>.Failure($"// Failed to confirm email. Errors : {errors}");
        }
    }
}
