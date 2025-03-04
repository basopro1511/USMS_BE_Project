using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;

namespace Repositories.ScheduleRepository
{
	public interface IScheduleRepository
    {
		List<Schedule> getAllSchedule();
        public ScheduleDTO GetScheduleById(int classScheduleId);
        Task AddSchedule(Schedule schedule);
        List<Schedule> GetSchedulesByDateAndSlot(DateOnly date, int slot);
        public List<ViewScheduleDTO> GetClassSchedulesByClassSubjectIds(List<int> classSubjectIds);
        public List<Schedule> GetSchedulesByClassSubjectId(int classSubjectId);
        public bool UpdateSchedule(ScheduleDTO scheduleDto);
        public bool DeleteScheduleById(int scheduleId);
        public List<ViewScheduleDTO> GetClassSchedulesForStaff(List<int> classSubjectIds,DateTime startDay, DateTime endDay);
        public List<ViewScheduleDTO> GetScheduleForTeacher(string teacherId, DateTime startDay, DateTime endDay);
            }
        }
