using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using SchedulerBusinessObject.AppDBContext;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;

namespace SchedulerService.Repository.StudentInExamScheduleRepository
    {
    public class StudentInExamScheduleRepository : IStudentInExamScheduleRepository
        {

        #region Get All Student In Exam Schedule
        /// <summary>
        /// Get All Student In Exam Schedule Class
        /// </summary>
        /// <returns></returns>
        public async Task<List<StudentInExamScheduleDTO>> GetAllStudentInExamSchedule()
            {
            try
                {
                using (var _dbContext = new MyDbContext())
                    {
                    var students = await _dbContext.StudentInExamSchedule.ToListAsync();
                    List<StudentInExamScheduleDTO> studentInExamScheduleDTOs = new List<StudentInExamScheduleDTO>();
                    foreach (var student in students)
                        {
                        StudentInExamScheduleDTO studentInExamScheduleDTO = new StudentInExamScheduleDTO();
                        studentInExamScheduleDTO.CopyProperties(student);
                        studentInExamScheduleDTOs.Add(studentInExamScheduleDTO);
                        await _dbContext.SaveChangesAsync();
                        }
                    return studentInExamScheduleDTOs;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get All Student in Exam Schedule by ExamScheduleID
        /// <summary>
        ///  Get All Student In Exam Schedule by ExamScheduleID
        /// </summary>
        /// <param name="examScheduleId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<StudentInExamScheduleDTO>> GetAllStudentInExamScheduleByExamScheduleId(int examScheduleId)
            {
            try
                {
                using (var _dbContext = new MyDbContext())
                    {
                    var students = await _dbContext.StudentInExamSchedule.Where(s=> s.ExamScheduleId == examScheduleId).ToListAsync();
                    List<StudentInExamScheduleDTO> studentInExamScheduleDTOs = new List<StudentInExamScheduleDTO>();
                    foreach (var student in students)
                        {
                        StudentInExamScheduleDTO studentInExamScheduleDTO = new StudentInExamScheduleDTO();
                        studentInExamScheduleDTO.CopyProperties(student);
                        studentInExamScheduleDTOs.Add(studentInExamScheduleDTO);
                        await _dbContext.SaveChangesAsync();
                        }
                    return studentInExamScheduleDTOs;
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
        ///  Get Student in Exam Schedule by ExamScheduleID and StudentId
        /// </summary>
        /// <param name="examScheduleId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<StudentInExamScheduleDTO> GetStudentInExamScheduleByExamScheduleIdAndStudentId(int examScheduleId, string studentId)
            {
            try
                {
                using (var _dbContext = new MyDbContext())
                    {
                    var student = await _dbContext.StudentInExamSchedule.FirstOrDefaultAsync(s => s.ExamScheduleId==examScheduleId && s.StudentId == studentId);
                    StudentInExamScheduleDTO studentDto = new StudentInExamScheduleDTO();
                    studentDto.CopyProperties(student);
                    return studentDto;
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
        /// <param name="examScheduleDTO"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> AddNewStudentToExamSchedule(StudentInExamScheduleDTO examScheduleDTO)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    StudentInExamSchedule student = new StudentInExamSchedule();
                    student.CopyProperties(examScheduleDTO);
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
        /// <param name="studentId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> RemoveStudentInExamScheduleClass(int studentExamId)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    StudentInExamSchedule student=await dbContext.StudentInExamSchedule.FirstOrDefaultAsync(s => s.StudentExamId==studentExamId);
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

        #region Add Mutiple Student into Exam Schedule
        /// <summary>
        /// Add Mutiple Student into Exam Schedule
        /// </summary>
        /// <param name="studentInExamScheduleDTOs"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> AddMultipleStudentsToExamSchedule(List<StudentInExamScheduleDTO> studentInExamScheduleDTOs)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    List<StudentInExamSchedule> students = studentInExamScheduleDTOs.Select(dto =>
                    {
                        var studentInClass = new StudentInExamSchedule();
                        studentInClass.CopyProperties(dto);
                        return studentInClass;
                    }).ToList();
                    await dbContext.StudentInExamSchedule.AddRangeAsync(students);
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

        #region Count number of Student in Exam Schedule Class
        /// <summary>
        /// Count number of Student In Exam Schedule to get range of Class Exam
        /// </summary>
        /// <param name="examScheduleId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<int> CountStudentInExamSchedule(int examScheduleId)
            {
            try
                {
                int count = 0;
                using (var dbContext= new MyDbContext())
                    {
                     count = await dbContext.StudentInExamSchedule.CountAsync(s => s.ExamScheduleId==examScheduleId);
                    }
                return count;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Student in Specific Class
        /// <summary>
        /// Check if a student already exists in a specific class exam schedule
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="examScheduleId"></param>
        /// <returns>StudentInExamScheduleDTO if exists, otherwise null</returns>
        public StudentInExamScheduleDTO GetStudentInClassByStudentIdAndClass(string studentId, int examScheduleId)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var studentInClass = dbContext.StudentInExamSchedule.FirstOrDefault(s => s.StudentId==studentId&&s.ExamScheduleId==examScheduleId);
                    if (studentInClass==null)
                        {
                        return null;
                        }
                    StudentInExamScheduleDTO studentInClassDTO = new StudentInExamScheduleDTO();
                    studentInClassDTO.CopyProperties(studentInClass);
                    return studentInClassDTO;
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
