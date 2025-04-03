using ClassBusinessObject.ModelDTOs;
using ClassBusinessObject.Models;

namespace ClassService.Repositories.StudentInClassRepository
    {
    public interface IStudentInClassRepository
        {
        public Task<List<StudentInClass>> GetAllStudentInClass();
        public Task<StudentInClass> GetStudentInClassByStudentId(string studentId);
        public Task<List<StudentInClass>> GetStudentInClassByClassId(int classSubjectId);
        public Task<List<int>> GetClassSubjectId(string studentId);
        public Task<bool> AddStudentToClass(StudentInClass studentInClassDTO);
        public Task<bool> AddMultipleStudentsToClass(List<StudentInClass> studentsInClassDTO);
        public Task<bool> UpdateStudentInClass(StudentInClass studentInClassDTO);
        public Task<bool> DeleteStudentFromClass(int studentClassId);
        public Task<StudentInClass> GetStudentInClassByStudentIdAndClass(string studentId, int classSubjectId);
        public Task<int> GetStudentCountByClassSubjectId(int classSubjectId);
        }
    }
