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

        // GET: api/Users
        [HttpGet]
        public async Task<APIResponse> GetAllUser()
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _userService.GetAllUser();
            return aPIResponse;
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        public async Task<APIResponse> GetUserById(string id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = await _userService.GetUserById(id);
            return aPIResponse;
        }

        [HttpPut("ResetPassword")]
        public async Task<APIResponse> ResetPassword(ResetPasswordDTO resetPasswordDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = await _userService.ResetPassword(resetPasswordDTO);
            return aPIResponse;
            }

        //#region  old
        //// POST: api/User
        //[HttpPost]
        //public APIResponse AddUser(AddUserDTO addUserDTO)
        //    {
        //    APIResponse aPIResponse = new APIResponse();
        //    aPIResponse=_userService.AddUser(addUserDTO);
        //    return aPIResponse;
        //    }
        //// PUT: api/User/{id}
        //[HttpPut("{id}")]
        //public APIResponse UpdateUser(string id, UpdateUserDTO updateUserDTO)
        //    {
        //    APIResponse aPIResponse = new APIResponse();
        //    aPIResponse=_userService.UpdateUser(id, updateUserDTO);
        //    return aPIResponse;
        //    }
        ////PUT: api/User/{id}
        //[HttpPut("/UpdateStudentStatus/{id}")]
        //public APIResponse UpdateStudentStatus(string id, int status)
        //    {
        //    APIResponse aPIResponse = new APIResponse();
        //    aPIResponse=_userService.UpdateStudentStatus(id, status);
        //    return aPIResponse;
        //    }
        ////PUT: api/User/{id}
        //[HttpPut("/UpdateInfor/{id}")]
        //public APIResponse UpdateInfor(string id, UpdateInforDTO updateInforDTO)
        //    {
        //    APIResponse aPIResponse = new APIResponse();
        //    aPIResponse=_userService.UpdateInfor(id, updateInforDTO);
        //    return aPIResponse;
        //    }
        //#endregion

        }
}
