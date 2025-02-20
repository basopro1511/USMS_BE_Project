using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;

namespace Repositories.ScheduleRepository
{
	public interface IScheduleRepository
	{
		/// <summary>
		/// Method Test to get all Schedule
		/// </summary>
		/// <returns>a list of all schedule</returns>
		Task<List<Schedule>?> getAllSchedule();
		Task<ScheduleDTO?> GetScheduleById(int classScheduleId);
		Task AddSchedule(Schedule schedule);
		List<Schedule> GetSchedulesByDateAndSlot(DateOnly date, int slot);
		List<ScheduleDTO> GetClassSchedulesByClassSubjectIds(List<int> classSubjectIds);
		List<ScheduleDTO> GetClassSchedulesByClassSubjectId(int classSubjectId);
		List<Schedule> GetSchedulesByClassSubjectId(int classSubjectId);
		bool UpdateSchedule(ScheduleDTO scheduleDto);
		bool DeleteScheduleById(int scheduleId);
	}
}
