using Microsoft.AspNetCore.Mvc;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerDataAccess.Services.SemesterServices;

namespace SchedulerService.Controllers.Semester
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
        public async Task<IActionResult> GetAllSemesters()
        {
            var result = await _semesterService.GetAllSemestersAsync();
            return Ok(result);
        }

        // GET: api/Semester/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSemesterById(string id)
        {
            var result = await _semesterService.GetSemesterByIdAsync(id);
            if (result == null)
                return NotFound("Semester not found.");
            return Ok(result);
        }

        // POST: api/Semester
        [HttpPost]
        public async Task<IActionResult> CreateSemester([FromBody] SemesterDTO semesterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _semesterService.AddSemesterAsync(semesterDto);
            return CreatedAtAction(nameof(GetSemesterById), new { id = semesterDto.SemesterId }, semesterDto);
        }

        // PUT: api/Semester/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSemester(string id, [FromBody] SemesterDTO semesterDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != semesterDto.SemesterId)
                return BadRequest("Semester ID mismatch.");

            var existingSemester = await _semesterService.GetSemesterByIdAsync(id);
            if (existingSemester == null)
                return NotFound("Semester not found.");

            await _semesterService.UpdateSemesterAsync(semesterDto);
            return NoContent();
        }

        // DELETE: api/Semester/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSemester(string id)
        {
            var existingSemester = await _semesterService.GetSemesterByIdAsync(id);
            if (existingSemester == null)
                return NotFound("Semester not found.");

            await _semesterService.DeleteSemesterAsync(id);
            return NoContent();
        }
    }
}
