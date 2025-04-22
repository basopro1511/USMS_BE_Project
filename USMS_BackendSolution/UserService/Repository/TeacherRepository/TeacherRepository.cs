using BusinessObject.AppDBContext;
using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using OfficeOpenXml;
using System.Globalization;

namespace UserService.Repository.TeacherRepository
    {
    public class TeacherRepository : ITeacherRepository
        {
        #region Get All Teacher
        /// <summary>
        /// Get All Teacher from Database
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<User>> GetAllTeacher()
            {
            try
                {
                using (var dbcontext = new MyDbContext())
                    {
                    var user = await dbcontext.User.Where(x => x.RoleId==4).ToListAsync();
                    return user;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get All Teacher Available by Major Id ( Status = 1 )
        /// <summary>
        /// Get All Teacher Available by Major Id ( Status = 1 )
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<User>> GetAllTeacherAvailableByMajorId(string majorId)
            {
            try
                {
                using (var dbcontext = new MyDbContext())
                    {
                    var user = await dbcontext.User.Where(x => x.Status==1&&x.MajorId==majorId&&x.RoleId==4).ToListAsync();
                    return user;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Add Teacher
        /// <summary>
        /// Thêm giáo viên mới vào database (Async)
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> AddNewTeacher(User user)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    user.CreatedAt=DateTime.Now;
                    await dbContext.User.AddAsync(user);
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception("Lỗi khi thêm giáo viên", ex);
                }
            }
        #endregion


        #region Update Techer
        /// <summary>
        /// Update Teacher information
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        public async Task<bool> UpdateTeacher(User user)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existUser =await dbContext.User.FirstOrDefaultAsync(x => x.UserId==user.UserId);
                    existUser.CopyProperties(user);
                    if (existUser==null)
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
        /// <param name="teachers"></param>
        /// <returns></returns>
        public async Task<bool> AddTeachersAsync(List<User> teachers)
            {
            try
                {
                using (var _db = new MyDbContext())
                    {
                    await _db.User.AddRangeAsync(teachers);
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
