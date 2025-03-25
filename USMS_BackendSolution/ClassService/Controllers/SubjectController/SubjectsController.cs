using ClassBusinessObject;
using ClassBusinessObject.ModelDTOs;
using Services.SubjectServices;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<APIResponse> GetSubjects()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _subjectService.GetAllSubjects();
            return aPIResponse;
            }

        [HttpGet("{id}")]
        public async Task<APIResponse> GetSubjectById(string id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _subjectService.GetSubjectById(id);
            return aPIResponse;
            }

        [HttpGet("Available")]
        public async Task<APIResponse> GetSubjectsAvailable()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _subjectService.GetAllSubjectsAvailable();
            return aPIResponse;
            }

        [HttpPost]
        public async Task<APIResponse> CreateSubject(SubjectDTO subject)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _subjectService.CreateSubject(subject);
            return aPIResponse;
            }

        [HttpPut]
        public async Task<APIResponse> UpdateSubject(SubjectDTO subject)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _subjectService.UpdateSubject(subject);
            return aPIResponse;
            }

        [HttpGet("SwitchStateSubject/{subjectId}/{status}")]
        public async Task<APIResponse> SwitchStateSubject(string subjectId, int status)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _subjectService.SwitchStateSubject(subjectId, status);
            return aPIResponse;
            }

        #region Get Subjects by MajorId
        [HttpGet("GetSubjectByMajor/{id}")]
        public async Task<APIResponse> GetSubjectsByMajor(string id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _subjectService.GetSubjectByMajorId(id);
            return aPIResponse;
            }
        #endregion

        #region GetSubjects by MajorId and Term
        [HttpGet("GetSubjectByMajorAndTerm/{id}/{term}")]
        public async Task<APIResponse> GetSubjectsByMajorAndTerm(string id, int term)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _subjectService.GetSubjectByMajorIdAndTerm(id, term);
            return aPIResponse;
            }
        #endregion

        }
    }
