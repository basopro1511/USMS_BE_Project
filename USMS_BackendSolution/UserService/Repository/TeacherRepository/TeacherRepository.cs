using BusinessObject.AppDBContext;
using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;

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
        public async Task<List<UserDTO>> GetAllTeacher()
            {
            try
                {
                using (var dbcontext = new MyDbContext())
                    {
                    var user = dbcontext.User.Where(x => x.RoleId==4).ToList();
                    List<UserDTO> userDTOs = new List<UserDTO>();
                    foreach (var item in user)
                        {
                        UserDTO userDTO = new UserDTO();
                        userDTO.CopyProperties(item);
                        userDTOs.Add(userDTO);
                        dbcontext.SaveChanges();
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

        #region Get All Teacher Available by Major Id ( Status = 1 )
        /// <summary>
        /// Get All Teacher Available by Major Id ( Status = 1 )
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<UserDTO>> GetAllTeacherAvailableByMajorId(string majorId)
            {
            try
                {
                var users = await GetAllTeacher();
                var teacherAvailable = users.Where(x => x.Status==1&&x.MajorId==majorId).ToList();
                return teacherAvailable;
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
        public async Task<bool> AddNewTeacher(UserDTO userDTO)
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
        public async Task<bool> UpdateTeacher(UserDTO userDTO)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var user = dbContext.User.FirstOrDefault(x => x.UserId==userDTO.UserId);
                    user.CopyProperties(userDTO);
                    if (user==null)
                        return false;
                    user.UpdatedAt = DateTime.Now;
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

        }
    }
