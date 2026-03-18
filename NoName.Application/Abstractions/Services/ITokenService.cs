using NoName.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Abstractions.Services
{
    public interface ITokenService
    {
        
        string CreateJwtToken(User user);
        string GenerateRefreshToken();
        void SetRefreshTokenInCookie(string refreshToken);

        // (Dùng khi cần refresh để biết token đó từng thuộc về ai)
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
