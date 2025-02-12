using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepository.IUserRepository
{
    public interface IUserRepository
    {
        public List<User> getAllUser();
        public User GetUserByEmail(string email);
        public bool AddNewUser(User user);
        public bool DeleteUser(string email);
    }
}
