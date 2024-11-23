using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchedulerBusinessObject;
using SchedulerDataAccess.Services.SchedulerServices;

namespace SchedulerService.Controllers.Schedule
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
    {
        private readonly ScheduleService _scheduleService;
        public ScheduleController(ScheduleService scheduleService)
        {
            _scheduleService = scheduleService;
        }

        // GET: api/Customers
        [HttpGet]
        public APIResponse GetAllCustomers()
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _scheduleService.GetAllSchedule();
            return aPIResponse;
        }
    }
}
