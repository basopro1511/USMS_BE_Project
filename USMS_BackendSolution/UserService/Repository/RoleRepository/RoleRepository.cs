using BusinessObject.AppDBContext;
using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IRepository.IRoleRepository
{
    public class RoleRepository : IRoleRepository
    {
        /// <summary>
        /// Get All Role in Database
        /// </summary>
        /// <returns>List of Role exist in Database</returns>
        public List<Role> getAllRole()
        {
            try
            {             
                using (var dbContext = new MyDbContext())
                {
                    List<Role> roles = dbContext.Roles.ToList();
                    return roles;
                }
            }
            catch (Exception ex)
            {
                throw new Exception (ex.Message);
            }
        }

        /// <summary>
        /// Retrieve a Role by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> a Role by id</returns>
        /// <exception cref="Exception"></exception>
        public Role GetRoleById(int id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id), "Role id cannot be null or empty.");
            }
            try
            {
                var roles = getAllRole();
                Role role = roles.FirstOrDefault(x => x.RoleId == id);
                return role;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///  Adds a new role to the database.
        /// </summary>
        /// <param name="Role"></param>
        /// <exception cref="Exception"></exception>
        public bool AddNewRole (Role role)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    dbContext.Roles.Add(role);
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        /// <summary>
        /// Delete Role by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A new list without the Role just deleted</returns>
        public bool DeleteRole (int id)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingRole = GetRoleById(id);
                    dbContext.Roles.Remove(existingRole);
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
