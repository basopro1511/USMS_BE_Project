using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerService.Services.StudentInExamScheduleServices;
using Services.RoomServices;

namespace SchedulerService.Controllers.StudentInExamScheduleController
    {
    [Route("api/[controller]")]
    [ApiController]
    public class StudentInExamScheduleController : ControllerBase
        {
        private readonly StudentInExamScheduleService _service;
        public StudentInExamScheduleController(StudentInExamScheduleService service)
            {
            _service=service;
            }

        [HttpGet("{id}")]
        public async Task<APIResponse> GetAllStudentInExamScheduleByExamScheduleId(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetAllStudentInExamScheduleByExamScheduleId(id);
            return aPIResponse;
            }

        [HttpPost]
        public async Task<APIResponse> AddStudentToExamSchedule(StudentInExamScheduleDTO studentInExamScheduleDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.AddNewStudentToExamSchedule(studentInExamScheduleDTO);
            return aPIResponse;
            }

        [HttpPost("AddMutipleStudents")]
        public async Task<APIResponse> AddMutipleStudentToExamSchedule(List<StudentInExamScheduleDTO> studentInExamScheduleDTOs)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.AddMultipleStudentsToExamSchedule(studentInExamScheduleDTOs);
            return aPIResponse;
            }

        [HttpDelete("{id}")]
        public async Task<APIResponse> RemoveStudentToClass(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.RemoveStudentInExamScheduleClass(id);
            return aPIResponse;
            }

        [HttpGet("AvailableStudents/{id}")]
        public async Task<APIResponse> AvailableStudent(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetAvailableStudentsForExamSchedule(id);
            return aPIResponse;
            }

        }
    }
