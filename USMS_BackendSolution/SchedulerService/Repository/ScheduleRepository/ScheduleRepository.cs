using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using SchedulerBusinessObject.AppDBContext;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;

namespace Repositories.ScheduleRepository
{
	public class ScheduleRepository : IScheduleRepository
	{
		/// <summary>
		/// Method Test To get all Schedule
		/// </summary>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public List<Schedule> getAllSchedule()
		{
			try
			{
				var dbContext = new MyDbContext();
				List<Schedule> schedules = dbContext.Schedule.ToList();
				return schedules;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Add Schedule
		/// </summary>
		/// <param name="schedule"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public async Task AddSchedule(Schedule schedule)
		{
			try
			{
				using (var dbContext = new MyDbContext())
				{
					await dbContext.Schedule.AddAsync(schedule);
					dbContext.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

		/// <summary>
		/// Get Schedules By Date And Slot
		/// </summary>
		/// <param name="date"></param>
		/// <param name="slotId"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public async Task<List<Schedule>?> GetSchedulesByDateAndSlot(DateOnly date, int slotId)
		{
			try
			{
				using (var dbContext = new MyDbContext())
				{
					return await dbContext.Schedule.Where(s => s.Status == 1 &&
																s.Date == date &&
																s.SlotId == slotId).ToListAsync();
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
	}
}
