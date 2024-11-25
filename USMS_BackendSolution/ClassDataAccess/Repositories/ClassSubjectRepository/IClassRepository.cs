using ClassBusinessObject.ModelDTOs;

namespace ClassDataAccess.Repositories.ClassSubjectRepository
{
	public interface IClassRepository
	{
		public List<ClassSubjectDTO> GetAllClassSubjects();
		public ClassSubjectDTO GetClassSubjectById(int id);
		public List<ClassSubjectDTO> GetClassSubjectByClassId(string classId);
		public void CreateSubject(SubjectDTO subjectDTO);
		public void UpdateSubject(SubjectDTO SubjectDTO);
		public List<SubjectDTO>? GetAllSubjects();
		public void SwitchStateSubject(string subjectId);
	}
}
