using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoName.Application.Abstractions.Services
{
    public interface IUserContext
    {
        string? UserId { get; }
        string? Email { get; }
        bool IsAdmin { get; }
        bool HasRole(string roleName);
        IEnumerable<string> GetRoles();
    }
}
