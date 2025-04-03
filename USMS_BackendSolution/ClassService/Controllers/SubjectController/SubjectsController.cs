using ClassBusinessObject;
using ClassBusinessObject.ModelDTOs;
using Services.SubjectServices;
using Microsoft.AspNetCore.Mvc;
using Services.ClassServices;

namespace ClassService.Controllers.SubjectController
    {
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
        {
        private readonly SubjectService _subjectService;
        public SubjectsController(SubjectService subjectService)
            {
            _subjectService=subjectService;
            }

        [HttpGet]
        public async Task<IActionResult> GetSubjects()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _subjectService.GetAllSubjects();
            return Ok(aPIResponse);
            }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubjectById(string id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _subjectService.GetSubjectById(id);
            return Ok(aPIResponse);
            }

        [HttpGet("Available")]
        public async Task<IActionResult> GetSubjectsAvailable()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _subjectService.GetAllSubjectsAvailable();
            return Ok(aPIResponse);
            }

        [HttpPost]
        public async Task<IActionResult> CreateSubject(SubjectDTO subject)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _subjectService.CreateSubject(subject);
            return Ok(aPIResponse);
            }

        [HttpPut]
        public async Task<IActionResult> UpdateSubject(SubjectDTO subject)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _subjectService.UpdateSubject(subject);
            return Ok(aPIResponse);
            }

        [HttpGet("SwitchStateSubject/{subjectId}/{status}")]
        public async Task<IActionResult> SwitchStateSubject(string subjectId, int status)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _subjectService.SwitchStateSubject(subjectId, status);
            return Ok(aPIResponse);
            }

        #region Get Subjects by MajorId
        [HttpGet("GetSubjectByMajor/{id}")]
        public async Task<IActionResult> GetSubjectsByMajor(string id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _subjectService.GetSubjectByMajorId(id);
            return Ok(aPIResponse);
            }
        #endregion

        #region GetSubjects by MajorId and Term
        [HttpGet("GetSubjectByMajorAndTerm/{id}/{term}")]
        public async Task<IActionResult> GetSubjectsByMajorAndTerm(string id, int term)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _subjectService.GetSubjectByMajorIdAndTerm(id, term);
            return Ok(aPIResponse);
            }
        #endregion

        [HttpPut("ChangeSelectStatus")]
        public async Task<IActionResult> ChangeStatusSubject(List<string> ids, int status)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _subjectService.ChangeSubjectStatusSelected(ids, status);
            return Ok(aPIResponse);
            }
        }
    }

