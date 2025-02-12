using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepository.IRoleRepository
{
    public interface IRoleRepository
    {
        public List<Role> getAllRole();
        public Role GetRoleById(int id);
        public bool AddNewRole(Role role);
        public bool DeleteRole(int id);
    }
}
