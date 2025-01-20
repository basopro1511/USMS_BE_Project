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
			_subjectService = subjectService;
		}

		[HttpGet]
		public APIResponse GetSubjects()
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _subjectService.GetAllSubjects();
			return aPIResponse;
		}

		[HttpPost]
		public APIResponse CreateSubject(SubjectDTO subject)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _subjectService.CreateSubject(subject);
			return aPIResponse;
		}

		[HttpPut]
		public APIResponse UpdateSubject(SubjectDTO subject)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _subjectService.UpdateSubject(subject);
			return aPIResponse;
		}

		[HttpGet("SwitchStateSubject/{subjectId}/{status}")]
		public APIResponse SwitchStateSubject(string subjectId, int status)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _subjectService.SwitchStateSubject(subjectId, status);
			return aPIResponse;
		}

        #region Get Subjects by MajorId
        [HttpGet("GetSubjectByMajor/{id}")]
        public APIResponse GetSubjectsByMajor(string id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _subjectService.GetSubjectByMajorId(id);
            return aPIResponse;
        }
        #endregion

        #region GetSubjects by MajorId and Term
        [HttpGet("GetSubjectByMajorAndTerm/{id}/{term}")]
        public APIResponse GetSubjectsByMajorAndTerm(string id, int term)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _subjectService.GetSubjectByMajorIdAndTerm(id,term);
            return aPIResponse;
        }
        #endregion
    }
}
