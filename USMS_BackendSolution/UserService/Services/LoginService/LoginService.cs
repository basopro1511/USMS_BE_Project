using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BusinessObject.ModelDTOs;
using BusinessObject;
using Microsoft.IdentityModel.Tokens;
using UserService.Repository.UserRepository;
using Microsoft.AspNetCore.Identity.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography;

namespace UserService.Services.UserService
{
    public class LoginService
    {
        private readonly UserRepository _userRepository; // Directly using repository
        private readonly IConfiguration _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public LoginService(IConfiguration config, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = new UserRepository();
            _config = config;
            _httpContextAccessor = httpContextAccessor; // Lưu HttpContext để lấy token từ request
        }
        public APIResponse Login(LoginDTO.LoginRequest loginDTO)
        {
            var user = _userRepository.GetUserByEmail(loginDTO.Email);
            if (user == null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Không tìm thấy người dùng."
                };
            }
            loginDTO.Password = HashPassword(loginDTO.Password);
            // Verify password
            if (loginDTO.Password != user.PasswordHash)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Mật khẩu bạn nhập không đúng."
                };
            }
            // Generate JWT Token
            var token = GenerateJwtToken(user);
            return new APIResponse
            {
                IsSuccess = true,
                Message = "Đăng nhập thành công.",
                Result = new { Token = token, UserId = user.UserId, RoleId = user.RoleId }
            };
        }
        private string GenerateJwtToken(UserDTO user)
        {
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId),
                new Claim(ClaimTypes.Email, user.PersonalEmail),
                new Claim(ClaimTypes.Role, user.RoleId.ToString())
            };
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        /// <summary>
        /// Hashing password
        /// </summary>
        /// <param name="plainPassword"></param>
        /// <returns></returns>
        public string HashPassword(string plainPassword)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(plainPassword);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
