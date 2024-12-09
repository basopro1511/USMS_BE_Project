using ClassBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.ClassServices;
using ClassBusinessObject.ModelDTOs;
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
            _semesterService = semesterService;
        }
        // GET: api/Semester
        [HttpGet]
        public APIResponse GetAllSemesters()
        {
            APIResponse response = new APIResponse();
            response = _semesterService.GetAllSemesters();
            return response;
        }
        // GET: api/Semester/{id}
        [HttpGet("{id}")]
        public APIResponse GetSemesterById(string id)
        {
            APIResponse response = new APIResponse();
            response = _semesterService.GetSemesterById(id);
            return response;
        }
        // POST: api/Semester
        [HttpPost]
        public APIResponse AddNewSemester(SemesterDTO semesterDTO)
        {
            APIResponse response = new APIResponse();
            response = _semesterService.AddSemester(semesterDTO);
            return response;
        }
        // PUT: api/ClassSubject
        [HttpPut]
        public APIResponse UpdateSemester(SemesterDTO semesterDTO)
        {
            APIResponse response = new APIResponse();
            response = _semesterService.UpdateSemester(semesterDTO);
            return response;
        }
        // PUT: api/Semester/ChangeStatus/{id}
        [HttpPut("ChangeStatus/{id}")]
        public APIResponse ChangeStatusSemester(string id)
        {
            APIResponse response = new APIResponse();
            response = _semesterService.GetSemesterById(id);
            return response;
        }

    }
}
