using ClassBusinessObject.ModelDTOs;

namespace ClassDataAccess.Repositories.SubjectRepository
{
	public interface ISubjectRepository
	{
		public void CreateSubject(SubjectDTO subjectDTO);
		public void UpdateSubject(SubjectDTO SubjectDTO);
		public List<SubjectDTO>? GetAllSubjects();
		public void SwitchStateSubject(string subjectId);
	}
}
