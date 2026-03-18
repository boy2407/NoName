using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoName.Application.Common;
using NoName.Application.Features.Auth.Commands.RefreshToken;
using NoName.Application.Features.Users.Commands.Login;
using NoName.Application.Features.Users.Commands.Logout;
using NoName.Application.Features.Users.Commands.RegisterUser;
using System.Security.Claims;

namespace NoName.BackendApi.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _config;

        public AuthController(IMediator mediator, IConfiguration config)
        {
            _mediator = mediator;
            _config = config;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);
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

        private void SetRefreshTokenCookie(string token)
        {
            var expirationDays = double.Parse(_config["Jwt:RefreshTokenExpirationDays"] ?? "7");
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, 
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(expirationDays)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccessed)
            {
                return Unauthorized(ApiResult<string>.Failure("Invalid User or PassWord InCorrect."));
            }

            SetRefreshTokenCookie(result.ResultObj.RefreshToken);

            return Ok(new { AccessToken = result.ResultObj.AccessToken });
        }


        [HttpPost("refresh-token")]

        public async Task<IActionResult> Refresh([FromBody] string expiredAccessToken) 
        {
           
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken)) return Unauthorized(ApiResult<string>.Failure("No Cookie Refresh Token Found."));
            var result = await _mediator.Send(new RefreshTokenCommand
            {
                AccessToken = expiredAccessToken,
                RefreshToken = refreshToken
            });

            if (!result.IsSuccessed) return Unauthorized(result);

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
                return Unauthorized("User information was not found in Token");
            }

            var result = await _mediator.Send(new LogoutCommand { Username = username });

            if (result.IsSuccessed)
            {

                Response.Cookies.Delete("refreshToken");
                return Ok(new { message = " Logged out and delete RefreshToke!" });
            }

            return BadRequest(result.Message);
        }
    }
}
