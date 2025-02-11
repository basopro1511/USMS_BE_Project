using BusinessObject.ModelDTOs;

namespace UserService.Repository.StudentRepository
{
    public interface IStudentRepository
    {
        public List<StudentDTO> GetAllStudent();
        public StudentDTO GetStudentById(string id);
        public bool AddNewStudent(StudentDTO StudentDTO);
        public bool UpdateStudent(StudentDTO updateStudentDTO);
        public bool UpdateStudentStatus(string id, int status);
    }
}
