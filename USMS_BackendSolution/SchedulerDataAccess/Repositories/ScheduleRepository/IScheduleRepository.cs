using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerDataAccess.Repositories.ScheduleRepository
{
    public interface IScheduleRepository
    {    
      /// <summary>
      /// Method Test to get all Schedule
      /// </summary>
      /// <returns>a list of all schedule</returns>
        public List<ScheduleDTO> getAllSchedule();
    }
}
