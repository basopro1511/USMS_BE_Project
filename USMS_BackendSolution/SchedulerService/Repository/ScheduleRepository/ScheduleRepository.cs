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
		public List<ScheduleDTO> getAllSchedule()
		{
			try
			{
				var dbContext = new MyDbContext();
				List<Schedule> schedules = dbContext.Schedule.ToList();
				List<ScheduleDTO> scheduleDTOs = new List<ScheduleDTO>();
				foreach (var schedule in schedules)
				{
					ScheduleDTO scheduleDTO = new ScheduleDTO();
					scheduleDTO.CopyProperties(schedule);
					scheduleDTO.ClassId = "SE1702"; // Tạo 1 method để get data những model này
					scheduleDTO.TeacherId = "Test";
					scheduleDTOs.Add(scheduleDTO);
				}
				return scheduleDTOs;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}

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
