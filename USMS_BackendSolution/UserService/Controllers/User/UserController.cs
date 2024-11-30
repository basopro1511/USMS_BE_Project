using BusinessObject;
using BusinessObject.ModelDTOs;
using DataAccess.Services.UserService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers.User
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataAccess.Services.UserService.UserService _userService;
        public UserController(DataAccess.Services.UserService.UserService userService)
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
        // GET: api/User/{id}
        [HttpGet("/Disable/{id}")]
        public APIResponse DisableStudent (string id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _userService.DisableStudent(id);
            return aPIResponse;
        }// GET: api/User/{id}
        [HttpGet("/OnSchedule/{id}")]
        public APIResponse OnScheduleStudent (string id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _userService.OnScheduleStudent(id);
            return aPIResponse;
        }// GET: api/User/{id}
        [HttpGet("/Deferment/{id}")]
        public APIResponse DefermentStudent(string id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _userService.DefermentStudent(id);
            return aPIResponse;
        }// GET: api/User/{id}
        [HttpGet("/Graduated/{id}")]
        public APIResponse GraduatedStudent(string id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _userService.GraduatedStudent(id);
            return aPIResponse;
        }
    }
}
