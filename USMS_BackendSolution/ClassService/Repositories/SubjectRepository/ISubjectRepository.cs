using ClassBusinessObject.ModelDTOs;

namespace Repositories.SubjectRepository
{
	public interface ISubjectRepository
	{
		public bool CreateSubject(SubjectDTO subjectDTO);
		public bool UpdateSubject(SubjectDTO SubjectDTO);
		public SubjectDTO GetSubjectsById(string subjectId);
		public List<SubjectDTO>? GetAllSubjects();
		public bool SwitchStateSubject(string subjectId, int status);
	}
}
