using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Authorization.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Author.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorizationService(
            IConfiguration config,
            IHttpContextAccessor httpContextAccessor)
        {
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }

        private string? GetTokenFromHttpContext()
        {
            var authorizationHeader = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                return null;
            }
            return authorizationHeader.Substring("Bearer ".Length).Trim();
        }

        public async Task<APIResponse> ValidateUserRole(string[] allowedRoles)
        {
            string? token = GetTokenFromHttpContext();

            if (string.IsNullOrEmpty(token))
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Không tìm thấy Token trong phiên làm việc."
                };
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtSettings = _config.GetSection("Jwt");
                var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    ValidateLifetime = true
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);

                if (validatedToken is JwtSecurityToken jwtToken && jwtToken.ValidTo < DateTime.UtcNow)
                {
                    return new APIResponse
                    {
                        IsSuccess = false,
                        Message = "Token đã hết hạn."
                    };
                }

                var roleClaim = principal.FindFirst(ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(roleClaim))
                {
                    return new APIResponse
                    {
                        IsSuccess = false,
                        Message = "Không có quyền."
                    };
                }

                if (!allowedRoles.Contains(roleClaim))
                {
                    return new APIResponse
                    {
                        IsSuccess = false,
                        Message = "Bạn không có quyền truy cập API này."
                    };
                }

                return new APIResponse
                {
                    IsSuccess = true,
                    Message = "Xác thực quyền thành công.",
                    Result = roleClaim
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Token không hợp lệ."
                };
            }
        }

        public string? GetCurrentUserId()
        {
            var token = GetTokenFromHttpContext();
            if (string.IsNullOrEmpty(token))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
