using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.Identity.Client;
using SchedulerBusinessObject.AppDBContext;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerDataAccess.Repositories.ScheduleRepository
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
                List<Schedules> schedules = dbContext.Schedules.ToList();
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
    }
}
