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
		public async Task<APIResponse> GetSchedules()
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse =await _scheduleService.GetAllSchedule();
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

        [HttpGet("Student/{id}/{startDay}/{endDay}")]
        public async Task<APIResponse> GetStudentSchedule(string id, DateTime startDay, DateTime endDay)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _scheduleService.GetClassScheduleForStudent(id, startDay, endDay);
            return aPIResponse;
            }

        [HttpGet("Teacher/{id}/{startDay}/{endDay}")]
        public async Task<APIResponse> GetTeacherSchedule(string id, DateTime startDay, DateTime endDay)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _scheduleService.GetClassSchedulesForTeacher(id, startDay, endDay);
            return aPIResponse;
            }

        [HttpGet("{majorId}/{classId}/{term}/{startDay}/{endDay}")]
        public async Task<APIResponse> GetClassSchedule(string majorId, string classId, int term, DateTime startDay, DateTime endDay)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _scheduleService.GetClassSchedulesForStaff(majorId,classId, term, startDay,endDay);
            return aPIResponse;
            }

        [HttpDelete("{id}")]
        public async Task<APIResponse> DeleteSchedule(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _scheduleService.DeleteSchedule(id);
            return aPIResponse;
            }

        [HttpGet("AvailableTeachers/{majorId}/{date}/{slot}")]
        public async Task<APIResponse> GetAvailableTeachersToAddSchedule(string majorId, DateOnly date, int slot)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _scheduleService.GetAllTeacherAvailableForAddSchedule(majorId,date,slot);
            return aPIResponse;
            }
        }
    }
