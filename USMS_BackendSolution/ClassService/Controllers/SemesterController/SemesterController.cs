using ClassBusinessObject;
using ClassBusinessObject.ModelDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.ClassServices;
using SchedulerBusinessObject.ModelDTOs;
using Services.SemesterServices;

namespace SchedulerService.Controllers.SemesterController
    {
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterController : ControllerBase
        {
        private readonly SemesterService _semesterService;

        public SemesterController(SemesterService semesterService)
            {
            _semesterService=semesterService;
            }
        // GET: api/Semester
        [HttpGet]
        public async Task<IActionResult> GetAllSemesters()
            {
            APIResponse response = new APIResponse();
            response=await _semesterService.GetAllSemesters();
            return Ok(response);
            }
        // GET: api/Semester/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSemesterById(string id)
            {
            APIResponse response = new APIResponse();
            response=await _semesterService.GetSemesterById(id);
            return Ok(response);
            }
        // POST: api/Semester
        [HttpPost]
        public async Task<IActionResult> AddNewSemester(SemesterDTO semesterDTO)
            {
            APIResponse response = new APIResponse();
            response=await _semesterService.AddSemester(semesterDTO);
            return Ok(response);
            }
        // PUT: api/ClassSubject
        [HttpPut]
        public async Task<IActionResult> UpdateSemester(SemesterDTO semesterDTO)
            {
            APIResponse response = new APIResponse();
            response=await _semesterService.UpdateSemester(semesterDTO);
            return Ok(response);
            }
        // Get: api/Semester/ChangeStatus/{id}
        [HttpGet("ChangeStatus/{id}/{status}")]
        public async Task<IActionResult> ChangeStatusSemester(string id, int status)
            {
            APIResponse response = new APIResponse();
            response=await _semesterService.ChangeStatusSemester(id, status);
            return Ok(response);
            }
        }
    }
