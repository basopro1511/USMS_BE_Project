using ClassBusinessObject;
using ClassBusinessObject.ModelDTOs;
using ClassDataAccess.Repositories.ClassSubjectRepository;

namespace ClassDataAccess.Services.ClassServices
{
	public class ClassSubjectService
	{
		private readonly IClassRepository _classRepository;
		public ClassSubjectService()
		{
			_classRepository = new ClassRepository();
		}
		//Copy Paste 
		#region Copy + Pase  
		#endregion

		#region Get All Schedule
		/// <summary>
		/// Retrive all ClassSubject in Database
		/// </summary>
		/// <returns>a list of all Class Subject in DB</returns>
		public APIResponse GetAllClassSubject()
		{
			APIResponse aPIResponse = new APIResponse();
			List<ClassSubjectDTO> classSubjects = _classRepository.GetAllClassSubjects();
			if (classSubjects == null)
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "Don't have any Class Subject available!";
			}
			aPIResponse.Result = classSubjects;
			return aPIResponse;
		}
		#endregion

		#region Get ClassSubject By ClassSubjectId 
		/// <summary>
		/// Retrive a ClassSubject with ClassSubjectId
		/// </summary>
		/// <param name="id"></param>
		/// <returns>a ClassSubject by Id</returns>
		public APIResponse GetClassSubjectById(int id)
		{
			APIResponse aPIResponse = new APIResponse();
			ClassSubjectDTO classSubject = _classRepository.GetClassSubjectById(id);
			if (classSubject == null)
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "ClassSubject with Id: " + id + " is not found";
			}
			aPIResponse.Result = classSubject;
			return aPIResponse;
		}
		#endregion

		#region Get ClassSubject By ClassId
		/// <summary>
		/// Retrive list ClassSubjects by ClassId
		/// </summary>
		/// <param name="id"></param>
		/// <returns>a list ClassSubjects by ClassId </returns>
		public APIResponse GetClassSubjectByClassId(string id)
		{
			APIResponse aPIResponse = new APIResponse();
			List<ClassSubjectDTO> classSubjects = _classRepository.GetClassSubjectByClassId(id);
			if (classSubjects == null || classSubjects.Count == 0)
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "ClassSubjects with Id: " + id + " is not found";
			}
			aPIResponse.Result = classSubjects;
			return aPIResponse;
		}
		#endregion

		#region Get All Subjects
		/// <summary>
		/// Get All Subjects
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public APIResponse GetAllSubjects()
		{
			APIResponse aPIResponse = new APIResponse();
			List<SubjectDTO>? subjects = _classRepository.GetAllSubjects();
			if (subjects == null || subjects.Count == 0)
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "Subject is null";
			}
			aPIResponse.Result = subjects;
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
			_classRepository.UpdateSubject(subjectDTO);
			aPIResponse.IsSuccess = true;
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
			_classRepository.CreateSubject(subjectDTO);
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
			_classRepository.SwitchStateSubject(subjectId);
			aPIResponse.IsSuccess = true;
			return aPIResponse;
		}
		#endregion
	}
}
