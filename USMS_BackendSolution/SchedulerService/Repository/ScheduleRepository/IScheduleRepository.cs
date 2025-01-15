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
        List<ScheduleDTO> getAllSchedule();
        Task AddSchedule(Schedule schedule);
        Task<List<Schedule>?> GetSchedulesByDateAndSlot(DateOnly date, int slot);
	}
}
