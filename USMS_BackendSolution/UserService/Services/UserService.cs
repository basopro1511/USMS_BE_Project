using BusinessObject;
using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using IRepository.IUserRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Services
{
    public class UsersService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UsersService()
        {
            _userRepository = new UserRepository();
            _httpContextAccessor = new HttpContextAccessor();
        }
        #region Get All User
        /// <summary>
        /// Get All User in database
        /// </summary>
        /// <returns></returns>
        public APIResponse GetUsers()
        {
            APIResponse aPIResponse = new APIResponse();
            List<User> users = _userRepository.getAllUser();
            if (users == null)
            {
                aPIResponse.IsSuccess = true;
                aPIResponse.Message = "Customer List Empty";
            }
            aPIResponse.Result = users;
            return aPIResponse;
        }
        #endregion

        #region GetUserByEmail
        /// <summary>
        /// Get User by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A user by their email</returns>
        public APIResponse GetUserByEmail(string email)
        {
            APIResponse aPIResponse = new APIResponse();
            User user = _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "User with email: " + email + " is not found";
            }
            aPIResponse.Result = user;
            return aPIResponse;
        }
        #endregion

        #region Login
        /// <summary>
        /// Login User by email and password
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>A user by their email and password</returns>
        public APIResponse Login(string email, string password)
        {
            APIResponse aPIResponse = new APIResponse();
            User user = _userRepository.GetUserByEmail(email);
            string jwt = "";
            if (user == null)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "User with email: " + email + " is not found";
            }
            else
            {
                if (user.PasswordHash == password)
                {
                    aPIResponse.IsSuccess=true;
                    aPIResponse.Message = "User found, JWT generated";
                    jwt = GenerateJwtToken(email, user.Role.RoleName);
                }
            }
            aPIResponse.Result = jwt;
            return aPIResponse;
        }
        #endregion

        #region Add new User
        /// <summary>
        /// Add New User to databse
        /// </summary>
        /// <param name="user"></param>
        public APIResponse AddNewUser(User user)
        {
            var existingUser = _userRepository.GetUserByEmail(user.Email);
            if (existingUser != null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "User with the given email already exists."
                };
            }
            bool isAdded = _userRepository.AddNewUser(user);
            if (isAdded)
            {
                return new APIResponse
                {
                    IsSuccess = true,
                    Message = "Customer added successfully."
                };
            }
            return new APIResponse
            {
                IsSuccess = false,
                Message = "Failed to add customer."
            };
        }
        #endregion

        #region Delete User
        /// <summary>
        /// Delete A User in User List
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public APIResponse DeleteUser(string email)
        {
            APIResponse aPIResponse = new APIResponse();
            var existingUser = GetUserByEmail(email);
            if (existingUser.Result == null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Cannot found the user with email: " + email
                };
            }
            bool isRemoved = _userRepository.DeleteUser(email);
            if (isRemoved)
            {
                return new APIResponse
                {
                    IsSuccess = true,
                    Message = "User Removed successfully."
                };
            }
            return new APIResponse
            {
                IsSuccess = false,
                Message = "Failed to Removed user."
            };
        }
        #endregion

        #region GenerateJWT
        /// <summary>
        /// Generate a JWT
        /// </summary>
        /// <param name="email"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public string GenerateJwtToken(string email, string role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("This_is_my_super_secret_key_2425");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] 
                { 
                    new Claim("email", email),
                    new Claim("role", role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = "http://localhost",
                Audience = "http://localhost",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        #endregion

        #region Get User Role
        /// <summary>
        /// Get the role of the user who logged in
        /// </summary>
        /// <returns></returns>
        public APIResponse GetUserRole()
        {
            APIResponse aPIResponse = new APIResponse();
            var user = _httpContextAccessor.HttpContext?.User;
            var role = user.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Value;
            aPIResponse.IsSuccess = true;
            aPIResponse.Result = role;
            return aPIResponse;
        }
        #endregion
    }
}
