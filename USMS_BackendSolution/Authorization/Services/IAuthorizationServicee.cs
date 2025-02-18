using Authorization.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authorization.Services
{
    public interface IAuthorizationServicee
    {
        Task<APIResponse> ValidateUserRole(string[] allowedRoles);
        string? GetCurrentUserId();
    }
}
