using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace UserService.Repository.UserRepository
{
    public interface IUserRepository
    {
        public Task<List<UserDTO>> GetAllUser();
        public Task<UserDTO> GetUserById(string id);
        public Task<UserDTO> GetUserByEmail(string email);

        #region Reset password 
        /// <summary>
        /// Reset password for user
        /// </summary>
        /// <param name="resetPasswordDTO"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<bool> ResetPassword(ResetPasswordDTO resetPasswordDTO);
        #endregion

        #region Reset password by Email
        /// <summary>
        /// Reset password for user
        /// </summary>
        /// <param name="resetPasswordDTO"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<bool> ResetPasswordByEmail(ResetPasswordByEmailDTO resetPasswordByEmailDTO);
        #endregion

        #region
        /// <summary>
        /// Change Users Status 
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<bool> ChangeUserStatusSelected(List<string> userIds, int status);
        #endregion
        }
    }
