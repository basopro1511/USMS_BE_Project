using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;

namespace SchedulerService.Repository.ExamScheduleRepository
{
    public interface IExamScheduleRepository
    {
        public Task<List<ExamScheduleDTO>> GetAllExamSchedule();
        public Task<List<ExamScheduleDTO>> GetUnassignedTeacherExamSchedules();
        public Task<List<ExamScheduleDTO>> GetUnassignedRoomExamSchedules();
        public Task<bool> AddNewExamSchedule(ExamScheduleDTO examScheduleDTO);
        public Task<List<Room>> GetAvailableRooms(DateOnly date, TimeOnly startTime, TimeOnly endTime);
        public Task<bool> AssignTeacherToExamSchedule(int examScheduleId, string teacherId);
        public Task<bool> AssignRooomToExamSchedule(int examScheduleId, string roomId);
        public Task<bool> ChangeExamScheduleStatus(int id, int newStatus);
        public Task<ExamScheduleDTO> GetExamScheduleById(int id);
    }
}
