using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;

namespace SchedulerService.Repository.ExamScheduleRepository
{
    public interface IExamScheduleRepository
    {
        public Task<List<ExamSchedule>> GetAllExamSchedule();
        public Task<List<ExamSchedule>> GetUnassignedTeacherExamSchedules();
        public Task<List<ExamSchedule>> GetUnassignedRoomExamSchedules();
        public Task<bool> AddNewExamSchedule(ExamSchedule examScheduleDTO);
        public Task<bool> AssignTeacherToExamSchedule(int examScheduleId, string teacherId);
        public Task<bool> AssignRooomToExamSchedule(int examScheduleId, string roomId);
        public Task<bool> ChangeExamScheduleStatus(int id, int newStatus);
        public Task<ExamSchedule> GetExamScheduleById(int id);
        public Task<List<ExamSchedule>> GetTeacherInExamSchedule(DateOnly date, TimeOnly startTime, TimeOnly endTime);
        public Task<bool> UpdateExamSchedule(ExamSchedule examScheduleDTO);
        public Task<List<ExamSchedule>> GetExamScheduleForStudent(string studentId);
        public Task<List<ExamSchedule>> GetExamScheduleForTeacher(string teacherId);
        public Task<int> CountStudentInExamSchedule(int examScheduleId);
        public Task<bool> ChangeExamScheduleStatusSelected(List<int> examIds, int status);
        }
    }
