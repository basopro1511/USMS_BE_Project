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
        public async Task<IActionResult> GetAllStudentInExamScheduleByExamScheduleId(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetAllStudentInExamScheduleByExamScheduleId(id);
            return Ok(aPIResponse);
            }

        [HttpPost]
        public async Task<IActionResult> AddStudentToExamSchedule(StudentInExamScheduleDTO studentInExamScheduleDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.AddNewStudentToExamSchedule(studentInExamScheduleDTO);
            return Ok(aPIResponse);
            }

        [HttpPost("AddMutipleStudents")]
        public async Task<IActionResult> AddMutipleStudentToExamSchedule(List<StudentInExamScheduleDTO> studentInExamScheduleDTOs)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.AddMultipleStudentsToExamSchedule(studentInExamScheduleDTOs);
            return Ok(aPIResponse);
            }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveStudentToClass(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.RemoveStudentInExamScheduleClass(id);
            return Ok(aPIResponse);
            }

        [HttpGet("AvailableStudents/{id}")]
        public async Task<IActionResult> AvailableStudent(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetAvailableStudentsForExamSchedule(id);
            return Ok(aPIResponse);
            }

        }
    }
