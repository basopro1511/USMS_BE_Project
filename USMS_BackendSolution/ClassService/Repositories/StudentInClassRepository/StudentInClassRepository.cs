using ClassBusinessObject.AppDBContext;
using ClassBusinessObject.ModelDTOs;
using ClassBusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using SchedulerBusinessObject.ModelDTOs;

namespace ClassService.Repositories.StudentInClassRepository
    {
    public class StudentInClassRepository : IStudentInClassRepository
        {
        #region Get All Student In Class
        /// <summary>
        /// Get All Student In Class
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<StudentInClass>> GetAllStudentInClass()
            {
            try
                {
                using (var _dbContext = new MyDbContext())
                    {
                    List<StudentInClass> studentInClasses = await _dbContext.StudentInClass.ToListAsync();
                    return studentInClasses;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Student In Class by StudentId
        /// <summary>
        /// Get Student In Class by StudentId
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<StudentInClass> GetStudentInClassByStudentId(string studentId)
            {
            try
                {
                using (var _dbContext = new MyDbContext())
                    {
                    StudentInClass studentInClassDTO = await _dbContext.StudentInClass.FirstOrDefaultAsync(sic => sic.StudentId==studentId);
                    return studentInClassDTO;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Student In Class by ClassId
        /// <summary>
        /// Get Student In Class by ClassId
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<StudentInClass>> GetStudentInClassByClassId(int classSubjectId)
            {
            try
                {
                using (var _dbContext = new MyDbContext())
                    {
                    var studentInClass = await _dbContext.StudentInClass.Where(c => c.ClassSubjectId==classSubjectId).ToListAsync();
                    return studentInClass;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get ClassSubjectId by StudentId
        /// <summary>
        /// Retrieves the list of ClassSubjectIds for a given student.
        /// </summary>
        /// <param name="studentId">Student ID</param>
        /// <returns>List of ClassSubjectIds</returns>
        public async Task<List<int>> GetClassSubjectId(string studentId)
            {
            try
                {
                using (var _dbContext = new MyDbContext())
                    {
                    var classSubjectIds = await _dbContext.StudentInClass.Where(sic => sic.StudentId==studentId).Select(sic => sic.ClassSubjectId).ToListAsync();
                    return classSubjectIds;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Add Student To Class
        /// <summary>
        /// Adds a student to a class.
        /// </summary>
        /// <param name="studentInClassDTO">StudentInClassDTO object</param>
        /// <returns>Boolean indicating success</returns>
        public async Task<bool> AddStudentToClass(StudentInClass studentInClass)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    await dbContext.StudentInClass.AddAsync(studentInClass);
                    await dbContext.SaveChangesAsync();
                    }
                return true;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Add Multiple Students To Class
        /// <summary>
        /// Add multiple students to class
        /// </summary>
        /// <param name="studentsInClassDTO"></param>
        /// <returns>Boolean indicating success</returns>
        public async Task<bool> AddMultipleStudentsToClass(List<StudentInClass> studentsInClassDTO)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    List<StudentInClass> students = studentsInClassDTO.Select(dto =>
                    {
                        var studentInClass = new StudentInClass();
                        studentInClass.CopyProperties(dto);
                        return studentInClass;
                    }).ToList();
                    await dbContext.StudentInClass.AddRangeAsync(students);
                    await dbContext.SaveChangesAsync();
                    }
                return true;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Update Student in Class
        /// <summary>
        /// Updates student information in a class.
        /// </summary>
        /// <param name="studentInClass">studentInClass object</param>
        /// <returns>Boolean indicating success</returns>
        public async Task<bool> UpdateStudentInClass(StudentInClass studentInClass)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existingStudent = await dbContext.StudentInClass
                                 .FirstOrDefaultAsync(sic => sic.StudentClassId==studentInClass.StudentClassId);
                    if (studentInClass==null)
                        {
                        return false;
                        }
                    dbContext.Entry(studentInClass).State=EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                    }
                return true;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Delete Student from Class
        /// <summary>
        /// Deletes a student from a class by StudentClassId.
        /// </summary>
        /// <param name="studentClassId">ID of the student-class association</param>
        /// <returns>Boolean indicating success</returns>
        public async Task<bool> DeleteStudentFromClass(int studentClassId)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var studentInClass = await dbContext.StudentInClass.FirstOrDefaultAsync(sic => sic.StudentClassId==studentClassId);
                    if (studentInClass==null)
                        return false;
                    dbContext.StudentInClass.Remove(studentInClass);
                    await dbContext.SaveChangesAsync();
                    }
                return true;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Student in Specific Class
        /// <summary>
        /// Check if a student already exists in a specific class
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="classSubjectId"></param>
        /// <returns>StudentInClassDTO if exists, otherwise null</returns>
        public async Task<StudentInClass> GetStudentInClassByStudentIdAndClass(string studentId, int classSubjectId)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var studentInClass =await dbContext.StudentInClass.FirstOrDefaultAsync(s => s.StudentId==studentId&&s.ClassSubjectId==classSubjectId);
                    if (studentInClass==null)
                        {
                        return null;
                        }
                    return studentInClass;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Student Count By Class SubjectId
        public async Task<int> GetStudentCountByClassSubjectId(int classSubjectId)
            {
            using (var dbContext = new MyDbContext())
                {
                return await dbContext.StudentInClass.CountAsync(s => s.ClassSubjectId==classSubjectId);
                }
            }
        #endregion
        }
    }
