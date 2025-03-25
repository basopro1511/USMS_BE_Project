using BusinessObject;
using BusinessObject.ModelDTOs;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Repository.UserRepository;

namespace UserService.Services.LoginServices
    {
    public class LoginService
        {
        private readonly UserRepository _userRepository; // Directly using repository
        private readonly IConfiguration _config;

        public LoginService(IConfiguration config)
            {
            _userRepository=new UserRepository(); // Direct instantiation
            _config=config;
            }

        #region Login By JWT
        /// <summary>
        /// Login by JWT
        /// </summary>
        /// <param name="loginDTO"></param>
        /// <returns></returns>
        public async Task<APIResponse> Login(LoginDTO loginDTO)
            {
            var user =await _userRepository.GetUserByEmail(loginDTO.Email);

            if (user==null)
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Không tìm thấy người dùng."
                    };
                }
            // Verify password
            if (loginDTO.Password!=user.PasswordHash)
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Mật khẩu bạn nhập không đúng."
                    };
                }
            // Generate JWT Token
            var token = GenerateJwtToken(user);

            return new APIResponse
                {
                IsSuccess=true,
                Message="Đăng nhập thành công.",
                Result=new { Token = token, UserId = user.UserId, RoleId = user.RoleId }
                };
            }
        #endregion

        #region Generate JWT Token
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
        #endregion

        }
    }
