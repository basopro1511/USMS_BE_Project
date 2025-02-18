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
		List<Schedule> getAllSchedule();
		public ScheduleDTO GetScheduleById(int classScheduleId);
		Task AddSchedule(Schedule schedule);
		List<Schedule> GetSchedulesByDateAndSlot(DateOnly date, int slot);
		public List<ScheduleDTO> GetClassSchedulesByClassSubjectIds(List<int> classSubjectIds);
		public List<ScheduleDTO> GetClassSchedulesByClassSubjectId(int classSubjectId);
		public List<Schedule> GetSchedulesByClassSubjectId(int classSubjectId);
		public bool UpdateSchedule(ScheduleDTO scheduleDto);
		public bool DeleteScheduleById(int scheduleId);
		public TimeSlot? GetTimeSlotById(int slotId);
	}
}
