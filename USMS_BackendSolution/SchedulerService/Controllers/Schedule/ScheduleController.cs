using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
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
		public APIResponse GetSchedules()
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _scheduleService.GetAllSchedule();
			return aPIResponse;
		}

		[HttpPost]
		public async Task<APIResponse> PostSchedule(ClassScheduleDTO schedule)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = await _scheduleService.PostSchedule(schedule);
			return aPIResponse;
		}

        // GET: api/Customers
        [HttpGet("{id}")]
        public APIResponse GetStudentSchedule(string id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _scheduleService.GetClassSchedulesByStudentId(id);
            return aPIResponse;
            }
        }
}
