using BusinessObject;
using BusinessObject.ModelDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserService.Services.LoginServices;

namespace UserService.Controllers.AuthController
    {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
        {
        private readonly IConfiguration _config;

        private readonly LoginService _loginService;

        public AuthController(IConfiguration config, LoginService loginService)
            {
            _config=config;
            _loginService=loginService;
            }

        [HttpPost]
        public async Task<APIResponse> Login([FromBody] LoginDTO model)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse= await _loginService.Login(model);
            return aPIResponse;
            }

        private string GenerateJwtToken(string username)
            {
            var jwtSettings = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
          {
           new Claim(ClaimTypes.Name, username),
           new Claim(ClaimTypes.Role, "Admin")  // Gán quyền hạn cho user
         };

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),  // Token hết hạn sau 1 giờ
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }
