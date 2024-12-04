using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.UserRepository
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
