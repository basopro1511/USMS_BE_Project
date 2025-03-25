using BusinessObject;
using BusinessObject.ModelDTOs;
using Microsoft.AspNetCore.Mvc;
using UserService.Services.StudentServices;

namespace UserService.Controllers.StudentController
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class StudentController : Controller
    {
        private readonly StudentService _service;
        public StudentController(StudentService studentService)
        {
            _service= studentService;
        }
        #region Get All Student
        [HttpGet]
        public async Task<APIResponse> GetAllStudent()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetAllStudent();
            return aPIResponse;
            }
        #endregion

        #region Get Student By Id
        [HttpGet("{id}")]
        public async Task<APIResponse> GetStudentById(string id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetUserById(id);
            return aPIResponse;
            }
        #endregion

        #region Add New Student
        [HttpPost]
        public async Task<APIResponse> AddNewStudent(UserDTO userDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.AddNewStudent(userDTO);
            return aPIResponse;
            }
        #endregion

        #region Update Student
        [HttpPut]
        public async Task<APIResponse> UpdateStudent(UserDTO userDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.UpdateStudent(userDTO);
            return aPIResponse;
            }
        #endregion

        #region Import From Excel
        [HttpPost("import")]
        public async Task<APIResponse> ImportStudents(IFormFile file)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.ImportStudentsFromExcel(file);
            return aPIResponse;
            }
        #endregion
        }
    }
