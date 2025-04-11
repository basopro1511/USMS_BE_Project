using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;
using SchedulerDataAccess.Services.SchedulerServices;
using Services.RoomServices;

namespace SchedulerService.Controllers.Schedule
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : ControllerBase
        {
        private readonly ScheduleService _scheduleService;
        public ScheduleController(ScheduleService scheduleService)
            {
            _scheduleService=scheduleService;
            }

        // GET: api/Customers
        [HttpGet]
        public async Task<IActionResult> GetSchedules()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _scheduleService.GetAllSchedule();
            return Ok(aPIResponse);
            }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSchedule(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _scheduleService.GetScheduleById(id);
            return Ok(aPIResponse);
            }
        [HttpPost]
        public async Task<IActionResult> PostSchedule(ClassScheduleDTO schedule)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _scheduleService.AddNewSchedule(schedule);
            return Ok(aPIResponse);
            }

        [HttpPut]
        public async Task<IActionResult> UpdateSchedule(ScheduleDTO schedule)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _scheduleService.UpdateSchedule(schedule);
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
            aPIResponse=await _scheduleService.GetClassSchedulesForStaff(majorId, classId, term, startDay, endDay);
            return Ok(aPIResponse);
            }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _scheduleService.DeleteSchedule(id);
            return Ok(aPIResponse);
            }

        [HttpGet("AvailableTeachers/{majorId}/{date}/{slot}")]
        public async Task<IActionResult> GetAvailableTeachersToAddSchedule(string majorId, DateOnly date, int slot)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _scheduleService.GetAllTeacherAvailableForAddSchedule(majorId, date, slot);
            return Ok(aPIResponse);
            }
        [HttpPut("ChangeStatus/{majorId}/{classId}/{term}/{status}")]
        public async Task<IActionResult> ChangeStatusSchedule(string majorId, string classId, int term, int status)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _scheduleService.ChangeScheduleStatus(majorId, classId, term, status);
            return Ok(aPIResponse);
            }
        /// <summary>
        /// Endpoint tự động sắp lịch học cho các lớp theo khoảng thời gian và ngày học được chọn.
        /// Ví dụ, với input: từ 2025-05-01 đến 2025-05-31, chỉ sắp lịch vào thứ Hai và thứ Tư.
        /// </summary>
        /// <param name="input">Thông tin input: ngày bắt đầu, kết thúc, các ngày trong tuần cho phép sắp lịch</param>
        /// <returns>APIResponse với kết quả sắp lịch</returns>
        [HttpPost("autoSchedule")]
        public async Task<IActionResult> AutoSchedule([FromQuery] string majorId, [FromQuery] string classId, [FromQuery] string semesterId, [FromQuery] int term, [FromBody] List<DayOfWeek> scheduledDays)
            {
            var result = await _scheduleService.AutoScheduleClasses(majorId, classId, semesterId, term, scheduledDays);
            if (result.IsSuccess)
                return Ok(result);
            return BadRequest(result);
            }
        }
 }
