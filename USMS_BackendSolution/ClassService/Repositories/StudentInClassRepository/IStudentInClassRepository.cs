using ClassBusinessObject.ModelDTOs;

namespace ClassService.Repositories.StudentInClassRepository
    {
    public interface IStudentInClassRepository
        {
        public List<StudentInClassDTO> GetAllStudentInClass();
        public StudentInClassDTO GetStudentInClassByStudentId(string studentId);
        public List<StudentInClassDTO> GetStudentInClassByClassId(int classSubjectId);
        public List<int> GetClassSubjectId(string studentId);
        public bool AddStudentToClass(StudentInClassDTO studentInClassDTO);
        public bool AddMultipleStudentsToClass(List<StudentInClassDTO> studentsInClassDTO);
        public bool UpdateStudentInClass(StudentInClassDTO studentInClassDTO);
        public bool DeleteStudentFromClass(int studentClassId);
        public StudentInClassDTO GetStudentInClassByStudentIdAndClass(string studentId, int classSubjectId);
        }
    }
