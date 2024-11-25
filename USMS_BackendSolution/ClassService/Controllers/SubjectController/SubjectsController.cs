using ClassBusinessObject;
using ClassBusinessObject.ModelDTOs;
using ClassDataAccess.Services.ClassServices;
using Microsoft.AspNetCore.Mvc;

namespace ClassService.Controllers.SubjectController
{
	[Route("api/[controller]")]
	[ApiController]
	public class SubjectsController : ControllerBase
	{
		private readonly ClassSubjectService _classSubjectService;
		public SubjectsController(ClassSubjectService classSubjectService)
		{
			_classSubjectService = classSubjectService;
		}

		[HttpGet("Subject")]
		public APIResponse GetSubjects()
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _classSubjectService.GetAllSubjects();
			return aPIResponse;
		}

		[HttpPost("CreateSubject")]
		public APIResponse CreateSubject(SubjectDTO subject)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _classSubjectService.CreateSubject(subject);
			return aPIResponse;
		}

		[HttpPut("UpdateSubject/{subjectId}")]
		public APIResponse UpdateSubject(SubjectDTO subject)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _classSubjectService.UpdateSubject(subject);
			return aPIResponse;
		}

		[HttpPut("SwitchStateSubject")]
		public APIResponse SwitchStateSubject(string subjectId)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _classSubjectService.SwitchStateSubject(subjectId);
			return aPIResponse;
		}
	}
}
