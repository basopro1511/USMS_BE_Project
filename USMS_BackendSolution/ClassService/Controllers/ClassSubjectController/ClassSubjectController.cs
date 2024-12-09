using ClassBusinessObject;
using ClassBusinessObject.ModelDTOs;
using Services.ClassServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClassService.Controllers.ClassSubjectController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClassSubjectController : ControllerBase
    {
        private readonly ClassSubjectService _classSubjectService;
        public ClassSubjectController(ClassSubjectService classSubjectService)
        {
            _classSubjectService = classSubjectService;
        }

        // GET: api/ClassSubject
        [HttpGet]
        public APIResponse GetAllClassSubject()
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _classSubjectService.GetAllClassSubject();
            return aPIResponse;
        }

        // GET: api/ClassSubject/1
        [HttpGet("{id}")]
        public APIResponse GetClassSubjectById(int id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _classSubjectService.GetClassSubjectById(id);
            return aPIResponse;
        }

        // GET: api/ClassSubject/1
        [HttpGet("classId/{classId}")]
        public APIResponse GetClassSubjectByClassId(string classId)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _classSubjectService.GetClassSubjectByClassId(classId);
            return aPIResponse;
        }

        // POST: api/ClassSubject
        [HttpPost]
        public APIResponse AddNewClassSubject(AddUpdateClassSubjectDTO classSubjectDTO)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _classSubjectService.AddNewClassSubject(classSubjectDTO);
            return aPIResponse;
        }

        // PUT: api/ClassSubject
        [HttpPut]
        public APIResponse UpdateClassSubject(AddUpdateClassSubjectDTO classSubjectDTO)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _classSubjectService.UpdateClassSubject(classSubjectDTO);
            return aPIResponse;
        }

        [HttpPut("ChangeStatus/{id}")]
        public APIResponse ChangeStatusClassSubject(int id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _classSubjectService.ChangeStatusClassSubject(id);
            return aPIResponse;
        }
    }
}
