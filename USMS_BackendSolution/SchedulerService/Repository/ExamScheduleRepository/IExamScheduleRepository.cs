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
    }
}
