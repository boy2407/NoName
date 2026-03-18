using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using NoName.Application.Common;
using NoName.Application.Features.Users.Commands.RegisterUser;
using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, ApiResult<Guid>>
{
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    // private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }

    public async Task<ApiResult<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        // check if email or username already exists
        var existingEmail = await _userManager.FindByEmailAsync(request.Email);
        if (existingEmail != null)
        {
           return ApiResult<Guid>.Failure("This email address has already been used.");
        }

        var existingUserName = await _userManager.FindByNameAsync(request.UserName);
        if (existingUserName != null)
        {
            return ApiResult<Guid>.Failure("This username has already been used.");
        }

        // Map the request to a User entity
        var user = _mapper.Map<User>(request);
        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return ApiResult<Guid>.Failure($"Registration failed: {errors}");
        }

        return ApiResult<Guid>.Success(user.Id ,"Registraion successed");
    }
}