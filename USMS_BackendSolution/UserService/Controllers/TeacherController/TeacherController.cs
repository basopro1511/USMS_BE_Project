using BusinessObject;
using BusinessObject.ModelDTOs;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Services.TeacherService;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace UserService.Controllers.TeacherController
    {
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
        {
        private readonly TeacherService _service;
        public TeacherController(TeacherService service)
            {
            _service=service;
            }

        #region Get All Teacher
        [HttpGet]
        public async Task<IActionResult> GetAllTeacher()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetAllTeacher();
            return Ok(aPIResponse);
            }
        #endregion

        #region Get All Teacher Available
        [HttpGet("Available/{majorId}")]
        public async Task<IActionResult> GetAllTeacherAvailableByMajorId(string majorId)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetAllTeacherAvailableByMajorId(majorId);
            return Ok(aPIResponse);
            }
        #endregion

        #region Add New Teacher
        [HttpPost]
        public async Task<IActionResult> AddNewTeacher(UserDTO userDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.AddNewTeacher(userDTO);
            return Ok(aPIResponse);
            }
        #endregion

        #region Update Teacher
        [HttpPut]
        public async Task<IActionResult> UpdateTeacher(UserDTO userDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.UpdateTeacher(userDTO);
            return Ok(aPIResponse);
            }
        #endregion

        #region Import From Excel
        [HttpPost("import")]
        public async Task<IActionResult> ImportTeachers(IFormFile file)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.ImportTeachersFromExcel(file);
            return Ok(aPIResponse);
            }
        #endregion

        #region Update Status
        [HttpPut("ChangeStatus")]
        public async Task<IActionResult> ChangeStudentStatus(List<string> userIds, int status)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.ChangeUsersStatusSelected(userIds, status);
            return Ok(aPIResponse);
            }
        #endregion

        #region
        [HttpGet("export")]
        public async Task<IActionResult> ExportTeachersToExcel([FromQuery] string? majorId, int? status)
            {
            APIResponse aPIResponse = new APIResponse();
            var export = await _service.ExportTeachersToExcel(majorId, status);
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
        /// <summary>
        /// Export Empty form to add teacher
        /// </summary>
        /// <returns></returns>
        [HttpGet("exportEmpty")]
        public async Task<IActionResult> ExportFormAddTeacher()
            {
            APIResponse aPIResponse = new APIResponse();
            var export = await _service.ExportFormAddTeacher();
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
