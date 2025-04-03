using SchedulerBusinessObject.SchedulerModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchedulerService.Repository.StudentInExamScheduleRepository
    {
    public interface IStudentInExamScheduleRepository
        {
        Task<List<StudentInExamSchedule>> GetAllStudentInExamScheduleByExamScheduleId(int examScheduleId);
        Task<StudentInExamSchedule> GetStudentInExamScheduleByExamScheduleIdAndStudentId(int examScheduleId, string studentId);
        Task<bool> AddNewStudentToExamSchedule(StudentInExamSchedule student);
        Task<bool> RemoveStudentInExamScheduleClass(int studentExamId);
        Task<bool> AddMultipleStudentsToExamSchedule(List<StudentInExamSchedule> students);
        Task<int> CountStudentInExamSchedule(int examScheduleId);
        }
    }
