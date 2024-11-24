using ClassBusinessObject;
using ClassDataAccess.Services.ClassServices;
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
    }
}
