using SchedulerBusinessObject;
using SchedulerBusinessObject.SchedulerModels;
using SchedulerDataAccess.Repositories.ScheduleRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerDataAccess.Services.SchedulerServices
{
    public class ScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;
        public ScheduleService()
        {
            _scheduleRepository = new ScheduleRepository();
        }
        #region Get All Schedule
        public APIResponse GetAllSchedule()
        {
            APIResponse aPIResponse = new APIResponse();
            var schedule = _scheduleRepository.getAllSchedule();
            if (schedule == null)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Don't have any schedule available!";
            }
            aPIResponse.Result = schedule;
            return aPIResponse;
        }
        #endregion
    }
}
