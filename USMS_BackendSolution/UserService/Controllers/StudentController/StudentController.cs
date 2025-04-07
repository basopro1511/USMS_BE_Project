using Azure;
using BusinessObject;
using BusinessObject.ModelDTOs;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;
using OfficeOpenXml;
using UserService.Services.StudentServices;

namespace UserService.Controllers.StudentController
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentService _service;
        public StudentController(StudentService studentService)
        {
            _service= studentService;
        }

        #region Get All Student
        [HttpGet]
        public async Task<IActionResult> GetAllStudent()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetAllStudent();
            return Ok(aPIResponse);
            }
        #endregion

        #region Get Student By Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(string id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetUserById(id);
            return Ok(aPIResponse);
            }
        #endregion

        #region Add New Student
        [HttpPost]
        public async Task<IActionResult> AddNewStudent(UserDTO userDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.AddNewStudent(userDTO);
            return Ok(aPIResponse);
            }
        #endregion

        #region Update Student
        [HttpPut]
        public async Task<IActionResult> UpdateStudent(UserDTO userDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.UpdateStudent(userDTO);
            return Ok(aPIResponse);
            }
        #endregion

        #region Import From Excel
        [HttpPost("import")]
        public async Task<IActionResult> ImportStudents(IFormFile file)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.ImportStudentsFromExcel(file);
            return Ok(aPIResponse);
            }
        #endregion

        #region Update Student
        [HttpPut("ChangeStatus")]
        public async Task<IActionResult> ChangeStudentStatus( List<string> userIds, int status)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.ChangeUsersStatusSelected(userIds, status);
            return Ok(aPIResponse);
            }
        #endregion

        #region
        [HttpGet("export")]
        public async Task<IActionResult> ExportStudentsToExcel([FromQuery] string? majorId)
            {
            APIResponse aPIResponse = new APIResponse();
            var export = await _service.ExportStudentsToExcel(majorId);

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
        #region

        [HttpGet("exportEmpty")]
        public async Task<IActionResult> ExportFormAddStudent()
            {
            APIResponse aPIResponse = new APIResponse();
            var export = await _service.ExportFormAddStudent();
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
