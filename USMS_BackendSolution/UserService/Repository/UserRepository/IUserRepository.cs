using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace UserService.Repository.UserRepository
{
    public interface IUserRepository
    {
        public Task<List<UserDTO>> GetAllUser();
        public Task<UserDTO> GetUserById(string id);
        public Task<UserDTO> GetUserByEmail(string email);

        //#region Old
        //public bool AddNewUser(UserDTO userDTO);
        //public bool UpdateUser(UserDTO UpdateUserDTO);
        //public bool UpdateInfor(UserDTO UpdateInforDTO);
        //public bool UpdateStudentStatus(string id, int status);
        //#endregion

        }
    }
