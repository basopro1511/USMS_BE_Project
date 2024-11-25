using ClassBusinessObject.ModelDTOs;

namespace ClassDataAccess.Repositories.SubjectRepository
{
	public interface ISubjectRepository
	{
		public bool CreateSubject(SubjectDTO subjectDTO);
		public bool UpdateSubject(SubjectDTO SubjectDTO);
		public List<SubjectDTO>? GetAllSubjects();
		public bool SwitchStateSubject(string subjectId);
	}
}
