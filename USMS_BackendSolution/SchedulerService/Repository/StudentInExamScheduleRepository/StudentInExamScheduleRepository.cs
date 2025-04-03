using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using SchedulerBusinessObject.AppDBContext;
using SchedulerBusinessObject.SchedulerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulerService.Repository.StudentInExamScheduleRepository
    {
    public class StudentInExamScheduleRepository : IStudentInExamScheduleRepository
        {
        #region Get All Student In Exam Schedule
        /// <summary>
        /// Get All Student In Exam Schedule Class
        /// </summary>
        /// <param name="examScheduleId"></param>
        /// <returns></returns>
        public async Task<List<StudentInExamSchedule>> GetAllStudentInExamScheduleByExamScheduleId(int examScheduleId)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    return await dbContext.StudentInExamSchedule
                        .Where(s => s.ExamScheduleId==examScheduleId)
                        .ToListAsync();
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Student in Exam Schedule by ExamScheduleID and StudentId
        /// <summary>
        /// Get Student in Exam Schedule by ExamScheduleID and StudentId
        /// </summary>
        /// <param name="examScheduleId"></param>
        /// <param name="studentId"></param>
        /// <returns></returns>
        public async Task<StudentInExamSchedule> GetStudentInExamScheduleByExamScheduleIdAndStudentId(int examScheduleId, string studentId)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    return await dbContext.StudentInExamSchedule
                        .FirstOrDefaultAsync(s => s.ExamScheduleId==examScheduleId&&s.StudentId==studentId);
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Add Student into Exam Schedule Class
        /// <summary>
        /// Add new Student into Exam Schedule Class
        /// </summary>
        /// <param name="student"></param>
        /// <returns></returns>
        public async Task<bool> AddNewStudentToExamSchedule(StudentInExamSchedule student)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    await dbContext.StudentInExamSchedule.AddAsync(student);
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

        #region Remove Student in Exam Schedule Class
        /// <summary>
        /// Remove student from exam schedule class
        /// </summary>
        /// <param name="studentExamId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveStudentInExamScheduleClass(int studentExamId)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var student = await dbContext.StudentInExamSchedule
                        .FirstOrDefaultAsync(s => s.StudentExamId==studentExamId);
                    if (student==null)
                        return false;
                    dbContext.StudentInExamSchedule.Remove(student);
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

        #region Add Multiple Student into Exam Schedule
        /// <summary>
        /// Add Multiple Student into Exam Schedule
        /// </summary>
        /// <param name="students"></param>
        /// <returns></returns>
        public async Task<bool> AddMultipleStudentsToExamSchedule(List<StudentInExamSchedule> students)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    await dbContext.StudentInExamSchedule.AddRangeAsync(students);
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

        #region Count number of Student in Exam Schedule Class
        /// <summary>
        /// Count number of Student In Exam Schedule to get range of Class Exam
        /// </summary>
        /// <param name="examScheduleId"></param>
        /// <returns></returns>
        public async Task<int> CountStudentInExamSchedule(int examScheduleId)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    return await dbContext.StudentInExamSchedule
                        .CountAsync(s => s.ExamScheduleId==examScheduleId);
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion
        }
    }
