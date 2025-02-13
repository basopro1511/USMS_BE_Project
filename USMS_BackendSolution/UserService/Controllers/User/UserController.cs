using BusinessObject;
using BusinessObject.ModelDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace UserService.Controllers.User
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly UserService.Services.UserService.UserService _userService;
		public UserController(UserService.Services.UserService.UserService userService)
		{
			_userService = userService;
		}

		// GET: api/Users
		[HttpGet]
		public APIResponse GetAllUser()
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _userService.GetAllUser();
			return aPIResponse;
		}
		// GET: api/User/{id}
		[HttpGet("{id}")]
		public APIResponse GetUserById(string id)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _userService.GetUserById(id);
			return aPIResponse;
		}
		// POST: api/User
		[HttpPost]
		public APIResponse AddUser(AddUserDTO addUserDTO)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _userService.AddUser(addUserDTO);
			return aPIResponse;
		}
		// PUT: api/User/{id}
		[HttpPut("{id}")]
		public APIResponse UpdateUser(string id, UpdateUserDTO updateUserDTO)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _userService.UpdateUser(id, updateUserDTO);
			return aPIResponse;
		}
		//PUT: api/User/{id}
		[HttpPut("/UpdateStudentStatus/{id}")]
		public APIResponse UpdateStudentStatus(string id, int status)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _userService.UpdateStudentStatus(id, status);
			return aPIResponse;
		}
		//PUT: api/User/{id}
		[HttpPut("/UpdateInfor/{id}")]
		public APIResponse UpdateInfor(string id, UpdateInforDTO updateInforDTO)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _userService.UpdateInfor(id, updateInforDTO);
			return aPIResponse;
		}
		[HttpPost("ForgotPassword")]
		public APIResponse ForgotPassword(string email)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _userService.ForgotPassword(email);
			return aPIResponse;
		}
		[HttpPost("ResetPassword")]
		public APIResponse ResetPassword(ResetPasswordDTO resetPassword)
		{
			APIResponse aPIResponse = new APIResponse();
			if (string.IsNullOrEmpty(resetPassword.Email))
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "Lỗi hệ thống vui lòng thử lại!";
				return aPIResponse;
			}
			if (string.IsNullOrEmpty(resetPassword.Password))
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "Vui lòng nhập mật khẩu mới!";
				return aPIResponse;
			}
			if (resetPassword.Password.Length < 8 || resetPassword.Password.Length > 20)
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "Mật khẩu phải từ 8 đến 20 ký tự!";
				return aPIResponse;
			}
			aPIResponse = _userService.ResetPassword(resetPassword);
			return aPIResponse;
		}
	}
}
