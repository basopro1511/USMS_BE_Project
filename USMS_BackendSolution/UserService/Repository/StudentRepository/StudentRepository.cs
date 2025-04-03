using BusinessObject.AppDBContext;
using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;

namespace UserService.Repository.StudentRepository
    {
    public class StudentRepository : IStudentRepository
        {
        #region Get All Student
        /// <summary>
        /// Get All Student from Database
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<User>> GetAllStudent()
            {
            try
                {
                using (var dbcontext = new MyDbContext())
                    {
                    var user = await dbcontext.User.Where(x => x.RoleId==5).ToListAsync();
                    return user;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Add Student
        /// <summary>
        /// Thêm sinh viên viên mới vào database (Async)
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> AddNewStudent(User user)
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
                throw new Exception("Lỗi khi thêm sinh viên", ex);
                }
            }
        #endregion

        #region Add Student
        /// <summary>
        /// Thêm sinh viên viên mới vào Student table database (Async)
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> AddNewStudentForStudentTable(Student student)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    await dbContext.Student.AddAsync(student);
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception("Lỗi khi thêm sinh viên", ex);
                }
            }
        #endregion

        #region Update Techer
        /// <summary>
        /// Update Student information
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        public async Task<bool> UpdateStudent(User user)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existUser = await GetStudentById(user.UserId);
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
        public async Task<bool> AddStudentAsync(List<User> users)
            {
            try
                {
                using (var _db = new MyDbContext())
                    {
                    await _db.User.AddRangeAsync(users);
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

        #region Update Term of Student
        /// <summary>
        /// Update Student Term
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="newTerm"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> UpdateStudentTerm(string userId, int newTerm)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    Student existStudent = await dbContext.Student.FirstOrDefaultAsync(x => x.StudentId==userId);
                    if (existStudent!=null)
                        existStudent.Term=newTerm;
                    dbContext.Entry(existStudent).State=EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message, ex);
                }
            }
        #endregion

        #region Get Student by Id 
        public async Task<User> GetStudentById(string userId)
            {
            try
                {
                using (var _db = new MyDbContext())
                    {
                    var user = await _db.User.FirstOrDefaultAsync(x => x.UserId==userId);
                    return user;
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
