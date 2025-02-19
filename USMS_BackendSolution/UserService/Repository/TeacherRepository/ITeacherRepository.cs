using BusinessObject.ModelDTOs;

namespace UserService.Repository.TeacherRepository
    {
    public interface ITeacherRepository
        {
        public List<UserDTO> GetAllTeacher();
        public List<UserDTO> GetAllTeacherAvailableByMajorId(string majorId);

        }
    }
