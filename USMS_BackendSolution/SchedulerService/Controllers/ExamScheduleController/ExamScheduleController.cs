using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerService.Services.ExamScheduleServices;
using Services.RoomServices;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SchedulerService.Controllers.ExamScheduleController
    {
    [Route("api/[controller]")]
    [ApiController]
    public class ExamScheduleController : ControllerBase
        {
        private readonly ExamScheduleService _examScheduleService;
        public ExamScheduleController(ExamScheduleService examScheduleService)
            {
            _examScheduleService=examScheduleService;
            }

        // GET: api/ExamSchedules
        [HttpGet]
        public async Task<IActionResult> GetAllExamSchedules()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _examScheduleService.GetAllExamSchedules();
            return Ok(aPIResponse);
            }

        // GET: api/UnassignedTeacherExamSchedules
        [HttpGet("UnassignedTeacherExamSchedules")]
        public async Task<IActionResult> GetUnassignedTeacherExamSchedules()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _examScheduleService.GetUnassignedTeacherExamSchedules();
            return Ok(aPIResponse);
            }

        // GET: api/UnassignedRoomExamSchedules
        [HttpGet("UnassignedRoomExamSchedules")]
        public async Task<IActionResult> GetUnassignedRoomExamSchedules()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _examScheduleService.GetUnassignedRoomExamSchedules();
            return Ok(aPIResponse);
            }

        [HttpPost]
        public async Task<IActionResult> AddNewExamSchedule(ExamScheduleDTO examScheduleDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _examScheduleService.AddNewExamSchedule(examScheduleDTO);
            return Ok(aPIResponse);
            }

        [HttpPut]
        public async Task<IActionResult> UpdateExamSchedule(ExamScheduleDTO examScheduleDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _examScheduleService.UpdateExamSchedule(examScheduleDTO);
            return Ok(aPIResponse);
            }


        [HttpPut("AssignTeacher/{id}/{teacherId}")]
        public async Task<IActionResult> AssignTeacher(int id, string teacherId)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _examScheduleService.AssignTeacherToExamSchedule(id, teacherId);
            return Ok(aPIResponse);
            }

        [HttpPut("AssignRoom/{id}/{roomId}")]
        public async Task<IActionResult> AssignRoom(int id, string roomId)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _examScheduleService.AssignRoomToExamSchedule(id, roomId);
            return Ok(aPIResponse);
            }

        [HttpGet("AvailableTeachers/{date}/{startTime}/{endTime}")]
        public async Task<IActionResult> GetAvailableTeachers(DateOnly date, TimeOnly startTime, TimeOnly endTime)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _examScheduleService.GetAllTeacherAvailableForAddExamSchedule(date,startTime,endTime);
            return Ok(aPIResponse);
            }

        [HttpGet("Student/{id}")]
        public async Task<IActionResult> GetExamScheduleForStudent(string id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _examScheduleService.GetExamScheduleForStudent(id);
            return Ok(aPIResponse);
            }

        [HttpGet("Teacher/{id}")]
        public async Task<IActionResult> GetExamScheduleForTeacher(string id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _examScheduleService.GetExamScheduleForTeacher(id);
            return Ok(aPIResponse);
            }

        [HttpPut("ChangeSelectStatus")]
        public async Task<IActionResult> ChangeStatusClassSubject(List<int> ids, int status)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _examScheduleService.ChangeExamScheduleStatusSelected(ids, status);
            return Ok(aPIResponse);
            }
        }
    }
