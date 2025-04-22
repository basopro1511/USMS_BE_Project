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
        public async Task<List<User>> GetAllStaff()
            {
            try
                {
                using (var dbcontext = new MyDbContext())
                    {
                    var user = await dbcontext.User.Where(x => x.RoleId==2).ToListAsync();
                    return user;
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
        public async Task<bool> AddNewStaff(User user)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    await dbContext.User.AddAsync(user);
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
        public async Task<bool> UpdateStaff(User user)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existUser = dbContext.User.FirstOrDefault(x => x.UserId==user.UserId);
                    existUser.CopyProperties(user);
                    if (existUser==null)
                        return false;
                    existUser.UpdatedAt=DateTime.Now;
                    dbContext.Entry(existUser).State=EntityState.Modified;
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
        public async Task<bool> UpdatePersonalInformationForStaff(User user)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existUser = dbContext.User.FirstOrDefault(x => x.UserId==user.UserId);
                    existUser.CopyProperties(user);
                    if (user==null)
                        return false;
                    existUser.UpdatedAt=DateTime.Now;
                    dbContext.Entry(existUser).State=EntityState.Modified;
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
