using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NoName.Application.Common;
using NoName.Application.Features.Auth.Commands.RefreshToken;
using NoName.Application.Features.Users.Commands.ConfirmEmail;
using NoName.Application.Features.Users.Commands.Login;
using NoName.Application.Features.Users.Commands.Logout;
using NoName.Application.Features.Users.Commands.RegisterUser;
using NoName.Shared.Contracts.Authentication;
using System.Security.Claims;

namespace NoName.BackendApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator, IConfiguration config) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        try
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                success = false,
                message = ex.Message,
                errorType = ex.GetType().Name
            });
        }
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccessed ? Ok(result) : BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var command = new LoginCommand(request.Username, request.Password);
        var result = await mediator.Send(command);

        if (!result.IsSuccessed)
        {
            return Unauthorized(ApiResult<string>.Failure("Invalid credentials"));
        }

        SetRefreshTokenCookie(result.ResultObj.RefreshToken);
        return Ok(new { AccessToken = result.ResultObj.AccessToken });
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        var refreshToken = Request.Cookies["refreshToken"];
        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized(ApiResult<string>.Failure("No refresh token found."));
        }

        var command = new RefreshTokenCommand(request.AccessToken, refreshToken);
        var result = await mediator.Send(command);

        if (!result.IsSuccessed)
        {
            return Unauthorized(result);
        }

        SetRefreshTokenCookie(result.ResultObj.RefreshToken);
        return Ok(new { AccessToken = result.ResultObj.AccessToken });
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var username = User.FindFirstValue(ClaimTypes.Name) ?? User.Identity?.Name;

        if (string.IsNullOrEmpty(username))
        {
            return Unauthorized("User information was not found in token.");
        }

        var result = await mediator.Send(new LogoutCommand { Username = username });

        if (result.IsSuccessed)
        {
            Response.Cookies.Delete("refreshToken");
            return Ok(new { message = "Logged out successfully." });
        }

        return BadRequest(result.Message);
    }

    private void SetRefreshTokenCookie(string token)
    {
        var expirationDays = double.Parse(config["Jwt:RefreshTokenExpirationDays"] ?? "7");
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(expirationDays)
        };
        Response.Cookies.Append("refreshToken", token, cookieOptions);
    }
}
