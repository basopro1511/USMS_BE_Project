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
        public Task<List<Schedule>> GetClassSchedulesForStaffByWeek(List<int> classSubjectIds,DateTime startDay, DateTime endDay);
        public Task<List<Schedule>> GetClassSchedulesForStaffByDay(List<int> classSubjectIds, DateTime day);
        public Task<List<Schedule>> GetScheduleForTeacher(string teacherId, DateTime startDay, DateTime endDay);
        public Task<bool> ChangeScheduleStatus(List<int> classSubjectIds, int status);
        public Task<List<int>> GetClassSubjcetIdByTeacherSchedule(string teacherId);
        public Task<List<int>> GetSlotNoInSubjectByClassSubjectId(int classSubjectId);
        public Task<List<Schedule>> GetScheduleDataByScheduleIdandSlotInSubject(int classSubjectId, int slotInSubject);

            }
    }
