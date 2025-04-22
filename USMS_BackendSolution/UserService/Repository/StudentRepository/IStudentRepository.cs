using BusinessObject.ModelDTOs;
using BusinessObject.Models;

namespace UserService.Repository.StudentRepository
{
    public interface IStudentRepository
    {
        public Task<List<User>> GetAllStudent();
        public Task<bool> AddNewStudent(User userDTO);
        public Task<bool> UpdateStudent(User userDTO);
        public Task<bool> AddStudentAsync(List<User> users);
        public Task<bool> AddNewStudentForStudentTable(Student userDTO);
        public Task<bool> UpdateStudentTerm(string userId, int newTerm);
        public Task<User> GetStudentById(string userId);
        }
    }
