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
		public async Task<IActionResult> GetSchedules()
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse =await _scheduleService.GetAllSchedule();
			return Ok(aPIResponse);
		}

		[HttpPost]
		public async Task<IActionResult> PostSchedule(ClassScheduleDTO schedule)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = await _scheduleService.AddNewSchedule(schedule);
			return Ok(aPIResponse);
		}

        [HttpPut]
        public async Task<IActionResult> UpdateSchedule(ScheduleDTO schedule)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = await _scheduleService.UpdateSchedule(schedule);
            return Ok(aPIResponse);
            }

        [HttpGet("Student/{id}/{startDay}/{endDay}")]
        public async Task<IActionResult> GetStudentSchedule(string id, DateTime startDay, DateTime endDay)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _scheduleService.GetClassScheduleForStudent(id, startDay, endDay);
            return Ok(aPIResponse);
            }

        [HttpGet("Teacher/{id}/{startDay}/{endDay}")]
        public async Task<IActionResult> GetTeacherSchedule(string id, DateTime startDay, DateTime endDay)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _scheduleService.GetClassSchedulesForTeacher(id, startDay, endDay);
            return Ok(aPIResponse);
            }

        [HttpGet("{majorId}/{classId}/{term}/{startDay}/{endDay}")]
        public async Task<IActionResult> GetClassSchedule(string majorId, string classId, int term, DateTime startDay, DateTime endDay)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _scheduleService.GetClassSchedulesForStaff(majorId,classId, term, startDay,endDay);
            return Ok(aPIResponse);
            }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _scheduleService.DeleteSchedule(id);
            return Ok(aPIResponse);
            }

        [HttpGet("AvailableTeachers/{majorId}/{date}/{slot}")]
        public async Task<IActionResult> GetAvailableTeachersToAddSchedule(string majorId, DateOnly date, int slot)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _scheduleService.GetAllTeacherAvailableForAddSchedule(majorId,date,slot);
            return Ok(aPIResponse);
            }
        }
    }
