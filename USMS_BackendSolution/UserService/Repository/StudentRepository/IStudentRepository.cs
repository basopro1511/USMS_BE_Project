using BusinessObject.ModelDTOs;
using BusinessObject.Models;

namespace UserService.Repository.StudentRepository
{
    public interface IStudentRepository
    {
        public Task<List<UserDTO>> GetAllStudent();
        public Task<bool> AddNewStudent(UserDTO userDTO);
        public Task<bool> UpdateStudent(UserDTO userDTO);
        public Task<bool> AddStudentAsync(List<User> users);
        public Task<bool> AddNewStudentForStudentTable(StudentTableDTO userDTO);
        public Task<bool> UpdateStudentTerm(string userId, int newTerm);
        public Task<UserDTO> GetStudentById(string userId);
        }
    }
