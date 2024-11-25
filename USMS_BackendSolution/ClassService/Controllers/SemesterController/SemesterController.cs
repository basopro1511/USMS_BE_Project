using ClassBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<APIResponse> GetAllSemesters()
        {
            return await _semesterService.GetAllSemestersAsync();
        }
        // GET: api/Semester/{id}
        [HttpGet("{id}")]
        public async Task<APIResponse> GetSemesterById(string id)
        {
            return await _semesterService.GetSemesterByIdAsync(id);
        }
        // GET: api/Semester/Active
        [HttpGet("Active")]
        public async Task<APIResponse> GetActiveSemesters()
        {
            return await _semesterService.GetActiveSemestersAsync();
        }
        // POST: api/Semester
        [HttpPost]
        public async Task<APIResponse> AddNewSemester([FromBody] SemesterDTO semesterDTO)
        {
            return await _semesterService.AddSemesterAsync(semesterDTO);
        }
        // PUT: api/Semester
        // PUT: api/Semester/ChangeStatus/{id}
        [HttpPut("ChangeStatus/{id}")]
        public async Task<APIResponse> ChangeStatusSemester(string id)
        {
            var response = await _semesterService.GetSemesterByIdAsync(id);
            if (!response.IsSuccess || response.Result == null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Semester not found."
                };
            }
            var semester = response.Result as SemesterDTO;
            if (semester == null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Failed to parse semester data."
                };
            }
            semester.Status = !semester.Status;
            return await _semesterService.UpdateSemesterAsync(semester);
        }
        // DELETE: api/Semester/{id}
        [HttpDelete("{id}")]
        public async Task<APIResponse> DeleteSemester(string id)
        {
            return await _semesterService.DeleteSemesterAsync(id);
        }
    }
}
