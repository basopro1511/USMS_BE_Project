using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;

namespace Repositories.ScheduleRepository
{
	public interface IScheduleRepository
    {
		public Task<List<Schedule>> getAllSchedule();
        public Task<ScheduleDTO> GetScheduleById(int classScheduleId);
        public Task AddSchedule(Schedule schedule);
        public List<Schedule> GetSchedulesByDateAndSlot(DateOnly date, int slot);
        public Task<List<ViewScheduleDTO>> GetClassSchedulesByClassSubjectIds(List<int> classSubjectIds);
        public Task<List<Schedule>> GetSchedulesByClassSubjectId(int classSubjectId);
        public Task<bool> UpdateSchedule(ScheduleDTO scheduleDto);
        public Task<bool> DeleteScheduleById(int scheduleId);
        public Task<List<ViewScheduleDTO>> GetClassSchedulesForStaff(List<int> classSubjectIds,DateTime startDay, DateTime endDay);
        public Task<List<ViewScheduleDTO>> GetScheduleForTeacher(string teacherId, DateTime startDay, DateTime endDay);
            }
        }
