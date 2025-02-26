using BusinessObject.ModelDTOs;

namespace UserService.Repository.TeacherRepository
    {
    public interface ITeacherRepository
        {
        public Task<List<UserDTO>> GetAllTeacher();
        public Task<List<UserDTO>> GetAllTeacherAvailableByMajorId(string majorId);
        public Task<bool> AddNewTeacher(UserDTO userDTO);
        public Task<bool> UpdateTeacher(UserDTO userDTO);

        }
    }
