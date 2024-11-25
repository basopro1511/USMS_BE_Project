using ClassBusinessObject;
using ClassBusinessObject.ModelDTOs;
using ClassDataAccess.Repositories.SubjectRepository;

namespace ClassDataAccess.Services.SubjectServices
{
	public class SubjectService
	{
		private readonly ISubjectRepository _subjectRepository;
		public SubjectService()
		{
			_subjectRepository = new SubjectRepository();
		}

		#region Get All Subjects
		/// <summary>
		/// Get All Subjects
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public APIResponse GetAllSubjects()
		{
			APIResponse aPIResponse = new APIResponse();
			List<SubjectDTO>? subjects = _subjectRepository.GetAllSubjects();
			if (subjects == null || subjects.Count == 0)
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "Subject is null";
			}
			aPIResponse.Result = subjects;
			return aPIResponse;
		}
		#endregion

		#region Create Subject
		/// <summary>
		/// Create Subject
		/// </summary>
		/// <param name="subjectDTO"></param>
		/// <returns></returns>
		public APIResponse CreateSubject(SubjectDTO subjectDTO)
		{
			APIResponse aPIResponse = new APIResponse();
			_subjectRepository.CreateSubject(subjectDTO);
			aPIResponse.IsSuccess = true;
			return aPIResponse;
		}
		#endregion

		#region Update Subject
		/// <summary>
		/// Update Subject
		/// </summary>
		/// <param name="subjectDTO"></param>
		/// <returns></returns>
		public APIResponse UpdateSubject(SubjectDTO subjectDTO)
		{
			APIResponse aPIResponse = new APIResponse();
			_subjectRepository.UpdateSubject(subjectDTO);
			aPIResponse.IsSuccess = true;
			return aPIResponse;
		}
		#endregion

		#region Switch state subject
		/// <summary>
		/// Switch state subject
		/// </summary>
		/// <param name="subjectId"></param>
		/// <returns></returns>
		public APIResponse SwitchStateSubject(string subjectId)
		{
			APIResponse aPIResponse = new APIResponse();
			_subjectRepository.SwitchStateSubject(subjectId);
			aPIResponse.IsSuccess = true;
			return aPIResponse;
		}
		#endregion
	}
}
