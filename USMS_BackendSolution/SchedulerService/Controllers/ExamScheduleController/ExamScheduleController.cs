using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerService.Services.ExamScheduleServices;
using Services.RoomServices;

namespace SchedulerService.Controllers.ExamScheduleController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamScheduleController : ControllerBase
    {
        private readonly ExamScheduleService _examScheduleService;
        public ExamScheduleController(ExamScheduleService examScheduleService)
        {
            _examScheduleService = examScheduleService;
        }

        // GET: api/ExamSchedules
        [HttpGet]
        public APIResponse GetAllExamSchedules()
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _examScheduleService.GetAllExamSchedules();
            return aPIResponse;
        }

        // GET: api/UnassignedTeacherExamSchedules
        [HttpGet("UnassignedTeacherExamSchedules")]
        public APIResponse GetUnassignedTeacherExamSchedules()
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _examScheduleService.GetUnassignedTeacherExamSchedules();
            return aPIResponse;
        }

        // GET: api/UnassignedRoomExamSchedules
        [HttpGet("UnassignedRoomExamSchedules")]
        public APIResponse GetUnassignedRoomExamSchedules()
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _examScheduleService.GetUnassignedRoomExamSchedules();
            return aPIResponse;
        }

        // GET: api/UnassignedRoomExamSchedules
        [HttpGet("AvailableRooms")]
        public APIResponse AvailableRooms(DateOnly date, TimeOnly startTime, TimeOnly endTime)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _examScheduleService.GetAvailableRooms(date, startTime,endTime);
            return aPIResponse;
        }
        //POST: api/Rooms
        [HttpPost]
        public APIResponse AddNewExamSchedule(ExamScheduleDTO examScheduleDTO)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _examScheduleService.AddNewExamSchedule(examScheduleDTO);
            return aPIResponse;
        }

        [HttpPut("AssignTeacher/{id}/{teacherId}")]
        public APIResponse AssignTeacher(int id, string teacherId)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _examScheduleService.AssignTeacherToExamSchedule(id,teacherId);
            return aPIResponse;
        }

        [HttpPut("AssignRoom/{id}/{roomId}")]
        public APIResponse AssignRoom(int id, string roomId)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _examScheduleService.AssignRoomToExamSchedule(id, roomId);
            return aPIResponse;
        }
    }
}
