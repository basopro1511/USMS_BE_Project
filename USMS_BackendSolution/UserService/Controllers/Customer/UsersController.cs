using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject.AppDBContext;
using BusinessObject.Models;
using BusinessObject.ModelDTOs;
using BusinessObject;
using UserService.Services;
using Microsoft.AspNetCore.Authorization;

namespace UserService.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _userService;
        public UsersController(UsersService userService)
        {
            _userService = userService;
        }

        // GET: api/Users
        [HttpGet]
        public APIResponse GetAllUsers()
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _userService.GetUsers();
            return aPIResponse;
        }
        // GET: api/Users/1
        [HttpGet("{id}")]
        public APIResponse GetUserByEmail(string email)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _userService.GetUserByEmail(email);
            return aPIResponse;
        }
        // POST
        [HttpPost("login")]
        public APIResponse Login(string email, string password)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _userService.Login(email, password);
            return aPIResponse;
        }
        // POST
        [Authorize]
        [HttpGet("getRole")]
        public APIResponse GetUserRole()
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _userService.GetUserRole();
            return aPIResponse;
        }
        // POST: api/User
        [HttpPost]
        public APIResponse AddNewUser(BusinessObject.Models.User user)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _userService.AddNewUser(user);
            return aPIResponse;
        }
        [HttpDelete]
        public APIResponse DeleteUser(string id)
        {
            APIResponse aPIResponse = aPIResponse = _userService.DeleteUser(id);
            return aPIResponse;
        }
    }
}
