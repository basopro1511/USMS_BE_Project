using BusinessObject;
using BusinessObject.ModelDTOs;
using UserService.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers.User
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Services.UserServices.UserService _userService;
        public UserController(Services.UserServices.UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _userService.GetAllUser();
            return Ok(aPIResponse);
            }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = await _userService.GetUserById(id);
            return Ok(aPIResponse);
            }

        [HttpPut("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDTO resetPasswordDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = await _userService.ResetPassword(resetPasswordDTO);
            return Ok(aPIResponse);
            }

        [HttpPut("ResetPasswordByEmail")]
        public async Task<IActionResult> ResetPasswordByEmail(ResetPasswordByEmailDTO resetPasswordByEmailDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _userService.ResetPasswordByEmail(resetPasswordByEmailDTO);
            return Ok(aPIResponse);
            }

        [HttpPost("ForgotPassword/{email}")]
        public async Task<IActionResult> ForgotPassword(string email)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _userService.ForgotPassword(email);
            return Ok(aPIResponse);
            }

        [HttpGet("Email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _userService.GetUserByEmail(email);
            return Ok(aPIResponse);
            }
        }
}
