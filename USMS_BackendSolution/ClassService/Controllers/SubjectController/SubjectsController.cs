using ClassBusinessObject;
using ClassBusinessObject.ModelDTOs;
using ClassDataAccess.Services.SubjectServices;
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

		[HttpGet("Subject")]
		public APIResponse GetSubjects()
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _subjectService.GetAllSubjects();
			return aPIResponse;
		}

		[HttpPost("CreateSubject")]
		public APIResponse CreateSubject(SubjectDTO subject)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _subjectService.CreateSubject(subject);
			return aPIResponse;
		}

		[HttpPut("UpdateSubject/{subjectId}")]
		public APIResponse UpdateSubject(SubjectDTO subject)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _subjectService.UpdateSubject(subject);
			return aPIResponse;
		}

		[HttpPut("SwitchStateSubject")]
		public APIResponse SwitchStateSubject(string subjectId)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _subjectService.SwitchStateSubject(subjectId);
			return aPIResponse;
		}
	}
}
