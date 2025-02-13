using BusinessObject.AppDBContext;
using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
namespace UserService.Repository.UserRepository
{
	public class UserRepository : IUserRepository
	{
		/// <summary>
		/// show a list full of user
		/// </summary>
		/// <returns>true if success</returns>
		/// <exception cref="Exception"></exception>
		public List<UserDTO> GetAllUser()
		{
			try
			{
				using (var dbContext = new MyDbContext())
				{
					List<User> users = dbContext.User
						.Include(u => u.Role)
						.Include(u => u.Major)
						.ToList();
					List<UserDTO> userDTOs = new List<UserDTO>();
					foreach (var user in users)
					{
						UserDTO userDTO = new UserDTO();
						userDTO.CopyProperties(user);
						userDTO.RoleName = user.Role?.RoleName;
						userDTO.MajorName = user.Major?.MajorName;
						userDTOs.Add(userDTO);
					}
					return userDTOs;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		/// <summary>
		/// admin use to retrive a user
		/// </summary>
		/// <param name="id"></param>
		/// <returns>true if success</returns>
		/// <exception cref="Exception"></exception>
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
		/// <summary>
		/// admin add a new user
		/// </summary>
		/// <param name="userDTO"></param>
		/// <returns>true if success</returns>
		/// <exception cref="Exception"></exception>
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
		}
		/// <summary>
		/// admin update a user
		/// </summary>
		/// <param name="UpdateUserDTO"></param>
		/// <returns>true if success</returns>
		/// <exception cref="Exception"></exception>
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
		/// <summary>
		/// Get a User by ID to provide UserUpdate
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
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
		/// <summary>
		/// Update status for student
		/// </summary>
		/// <param name="id"></param>
		/// <returnstrue if success></returns>
		/// <exception cref="Exception"></exception>
		public bool UpdateStudentStatus(string id, int status)
		{
			try
			{
				var existingUser = GetUserToUpdate(id);
				if (existingUser != null)
				{
					existingUser.Status = status;
					using (var dbContext = new MyDbContext())
					{
						dbContext.User.Attach(existingUser);
						dbContext.Entry(existingUser).State = EntityState.Modified;
						dbContext.SaveChanges();
					}
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message, ex);
			}
		}
		/// <summary>
		/// User Sefl-update their infor
		/// </summary>
		/// <param name="UpdateInforDTO"></param>
		/// <returns></returns>
		/// <exception cref="Exception"></exception>
		public bool UpdateInfor(UserDTO UpdateInforDTO)
		{
			try
			{
				using (var dbContext = new MyDbContext())
				{
					var existingUser = GetUserToUpdate(UpdateInforDTO.UserId);
					if (existingUser == null)
					{
						return false;
					}
					existingUser.CopyProperties(UpdateInforDTO);
					dbContext.Entry(existingUser).State = EntityState.Modified;
					dbContext.SaveChanges();
					return true;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message, ex);
			}
		}
		/// <summary>
		/// Check if user exist by email
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		public bool CheckUserByEmail(string email)
		{
			try
			{
				using (var db = new MyDbContext())
				{
					var user = db.User.FirstOrDefault(x => x.Email == email);
					if (user != null)
					{
						return true;
					}
					return false;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message, ex);
			}
		}
		/// <summary>
		/// Reset password for user
		/// </summary>
		/// <param name="email"></param>
		/// <param name="newPassword"></param>
		/// <returns></returns>
		public bool ResetPassword(ResetPasswordDTO resetPassword)
		{
			try
			{
				using (var db = new MyDbContext())
				{
					var user = db.User.FirstOrDefault(x => x.Email == resetPassword.Email);
					if (user != null)
					{
						user.PasswordHash = resetPassword.Password;
						db.SaveChanges();
						return true;
					}
					return false;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message, ex);
			}
		}
	}
}
