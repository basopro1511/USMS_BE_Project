using BusinessObject.ModelDTOs;
namespace UserService.Repository.UserRepository
{
	public interface IUserRepository
    {
        public List<UserDTO> GetAllUser();
        public UserDTO GetUserById(string id);
        public bool AddNewUser(UserDTO userDTO);
        public bool UpdateUser(UserDTO UpdateUserDTO);
        public bool UpdateInfor(UserDTO UpdateInforDTO);
        public bool UpdateStudentStatus(string id, int status);

    }
}
