using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;

namespace UserService.Repository.StaffRepository
    {
    public interface IStaffRepository
        {
        #region Get All Staff
        /// <summary>
        /// Get All Staff from Database
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<List<User>> GetAllStaff();
        #endregion

        #region Add Staff
        /// <summary>
        /// Thêm nhân viên mới vào database (Async)
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<bool> AddNewStaff(User user);
       #endregion

        #region Update Staff
        /// <summary>
        /// Update Staff information
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        public Task<bool> UpdateStaff(User user);
        #endregion                                          '

        #region Update Staff (Personal infor )
        /// <summary>
        /// Update Staff's information
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        public  Task<bool> UpdatePersonalInformationForStaff(User user);
        #endregion

        #region Add List User ( use for Import Excel )
        /// <summary>
        /// Add Range from excel
        /// </summary>
        /// <param name="teachers"></param>
        /// <returns></returns>
        public  Task<bool> AddStaffsAsync(List<User> users);
        #endregion
        }
    }
