using ClassBusinessObject;
using ClassBusinessObject.ModelDTOs;
using ClassService.Services.StudentInClassServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.SubjectServices;

namespace ClassService.Controllers.StudentInClassController
    {
    [Route("api/[controller]")]
    [ApiController]
    public class StudentInClassController : ControllerBase
        {
        private readonly StudentInClassService _service;
        public StudentInClassController(StudentInClassService service)
            {
            _service=service;
            }

        [HttpGet]
        public async Task<IActionResult> GetAllStudentInClass()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetAllStudentInClass();
            return Ok(aPIResponse);
            }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAllStudentInClassByClassId(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetStudentInClassByClassId(id);
            return Ok(aPIResponse);
            }

        [HttpPost]
        public async Task<IActionResult> AddStudentToClass(StudentInClassDTO studentInClassDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.AddStudentToClass(studentInClassDTO);
            return Ok(aPIResponse);
            }

        [HttpPost("AddMutipleStudents")]
        public async Task<IActionResult> AddMutipleStudentToClass (List<StudentInClassDTO> studentInClassDTOs)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.AddMultipleStudentsToClass(studentInClassDTOs);
            return Ok(aPIResponse);
            }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveStudentToClass(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.DeleteStudentFromClass(id);
            return Ok(aPIResponse);
            }

        #region Get ClassSubjectIds by Student Id
        [HttpGet("ClassSubject/{id}")]
        public async Task<IActionResult> GetClassSubjectByStudentId(string id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetClassSubjectId(id);
            return Ok(aPIResponse);
            }
        #endregion

        #region Get Student Data in Class by ClassId
        [HttpGet("ClassId/{id}")]
        public async Task<IActionResult> GetStudentInClassByClassIdWithStudentData(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetStudentInClassByClassIdWithStudentData(id);
            return Ok(aPIResponse);
            }
        #endregion

        #region Get Student Data in Class by ClassId
        [HttpGet("AvailableStudent/{id}")]
        public async Task<IActionResult> getAvailableStudent(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetAvailableStudentsForClass(id);
            return Ok(aPIResponse);
            }
        #endregion

        #region Export Data
        [HttpGet("export")]
        public async Task<IActionResult> ExportSubjectsToExcel([FromQuery] int id)
            {
            APIResponse aPIResponse = new APIResponse();
            var export = await _service.ExportStudentInClassToExcel(id);
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
        }
    }

