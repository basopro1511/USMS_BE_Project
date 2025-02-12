using BusinessObject;
using BusinessObject.ModelDTOs;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Resources;
using System.Security.Cryptography;
using System.Text;
using UserService.Repository.UserRepository;
namespace UserService.Services.UserServices
{
	public class UserServices
	{
		private readonly IUserRepository _userRepository;

		public UserServices()
		{
			_userRepository = new UserRepository();
		}
		/// <summary>
		/// Hashing password
		/// </summary>
		/// <param name="plainPassword"></param>
		/// <returns></returns>
		public string HashPassword(string plainPassword)
		{
			using (SHA256 sha256 = SHA256.Create())
			{
				byte[] bytes = Encoding.UTF8.GetBytes(plainPassword);
				byte[] hashBytes = sha256.ComputeHash(bytes);
				return Convert.ToBase64String(hashBytes);
			}
		}
		/// <summary>
		/// Generate next UserId for students (with major)
		/// </summary>
		/// <param name="majorId"></param>
		/// <returns></returns>
		private string GenerateNextUserId(string majorId)
		{
			//Select a list of User with the same MajorId
			var allUsers = _userRepository.GetAllUser();
			var filteredUsers = allUsers.Where(u => u.UserId.StartsWith(majorId)).ToList();
			if (filteredUsers.Count == 0)
			{
				return majorId + "0001";
			}
			//Get the max number in UserId of the list
			int maxNumber = filteredUsers
				.Select(u => int.Parse(u.UserId.Substring(majorId.Length)))
				.Max();
			int nextNumber = maxNumber + 1;
			return majorId + nextNumber.ToString("D4"); // Format as MajorId_xxxx
		}
		/// <summary>
		/// Generate next UserId without major (for other roles)
		/// </summary>
		/// <param name="roleId"></param>
		/// <returns></returns>
		private string GenerateNextUserIdWithoutMajor(int roleId)
		{
			//Select a list of User with the same RoleId
			var allUsers = _userRepository.GetAllUser();
			var filteredUsers = allUsers.Where(u => u.UserId.StartsWith(roleId.ToString() + "_")).ToList();
			if (filteredUsers.Count == 0)
			{
				return $"{roleId}_0001";
			}
			int maxNumber = filteredUsers
				.Select(u => int.Parse(u.UserId.Split('_')[1])) // Split by "_" to get the numeric part
				.Max();
			int nextNumber = maxNumber + 1;
			return $"{roleId}_{nextNumber.ToString("D4")}"; // Format as RoleId_xxxx
		}
		/// <summary>
		/// generate email like the format of FPT Email
		/// </summary>
		/// <param name="first"></param>
		/// <param name="mid"></param>
		/// <param name="last"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		private string GenerateEmail(string first, string mid, string last, string userId)
		{
			//Take the first letter of lastname
			char firstCharacterLastName = last[0];
			//Take the first letter of middlename
			char firstCharacterMidName = mid[0];
			return $"{first}{firstCharacterLastName}{firstCharacterMidName}{userId}@gmail.com";
		}
		/// <summary>
		/// Add new a user
		/// </summary>
		/// <param name="addUserDTO"></param>
		/// <returns></returns>
		public APIResponse AddUser(AddUserDTO addUserDTO)
		{
			string roleName = addUserDTO.RoleId switch
			{
				1 => "Admin",
				2 => "Academic Staff",
				3 => "Chairperson",
				4 => "Lecturer",
				5 => "Student",
				_ => null
			};
			if (roleName == null)
			{
				return new APIResponse
				{
					IsSuccess = false,
					Message = "Invalid RoleId."
				};
			}
			// Require MajorID for Student
			if (addUserDTO.RoleId == 5 && string.IsNullOrEmpty(addUserDTO.MajorId))
			{
				return new APIResponse
				{
					IsSuccess = false,
					Message = "MajorId is required for RoleId = 5 (Student)."
				};
			}
			// Validate Email
			if (!IsValidEmail(addUserDTO.PersonalEmail))
			{
				return new APIResponse
				{
					IsSuccess = false,
					Message = "Invalid email format."
				};
			}
			// Validate PhoneNumber
			if (!IsValidPhoneNumber(addUserDTO.PhoneNumber))
			{
				return new APIResponse
				{
					IsSuccess = false,
					Message = "Invalid phone number format."
				};
			}
			string genUserId = addUserDTO.RoleId == 5 ? GenerateNextUserId(addUserDTO.MajorId) : GenerateNextUserIdWithoutMajor(addUserDTO.RoleId);
			var userDTO = new UserDTO
			{
				UserId = genUserId,
				FirstName = addUserDTO.FirstName,
				MiddleName = addUserDTO.MiddleName,
				LastName = addUserDTO.LastName,
				PasswordHash = HashPassword(addUserDTO.PasswordHash),
				PersonalEmail = addUserDTO.PersonalEmail,
				Email = GenerateEmail(addUserDTO.FirstName, addUserDTO.MiddleName, addUserDTO.LastName, genUserId),
				PhoneNumber = addUserDTO.PhoneNumber,
				RoleId = addUserDTO.RoleId,
				RoleName = roleName,
				MajorId = addUserDTO.RoleId == 5 ? addUserDTO.MajorId : null,
				Status = 1,
			};
			UserDTO existingUser = _userRepository.GetUserById(userDTO.UserId);
			if (existingUser != null)
			{
				return new APIResponse
				{
					IsSuccess = false,
					Message = "User with the given UserId already exists."
				};
			}
			bool isAdded = _userRepository.AddNewUser(userDTO);
			if (isAdded)
			{
				return new APIResponse
				{
					IsSuccess = true,
					Message = "User added successfully."
				};
			}
			return new APIResponse
			{
				IsSuccess = false,
				Message = "Failed to add User."
			};
		}
		//Validate Email Format
		private bool IsValidEmail(string email)
		{
			try
			{
				var addr = new System.Net.Mail.MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}
		//Validate Phonenumber Format
		private bool IsValidPhoneNumber(string phoneNumber)
		{
			return phoneNumber.All(char.IsDigit) && phoneNumber.Length >= 10 && phoneNumber.Length <= 15 && phoneNumber.StartsWith("0");
		}
		/// <summary>
		/// Get all user
		/// </summary>
		/// <returns></returns>
		public APIResponse GetAllUser()
		{
			APIResponse aPIResponse = new APIResponse();
			List<UserDTO> users = _userRepository.GetAllUser();
			if (users == null || users.Count == 0)
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "No users found.";
			}
			else
			{
				aPIResponse.IsSuccess = true;
				aPIResponse.Result = users;
			}
			return aPIResponse;
		}
		/// <summary>
		/// admin Update personal email and phone number for student
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="updateUser"></param>
		/// <returns></returns>
		public APIResponse UpdateUser(string userId, UpdateUserDTO updateUser)
		{
			UserDTO existingUser = _userRepository.GetUserById(userId);
			if (existingUser == null)
			{
				return new APIResponse
				{
					IsSuccess = false,
					Message = "User not found."
				};
			}
			if (!string.IsNullOrEmpty(updateUser.PersonalEmail) && !IsValidEmail(updateUser.PersonalEmail))
			{
				return new APIResponse
				{
					IsSuccess = false,
					Message = "Invalid email format."
				};
			}
			if (!string.IsNullOrEmpty(updateUser.PhoneNumber) && !IsValidPhoneNumber(updateUser.PhoneNumber))
			{
				return new APIResponse
				{
					IsSuccess = false,
					Message = "Invalid phone number format."
				};
			}
			existingUser.PersonalEmail = updateUser.PersonalEmail ?? existingUser.PersonalEmail;
			existingUser.PhoneNumber = updateUser.PhoneNumber ?? existingUser.PhoneNumber;
			bool isUpdated = _userRepository.UpdateUser(existingUser);
			if (isUpdated)
			{
				return new APIResponse
				{
					IsSuccess = true,
					Message = "User updated successfully."
				};
			}
			return new APIResponse
			{
				IsSuccess = false,
				Message = "Failed to update User."
			};
		}
		/// <summary>
		/// Get User by Id
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public APIResponse GetUserById(string userId)
		{
			APIResponse aPIResponse = new APIResponse();
			UserDTO user = _userRepository.GetUserById(userId);
			if (user == null)
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "User not found.";
			}
			else
			{
				aPIResponse.IsSuccess = true;
				aPIResponse.Result = user;
			}
			return aPIResponse;
		}
		/// <summary>
		/// Change status of a student
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="status"></param>
		/// <returns></returns>
		public APIResponse UpdateStudentStatus(string userId, int status)
		{
			APIResponse aPIResponse = new APIResponse();
			// Validate status input
			if (status < 0 || status > 3)
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "Invalid status. Status must be between 0 and 3.";
				return aPIResponse;
			}
			UserDTO user = _userRepository.GetUserById(userId);
			// Check if the user exists
			if (user == null)
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "User not found.";
				return aPIResponse;
			}
			// Check if the user is a student
			if (user.RoleId != 5)
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "Only students can have this status changed.";
				return aPIResponse;
			}
			// Update the student's status
			bool isUpdated = _userRepository.UpdateStudentStatus(userId, status);
			if (isUpdated)
			{
				aPIResponse.IsSuccess = true;
				// Provide the message based on the status value
				switch (status)
				{
					case 0:
						aPIResponse.Message = "Disable student successfully.";
						break;
					case 1:
						aPIResponse.Message = "Set student on schedule successfully.";
						break;
					case 2:
						aPIResponse.Message = "Set student deferment successfully.";
						break;
					case 3:
						aPIResponse.Message = "Set student graduated successfully.";
						break;
				}
			}
			else
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "Failed to update student status.";
			}
			return aPIResponse;
		}
		/// <summary>
		/// User Sefl-update their infor
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="updateInfor"></param>
		/// <returns></returns>
		public APIResponse UpdateInfor(string userId, UpdateInforDTO updateInfor)
		{
			UserDTO existingUser = _userRepository.GetUserById(userId);
			// Check if the user exists
			if (existingUser == null)
			{
				return new APIResponse
				{
					IsSuccess = false,
					Message = "User not found."
				};
			}
			// Check valid Email
			if (!string.IsNullOrEmpty(updateInfor.PersonalEmail) && !IsValidEmail(updateInfor.PersonalEmail))
			{
				return new APIResponse
				{
					IsSuccess = false,
					Message = "Invalid email format."
				};
			}
			// Check valid PhoneNumber
			if (!string.IsNullOrEmpty(updateInfor.PhoneNumber) && !IsValidPhoneNumber(updateInfor.PhoneNumber))
			{
				return new APIResponse
				{
					IsSuccess = false,
					Message = "Invalid phone number format."
				};
			}
			existingUser.FirstName = updateInfor.FirstName ?? existingUser.FirstName;
			existingUser.MiddleName = updateInfor.MiddleName ?? existingUser.MiddleName;
			existingUser.LastName = updateInfor.LastName ?? existingUser.LastName;
			existingUser.UserAvartar = updateInfor.UserAvartar ?? existingUser.UserAvartar;
			existingUser.PersonalEmail = updateInfor.PersonalEmail ?? existingUser.PersonalEmail;
			existingUser.PhoneNumber = updateInfor.PhoneNumber ?? existingUser.PhoneNumber;
			bool isUpdated = _userRepository.UpdateInfor(existingUser);
			if (isUpdated)
			{
				return new APIResponse
				{
					IsSuccess = true,
					Message = "User updated successfully."
				};
			}
			return new APIResponse
			{
				IsSuccess = false,
				Message = "Failed to update User."
			};
		}
		/// <summary>
		/// Forgot password send OTP
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		public APIResponse ForgotPassword(string email)
		{
			APIResponse aPIResponse = new APIResponse();
			bool isExist = _userRepository.CheckUserByEmail(email);

			if (!isExist)
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "Email is not exist.";
				return aPIResponse;
			}

			try
			{
				string fromEmail = "thainam26903@gmail.com";
				string appPassword = "rzsd encn mqbj dwfy";

				ResourceManager rm = new ResourceManager("UserService.Resources.OTP", Assembly.GetExecutingAssembly());
				string template = rm.GetString("OTPTemplate");

				string otp = GenerateOTP();
				string emailBody = template.Replace("@paramOTP", otp);

				MailMessage mail = new MailMessage();
				mail.From = new MailAddress(fromEmail);
				mail.To.Add(email);
				mail.Subject = $"Mã OTP của bạn là: {otp}";
				mail.Body = emailBody;
				mail.IsBodyHtml = true;

				SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
				smtp.Credentials = new NetworkCredential(fromEmail, appPassword);
				smtp.EnableSsl = true;

				smtp.Send(mail);
			}
			catch (Exception ex)
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "Failed to send OTP: " + ex.Message;
			}

			return aPIResponse;
		}
		/// <summary>
		/// Generate OTP to reset password
		/// </summary>
		/// <returns></returns>
		private string GenerateOTP()
		{
			Random random = new Random();
			return random.Next(0, 999999).ToString("D6");
		}
		/// <summary>
		/// Reset password
		/// </summary>
		/// <param name="resetPassword"></param>
		/// <returns></returns>
		public APIResponse ResetPassword(ResetPasswordDTO resetPassword)
		{
			APIResponse aPIResponse = new APIResponse();
			bool isReset = _userRepository.ResetPassword(resetPassword);
			if (isReset)
			{
				aPIResponse.IsSuccess = true;
				aPIResponse.Message = "Password reset successfully.";
			}
			else
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "Failed to reset password.";
			}
			return aPIResponse;
		}
	}
}
