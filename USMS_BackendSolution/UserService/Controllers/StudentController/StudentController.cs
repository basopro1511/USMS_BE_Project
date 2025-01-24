using BusinessObject;
using BusinessObject.ModelDTOs;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers.StudentController
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    public class StudentController : Controller
    {
        private readonly Services.StudentService.StudentService _studentService;
        public StudentController(Services.StudentService.StudentService studentService)
        {
            _studentService = studentService;
        }
        // GET: api/Students
        [HttpGet]
        public APIResponse GetAllUser()
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _studentService.GetAllStudent();
            return aPIResponse;
        }
        // GET: api/Students
        [HttpGet("{id}")]
        public APIResponse GetStudentById(string id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _studentService.GetStudentById(id);
            return aPIResponse;
        }
        // POST: api/Student
        [HttpPost]
        public APIResponse AddStudent(AddStudentDTO addstudentDTO)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _studentService.AddStudent(addstudentDTO);
            return aPIResponse;
        }
        // PUT: api/Student/{id}
        [HttpPut("UpdateStudent/{id}")]
        public APIResponse UpdateUser(string id, UpdateStudentDTO updateStudentDTO)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _studentService.UpdateStudent(id, updateStudentDTO);
            return aPIResponse;
        }
        //PUT: api/User/{id}
        [HttpPut("UpdateStudentStatus/{id}")]
        public APIResponse UpdateStudentStatus(string id, int status)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _studentService.UpdateStudentStatus(id, status);
            return aPIResponse;
        }
    }
}
