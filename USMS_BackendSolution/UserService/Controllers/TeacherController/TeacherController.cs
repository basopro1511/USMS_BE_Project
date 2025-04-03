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
        }
    }
