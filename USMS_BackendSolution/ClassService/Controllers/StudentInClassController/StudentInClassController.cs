using ClassBusinessObject;
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
            _service = service;
            }

        [HttpGet]
        public APIResponse GetAllStudentInClass()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _service.GetAllStudentInClass();
            return aPIResponse;
            }
        #region get Class Subject

        [HttpGet("ClassSubject/{id}")]
        public APIResponse GetClassSubjectByStudentId(string id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _service.GetClassSubjectId(id);
            return aPIResponse;
            }
        }

    #endregion
    }
