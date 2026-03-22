using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NoName.Application.Abstractions.Services;
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
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;
    private readonly IEmailService _emailService;
    // private readonly IUnitOfWork _unitOfWork;

    public RegisterUserCommandHandler(UserManager<User> userManager, IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
    {
        _userManager = userManager;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _emailService = emailService;
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

        await _userManager.AddToRoleAsync(user, "Customer");
        // Email Veriftation and send confirmation email





        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var requestUrl = _httpContextAccessor.HttpContext.Request;
        var baseUrl = $"{requestUrl.Scheme}://{requestUrl.Host}";
        var confirmationLink = $"{baseUrl}/api/auth/confirm-email?userId={user.Id}&token={Uri.EscapeDataString(token)}";
        Console.WriteLine("===============================================");
        Console.WriteLine($"CONFIRM EMAIL LINK: {confirmationLink}");
        Console.WriteLine("===============================================");



        var emailBody = $@"
<div style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; max-width: 600px; margin: 0 auto; border: 1px solid #e0e0e0; border-radius: 10px; overflow: hidden;"">
    <div style=""background-color: #0078d4; padding: 20px; text-align: center; color: white;"">
        <h1 style=""margin: 0; font-size: 24px;"">Chào mừng bạn đến với NoName Shop!</h1>
    </div>
    <div style=""padding: 30px; line-height: 1.6; color: #333;"">
        <p style=""font-size: 18px;"">Xin chào <strong>{user.FirstName} {user.LastName}</strong>,</p>
        <p>Cảm ơn bạn đã đăng ký tài khoản tại hệ thống của chúng tôi. Để hoàn tất quá trình đăng ký và bảo mật tài khoản, vui lòng xác nhận địa chỉ email của bạn.</p>
        
        <div style=""text-align: center; margin: 30px 0;"">
            <a href='{confirmationLink}' 
               style=""background-color: #0078d4; color: white; padding: 15px 25px; text-decoration: none; font-weight: bold; border-radius: 5px; display: inline-block; box-shadow: 0 4px 6px rgba(0,0,0,0.1);"">
               XÁC NHẬN EMAIL NGAY
            </a>
        </div>

        <p style=""font-size: 13px; color: #666;"">Nếu nút trên không hoạt động, bạn có thể copy và dán đường link này vào trình duyệt:</p>
        <p style=""font-size: 12px; color: #0078d4; word-break: break-all;"">{confirmationLink}</p>
        
        <hr style=""border: 0; border-top: 1px solid #eee; margin: 20px 0;"" />
        <p style=""font-size: 12px; color: #999; text-align: center;"">
            Đây là email tự động, vui lòng không phản hồi email này.<br/>
            &copy; 2026 NoName Team. All rights reserved.
        </p>
    </div>
</div>";
        try
        {
            await _emailService.SendEmailAsync(user.Email, "Xác nhận tài khoản NoName Shop", emailBody);
        }
        catch (Exception ex)
        {
           
            Console.WriteLine($"Lỗi gửi mail: {ex.Message}");
           
        }


        return ApiResult<Guid>.Success(user.Id ,"Registraion successed");
    }
}