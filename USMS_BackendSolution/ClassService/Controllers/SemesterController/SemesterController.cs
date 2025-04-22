using ClassBusinessObject;
using ClassBusinessObject.ModelDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.ClassServices;
using SchedulerBusinessObject.ModelDTOs;
using Services.SemesterServices;
using Services.SubjectServices;

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

        #region Export Data
        [HttpGet("export")]
        public async Task<IActionResult> ExportSemestersToExcel(int? status)
            {
            APIResponse aPIResponse = new APIResponse();
            var export = await _semesterService.ExportSemestersToExcel(status);
            if (export==null)
                {
                aPIResponse.Message="Không có dữ liệu để xuất.";
                return BadRequest(aPIResponse);
                }
            aPIResponse.Result="File đã được tạo và sẵn sàng để tải về.";
            aPIResponse.Message="Export Thành công";
            // Trả về tệp Excel trực tiếp
            var fileBytes = export as byte[];
            if (fileBytes==null)
                {
                aPIResponse.Message="Đã xảy ra lỗi khi tạo tệp Excel.";
                return StatusCode(500, aPIResponse);
                }
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachSinhVien.xlsx");
            }
        #endregion

        #region Export empty form
        [HttpGet("exportEmpty")]
        public async Task<IActionResult> ExportFormAddSemester()
            {
            APIResponse aPIResponse = new APIResponse();
            var export = await _semesterService.ExportFormAddSemester();
            if (export==null)
                {
                aPIResponse.Message="Không có dữ liệu để xuất.";
                return BadRequest(aPIResponse);
                }
            aPIResponse.Result="File đã được tạo và sẵn sàng để tải về.";
            aPIResponse.Message="Export Thành công";
            // Trả về tệp Excel trực tiếp
            var fileBytes = export as byte[];
            if (fileBytes==null)
                {
                aPIResponse.Message="Đã xảy ra lỗi khi tạo tệp Excel.";
                return StatusCode(500, aPIResponse);
                }
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachSinhVien.xlsx");
            }
        #endregion

        #region Import From Excel
        [HttpPost("import")]
        public async Task<IActionResult> ImportSemesters(IFormFile file)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _semesterService.ImportSemestersFromExcel(file);
            return Ok(aPIResponse);
            }
        #endregion

        #region Change Selected Status
        [HttpPut("ChangeSelectStatus")]
        public async Task<IActionResult> ChangeStatusClassSubject(List<string> ids, int status)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _semesterService.ChangeSemesterStatusSelected(ids, status);
            return Ok(aPIResponse);
            }
        #endregion
        }
    }
