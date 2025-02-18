using BusinessObject;
using BusinessObject.ModelDTOs;
using UserService.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using UserService.Services.UserService;
using Authorization.Services;


namespace UserService.Controllers.User
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly Services.UserService.UserService _userService;
        private readonly LoginService _loginService;
        private readonly IAuthorizationServicee _authorizationService;
        public UserController(Services.UserService.UserService userService, LoginService loginService, IAuthorizationServicee authorizationServicee)
        {
            _userService = userService;
            _loginService = loginService;
            _authorizationService = authorizationServicee;
        }
        // GET: api/Users
        [HttpGet]
        public async Task<APIResponse> GetAllUser()
        {
            var authResponse = await _authorizationService.ValidateUserRole(new[] { "1" });
            if (!authResponse.IsSuccess)
            {
                return new APIResponse
                {
                    IsSuccess = authResponse.IsSuccess,
                    Message = authResponse.Message,
                    Errors = authResponse.Errors,
                    Result = authResponse.Result
                };
            }
            return _userService.GetAllUser();
        }
        // GET: api/User/{id}
        [HttpGet("{id}")]
        public async Task<APIResponse> GetUserById(string id)
        {
            var authResponse = await _authorizationService.ValidateUserRole(new[] {"1" });
            if (!authResponse.IsSuccess)
            {
                return new APIResponse
                {
                    IsSuccess = authResponse.IsSuccess,
                    Message = authResponse.Message,
                    Errors = authResponse.Errors,
                    Result = authResponse.Result
                };
            }
            return _userService.GetUserById(id);
        }
        // POST: api/User
        [HttpPost]
        public async Task<APIResponse> AddUser(AddUserDTO addUserDTO)
        {
            var authResponse = await _authorizationService.ValidateUserRole(new[] { "1" });
            if (!authResponse.IsSuccess)
            {
                return new APIResponse
                {
                    IsSuccess = authResponse.IsSuccess,
                    Message = authResponse.Message,
                    Errors = authResponse.Errors,
                    Result = authResponse.Result
                };
            }
            return _userService.AddUser(addUserDTO);
        }
        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public async Task<APIResponse> UpdateUser(string id, UpdateUserDTO updateUserDTO)
        {
            var authResponse = await _authorizationService.ValidateUserRole(new[] { "1" });
            if (!authResponse.IsSuccess)
            {
                return new APIResponse
                {
                    IsSuccess = authResponse.IsSuccess,
                    Message = authResponse.Message,
                    Errors = authResponse.Errors,
                    Result = authResponse.Result
                };
            }
            return _userService.UpdateUser(id, updateUserDTO);
        }
        //PUT: api/User/{id}
        [HttpPut("/UpdateStudentStatus/{id}")]
        public async Task<APIResponse> UpdateStudentStatus(string id, int status)
        {
            var authResponse = await _authorizationService.ValidateUserRole(new[] { "1" });
            if (!authResponse.IsSuccess)
            {
                return new APIResponse
                {
                    IsSuccess = authResponse.IsSuccess,
                    Message = authResponse.Message,
                    Errors = authResponse.Errors,
                    Result = authResponse.Result
                };
            }
            return _userService.UpdateStudentStatus(id, status);
        }
        //PUT: api/User/{id}
        [HttpPut("/UpdateInfor/{id}")]
        public async Task<APIResponse> UpdateInfor(string id, UpdateInforDTO updateInforDTO)
        {
            var authResponse = await _authorizationService.ValidateUserRole(new[] { "1" });
            if (!authResponse.IsSuccess)
            {
                return new APIResponse
                {
                    IsSuccess = authResponse.IsSuccess,
                    Message = authResponse.Message,
                    Errors = authResponse.Errors,
                    Result = authResponse.Result
                };
            }
            return _userService.UpdateInfor(id, updateInforDTO);
        }
    }
}
