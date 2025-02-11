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
    public class StudentInClassController : ControllerBase {
        private readonly StudentInClassService _service;
        public StudentInClassController(StudentInClassService service) {
            _service=service;
            }

        [HttpGet]
        public APIResponse GetAllStudentInClass() {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=_service.GetAllStudentInClass();
            return aPIResponse;
            }

        [HttpPost]
        public APIResponse AddStudentToClass(StudentInClassDTO studentInClassDTO) {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=_service.AddStudentToClass(studentInClassDTO);
            return aPIResponse;
            }
        [HttpPost("AddMutipleStudents")]
        public APIResponse AddStudentToClass(List<StudentInClassDTO> studentInClassDTOs) {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=_service.AddMultipleStudentsToClass(studentInClassDTOs);
            return aPIResponse;
            }


        [HttpDelete]
        public APIResponse RemoveStudentToClass(int studentId) {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=_service.DeleteStudentFromClass(studentId);
            return aPIResponse;
            }

        #region Get ClassSubjectIds by Student Id
        [HttpGet("ClassSubject/{id}")]
        public APIResponse GetClassSubjectByStudentId(string id) {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=_service.GetClassSubjectId(id);
            return aPIResponse;
            }
        }
    #endregion
    }
