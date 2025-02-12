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
using Microsoft.AspNetCore.Identity.Data;

namespace UserService.Controllers
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
        // POST
        [HttpPost("login")]
        public APIResponse Login([FromBody] APIRequest request)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _userService.Login(request.Email, request.Password);
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
    }
}
