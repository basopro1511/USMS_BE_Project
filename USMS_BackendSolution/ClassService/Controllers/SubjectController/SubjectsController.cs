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

		[HttpPut("SwitchStateSubject/{subjectId}")]
		public APIResponse SwitchStateSubject(string subjectId)
		{
			APIResponse aPIResponse = new APIResponse();
			aPIResponse = _subjectService.SwitchStateSubject(subjectId);
			return aPIResponse;
		}
	}
}
