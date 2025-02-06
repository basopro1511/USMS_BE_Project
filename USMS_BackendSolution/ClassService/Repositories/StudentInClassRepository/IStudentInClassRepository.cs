using ClassBusinessObject.ModelDTOs;

namespace ClassService.Repositories.StudentInClassRepository
    {
    public interface IStudentInClassRepository
        {
        public List<StudentInClassDTO> GetAllStudentInClass();
        public List<int> GetClassSubjectId(string studentId);
        }
    }
