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
        public async Task<APIResponse> GetAllTeacher()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetAllTeacher();
            return aPIResponse;
            }
        #endregion

        #region Get All Teacher Available
        [HttpGet("Available/{majorId}")]
        public async Task<APIResponse> GetAllTeacherAvailableByMajorId(string majorId)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetAllTeacherAvailableByMajorId(majorId);
            return aPIResponse;
            }
        #endregion

        #region Add New Teacher
        [HttpPost]
        public async Task<APIResponse> AddNewTeacher(UserDTO userDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.AddNewTeacher(userDTO);
            return aPIResponse;
            }
        #endregion

        #region Update Teacher
        [HttpPut]
        public async Task<APIResponse> UpdateTeacher(UserDTO userDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.UpdateTeacher(userDTO);
            return aPIResponse;
            }
        #endregion

        #region Import From Excel
        [HttpPost("import")]
        public async Task<APIResponse> ImportTeachers(IFormFile file)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.ImportTeachersFromExcel(file);
            return aPIResponse;
            }
        #endregion
        }
    }
