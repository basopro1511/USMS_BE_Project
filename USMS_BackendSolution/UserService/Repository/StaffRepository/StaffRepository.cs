using BusinessObject.AppDBContext;
using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;

namespace UserService.Repository.StaffRepository
    {
    public class StaffRepository : IStaffRepository
        {

        #region Get All Staff
        /// <summary>
        /// Get All Staff from Database
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<UserDTO>> GetAllStaff()
            {
            try
                {
                using (var dbcontext = new MyDbContext())
                    {
                    var user = dbcontext.User.Where(x => x.RoleId==2).ToList();
                    List<UserDTO> userDTOs = new List<UserDTO>();
                    foreach (var item in user)
                        {
                        UserDTO userDTO = new UserDTO();
                        userDTO.CopyProperties(item);
                        userDTOs.Add(userDTO);
                        await dbcontext.SaveChangesAsync();
                        }
                    return userDTOs;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Add Staff
        /// <summary>
        /// Thêm nhân viên mới vào database (Async)
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> AddNewStaff(UserDTO userDTO)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var teacher = new User();
                    teacher.CopyProperties(userDTO);
                    teacher.CreatedAt=DateTime.Now;
                    dbContext.User.Add(teacher);
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception("Lỗi khi thêm nhân viên", ex);
                }
            }
        #endregion

        #region Update Staff
        /// <summary>
        /// Update Staff information
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        public async Task<bool> UpdateStaff(UserDTO userDTO)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var user = dbContext.User.FirstOrDefault(x => x.UserId==userDTO.UserId);
                    user.CopyProperties(userDTO);
                    if (user==null)
                        return false;
                    user.UpdatedAt=DateTime.Now;
                    dbContext.Entry(user).State=EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Update Staff (Personal infor )
        /// <summary>
        /// Update Staff's information
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        public async Task<bool> UpdatePersonalInformationForStaff(UserDTO userDTO)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var user = dbContext.User.FirstOrDefault(x => x.UserId==userDTO.UserId);
                    user.CopyProperties(userDTO);
                    if (user==null)
                        return false;
                    user.UpdatedAt=DateTime.Now;
                    dbContext.Entry(user).State=EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception)
                {
                throw;
                }
            }
        #endregion

        #region Add List User ( use for Import Excel )
        /// <summary>
        /// Add Range from excel
        /// </summary>
        /// <param name="staffs"></param>
        /// <returns></returns>
        public async Task<bool> AddStaffsAsync(List<User> staffs)
            {
            try
                {
                using (var _db = new MyDbContext())
                    {
                    await _db.User.AddRangeAsync(staffs);
                    await _db.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception)
                {
                throw;
                }
            }
        #endregion
        }
    }
