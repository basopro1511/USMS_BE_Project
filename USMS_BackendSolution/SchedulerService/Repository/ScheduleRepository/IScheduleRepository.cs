using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;

namespace Repositories.ScheduleRepository
{
	public interface IScheduleRepository
    {
		public Task<List<Schedule>> getAllSchedule();
        public Task<Schedule> GetScheduleById(int classScheduleId);
        public Task AddSchedule(Schedule schedule);
        public List<Schedule> GetSchedulesByDateAndSlot(DateOnly date, int slot);
        public Task<List<Schedule>> GetClassSchedulesByClassSubjectIds(List<int> classSubjectIds);
        public Task<List<Schedule>> GetSchedulesByClassSubjectId(int classSubjectId);
        public Task<bool> UpdateSchedule(Schedule scheduleDto);
        public Task<bool> DeleteScheduleById(int scheduleId);
        public Task<List<Schedule>> GetClassSchedulesForStaff(List<int> classSubjectIds,DateTime startDay, DateTime endDay);
        public Task<List<Schedule>> GetScheduleForTeacher(string teacherId, DateTime startDay, DateTime endDay);
        public Task<bool> ChangeScheduleStatus(List<int> classSubjectIds, int status);
            }
    }
