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

namespace IRepository.ICustomerRepository
{
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// Get All User in Database
        /// </summary>
        /// <returns>List of User exist in Database</returns>
        public List<User> getAllUser()
        {
            try
            {             
                using (var dbContext = new MyDbContext())
                {
                    List<User> users = dbContext.User.ToList();
                    return users;
                }
            }
            catch (Exception ex)
            {
                throw new Exception (ex.Message);
            }
        }

        /// <summary>
        /// Retrieve a User by their email
        /// </summary>
        /// <param name="email"></param>
        /// <returns> a User by their email </returns>
        /// <exception cref="Exception"></exception>
        public User GetUserByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email), "User email cannot be null or empty.");
            }
            try
            {
                var users = getAllUser();
                User user = users.FirstOrDefault(x => x.Email == email);
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        ///  Adds a new customer to the database.
        /// </summary>
        /// <param name="User"></param>
        /// <exception cref="Exception"></exception>
        public bool AddNewUser (User user)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    dbContext.User.Add(user);
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
        /// Detele Customer by ID
        /// </summary>
        /// <param name="email"></param>
        /// <returns>A new list without the customer just deleted</returns>
        public bool DeleteUser (string email)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingUser = GetUserByEmail(email);
                    dbContext.User.Remove(existingUser);
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
