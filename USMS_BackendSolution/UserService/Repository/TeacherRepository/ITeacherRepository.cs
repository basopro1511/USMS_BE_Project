using BusinessObject.ModelDTOs;
using BusinessObject.Models;

namespace UserService.Repository.TeacherRepository
    {
    public interface ITeacherRepository
        {
        public Task<List<User>> GetAllTeacher();
        public Task<List<User>> GetAllTeacherAvailableByMajorId(string majorId);
        public Task<bool> AddNewTeacher(User user);
        public Task<bool> UpdateTeacher(User user);
        public Task<bool> AddTeachersAsync(List<User> teachers);
        }
    }
