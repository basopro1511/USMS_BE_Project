using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;

namespace SchedulerService.Repository.StudentInExamScheduleRepository
    {
    public interface IStudentInExamScheduleRepository
        {
        #region Get All Student In Exam Schedule
        /// <summary>
        /// Get All Student In Exam Schedule Class
        /// </summary>
        /// <returns></returns>
        public Task<List<StudentInExamScheduleDTO>> GetAllStudentInExamSchedule();
        #endregion

        #region Get All Student in Exam Schedule by ExamScheduleID
        /// <summary>
        ///  Get All Student In Exam Schedule by ExamScheduleID
        /// </summary>
        /// <param name="examScheduleId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<List<StudentInExamScheduleDTO>> GetAllStudentInExamScheduleByExamScheduleId(int examScheduleId);
        #endregion

        #region Get Student in Exam Schedule by ExamScheduleID and StudentId
        /// <summary>
        ///  Get Student in Exam Schedule by ExamScheduleID and StudentId
        /// </summary>
        /// <param name="examScheduleId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<StudentInExamScheduleDTO> GetStudentInExamScheduleByExamScheduleIdAndStudentId(int examScheduleId, string studentId);
        #endregion

        #region Add Student into Exam Schedule Class
        /// <summary>
        /// Add new Student into Exam Schedule Class
        /// </summary>
        /// <param name="examScheduleDTO"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<bool> AddNewStudentToExamSchedule(StudentInExamScheduleDTO examScheduleDTO);
        #endregion

        #region Remove Student in Exam Schedule Class
        /// <summary>
        /// Remove student from exam schedule class
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<bool> RemoveStudentInExamScheduleClass(int studentExamId);
        #endregion

        #region Add Mutiple Student into Exam Schedule
        /// <summary>
        /// Add Mutiple Student into Exam Schedule
        /// </summary>
        /// <param name="studentInExamScheduleDTOs"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<bool> AddMultipleStudentsToExamSchedule(List<StudentInExamScheduleDTO> studentInExamScheduleDTOs);
        #endregion

        #region Count number of Student in Exam Schedule Class
        /// <summary>
        /// Count number of Student In Exam Schedule to get range of Class Exam
        /// </summary>
        /// <param name="examScheduleId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Task<int> CountStudentInExamSchedule(int examScheduleId);
        #endregion
        }
    }
