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
			aPIResponse = await _scheduleService.AddNewSchedule(schedule);
			return aPIResponse;
		}

		[HttpPut]
		public async Task<APIResponse> UpdateSchedule(ScheduleDTO schedule)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = await _scheduleService.UpdateSchedule(schedule);
			return aPIResponse;
		}

		// GET: api/Customers
		[HttpGet("{id}")]
		public APIResponse GetStudentSchedule(string id)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _scheduleService.GetClassSchedulesByStudentIds(id);
			return aPIResponse;
		}

		// GET: api/Customers
		[HttpGet("{majorId}/{classId}/{term}/{startDay}/{endDay}")]
		public APIResponse GetClassSchedule(string majorId, string classId, int term, DateTime startDay, DateTime endDay)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _scheduleService.GetClassSchedulesForClass(majorId, classId, term, startDay, endDay);
			return aPIResponse;
		}

		[HttpDelete("{id}")]
		public APIResponse DeleteSchedule(int id)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _scheduleService.DeleteSchedule(id);
			return aPIResponse;
		}

		[HttpGet("ScheduleDetails/{id}")]
		public async Task<APIResponse> GetScheduleDetails(int id)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = await _scheduleService.GetScheduleById(id);
			return aPIResponse;
		}
	}
}
