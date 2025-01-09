using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;

namespace SchedulerService.Repository.ExamScheduleRepository
{
    public interface IExamScheduleRepository
    {
        public List<ExamScheduleDTO> GetAllExamSchedule();
        public List<ExamScheduleDTO> GetUnassignedTeacherExamSchedules();
        public List<ExamScheduleDTO> GetUnassignedRoomExamSchedules();
        public bool AddNewExamSchedule(ExamScheduleDTO examScheduleDTO);
        public List<Room> GetAvailableRooms(DateOnly date, TimeOnly startTime, TimeOnly endTime);
        public bool AssignTeacherToExamSchedule(int examScheduleId, string teacherId);
        public bool AssignRooomToExamSchedule(int examScheduleId, string roomId);
        public bool ChangeExamScheduleStatus(int id, int newStatus);
        public ExamScheduleDTO GetExamScheduleById(int id);
    }
}
