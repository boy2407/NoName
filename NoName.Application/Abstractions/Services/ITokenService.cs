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
        
        Task<string> CreateJwtToken(User user);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
