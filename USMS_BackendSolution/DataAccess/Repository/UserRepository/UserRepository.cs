using BusinessObject.AppDBContext;
using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.UserRepository
{
    public class UserRepository : IUserRepository
    {
        public List<UserDTO> GetAllUser()
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    List<User> users = dbContext.User.ToList();
                    List<UserDTO> userDTOs = new List<UserDTO>();
                    foreach (var user in users)
                    {
                        UserDTO UserDTO = new UserDTO();
                        UserDTO.CopyProperties(user);
                        userDTOs.Add(UserDTO);
                    }
                    return userDTOs;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public UserDTO GetUserById(string id)
        {
            try
            {
                var users = GetAllUser();
                UserDTO userDTO = users.FirstOrDefault(x => x.UserId == id);
                return userDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool AddNewUser(UserDTO userDTO)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var user = new User();
                    user.CopyProperties(userDTO);
                    dbContext.User.Add(user);
                    dbContext.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;
        }
        public bool UpdateUser(UserDTO UpdateUserDTO)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingUser = GetUserToUpdate(UpdateUserDTO.UserId);
                    if (existingUser == null)
                    {
                        return false;
                    }
                    existingUser.CopyProperties(UpdateUserDTO);
                    dbContext.Entry(existingUser).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public User GetUserToUpdate(string id)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingUser = dbContext.User.FirstOrDefault(cs => cs.UserId == id);
                    return existingUser;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public bool DisableStudent(string id)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingUser = dbContext.User.FirstOrDefault(cs => cs.UserId == id);
                    existingUser.Status = 0;
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public bool OnScheduleStudent(string id)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingUser = dbContext.User.FirstOrDefault(cs => cs.UserId == id);
                    existingUser.Status = 1;
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public bool DefermentStudent(string id)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingUser = dbContext.User.FirstOrDefault(cs => cs.UserId == id);
                    existingUser.Status = 2;
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }
        public bool GraduatedStudent(string id)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingUser = dbContext.User.FirstOrDefault(cs => cs.UserId == id);
                    existingUser.Status = 3;
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
