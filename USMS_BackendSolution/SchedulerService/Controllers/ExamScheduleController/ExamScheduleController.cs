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
        public async Task<APIResponse> GetAllExamSchedules()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _examScheduleService.GetAllExamSchedules();
            return aPIResponse;
            }

        // GET: api/UnassignedTeacherExamSchedules
        [HttpGet("UnassignedTeacherExamSchedules")]
        public async Task<APIResponse> GetUnassignedTeacherExamSchedules()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _examScheduleService.GetUnassignedTeacherExamSchedules();
            return aPIResponse;
            }

        // GET: api/UnassignedRoomExamSchedules
        [HttpGet("UnassignedRoomExamSchedules")]
        public async Task<APIResponse> GetUnassignedRoomExamSchedules()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _examScheduleService.GetUnassignedRoomExamSchedules();
            return aPIResponse;
            }

        [HttpPost]
        public async Task<APIResponse> AddNewExamSchedule(ExamScheduleDTO examScheduleDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _examScheduleService.AddNewExamSchedule(examScheduleDTO);
            return aPIResponse;
            }

        [HttpPut]
        public async Task<APIResponse> UpdateExamSchedule(ExamScheduleDTO examScheduleDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _examScheduleService.UpdateExamSchedule(examScheduleDTO);
            return aPIResponse;
            }


        [HttpPut("AssignTeacher/{id}/{teacherId}")]
        public async Task<APIResponse> AssignTeacher(int id, string teacherId)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _examScheduleService.AssignTeacherToExamSchedule(id, teacherId);
            return aPIResponse;
            }

        [HttpPut("AssignRoom/{id}/{roomId}")]
        public async Task<APIResponse> AssignRoom(int id, string roomId)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _examScheduleService.AssignRoomToExamSchedule(id, roomId);
            return aPIResponse;
            }

        [HttpGet("AvailableTeachers/{date}/{startTime}/{endTime}")]
        public async Task<APIResponse> GetAvailableTeachers(DateOnly date, TimeOnly startTime, TimeOnly endTime)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _examScheduleService.GetAllTeacherAvailableForAddExamSchedule(date,startTime,endTime);
            return aPIResponse;
            }

        [HttpGet("Student/{id}")]
        public async Task<APIResponse> GetExamScheduleForStudent(string id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _examScheduleService.GetExamScheduleForStudent(id);
            return aPIResponse;
            }

        [HttpGet("Teacher/{id}")]
        public async Task<APIResponse> GetExamScheduleForTeacher(string id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _examScheduleService.GetExamScheduleForTeacher(id);
            return aPIResponse;
            }
        }
    }
