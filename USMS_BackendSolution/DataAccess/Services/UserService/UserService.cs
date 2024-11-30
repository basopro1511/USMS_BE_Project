using BusinessObject;
using BusinessObject.ModelDTOs;
using DataAccess.Repository.UserRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace DataAccess.Services.UserService
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService()
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
            var allUsers = _userRepository.GetAllUser();
            var filteredUsers = allUsers.Where(u => u.UserId.StartsWith(majorId)).ToList();

            if (filteredUsers.Count == 0)
            {
                return majorId + "0001";
            }
            int maxNumber = filteredUsers
                .Select(u => int.Parse(u.UserId.Substring(majorId.Length)))
                .Max();
            int nextNumber = maxNumber + 1;

            return majorId + nextNumber.ToString("D4");
        }

        /// <summary>
        /// Generate next UserId without major (for other roles)
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        private string GenerateNextUserIdWithoutMajor(int roleId)
        {
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
        /// generate email theo format truong
        /// </summary>
        /// <param name="first"></param>
        /// <param name="mid"></param>
        /// <param name="last"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private string GenerateEmail(string first, string mid, string last, string userId)
        {
            char firstCharacterFirstName = first[0];
            char firstCharacterMidName = mid[0];

            return $"{last}{firstCharacterFirstName}{firstCharacterMidName}{userId}@gmail.com";
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
            if (addUserDTO.RoleId == 5 && string.IsNullOrEmpty(addUserDTO.MajorId))
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "MajorId is required for RoleId = 5 (Student)."
                };
            }
            if (!IsValidEmail(addUserDTO.PersonalEmail))
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Invalid email format."
                };
            }
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
                Status = 1
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
        /// Disable a student
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public APIResponse DisableStudent(string userId)
        {
            APIResponse aPIResponse = new APIResponse();
            UserDTO user = _userRepository.GetUserById(userId);
            if (user == null)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "User not found.";
                return aPIResponse;
            }
            if (user.RoleId != 5)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Only students can have this status changed.";
                return aPIResponse;
            }
            bool isUpdated = _userRepository.DisableStudent(userId);
            if (isUpdated)
            {
                aPIResponse.IsSuccess = true;
                aPIResponse.Message = "Student disabled successfully.";
            }
            else
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Failed to disable student.";
            }
            return aPIResponse;
        }
        /// <summary>
        /// Mark a student On schedule
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public APIResponse OnScheduleStudent(string userId)
        {
            APIResponse aPIResponse = new APIResponse();
            UserDTO user = _userRepository.GetUserById(userId);
            if (user == null)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "User not found.";
                return aPIResponse;
            }
            if (user.RoleId != 5)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Only students can have this status changed.";
                return aPIResponse;
            }
            bool isUpdated = _userRepository.OnScheduleStudent(userId);
            if (isUpdated)
            {
                aPIResponse.IsSuccess = true;
                aPIResponse.Message = "Student marked as on schedule successfully.";
            }
            else
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Failed to update student status to on schedule.";
            }
            return aPIResponse;
        }
        /// <summary>
        /// Mark a student Deferement
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public APIResponse DefermentStudent(string userId)
        {
            APIResponse aPIResponse = new APIResponse();
            UserDTO user = _userRepository.GetUserById(userId);
            if (user == null)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "User not found.";
                return aPIResponse;
            }
            if (user.RoleId != 5)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Only students can have this status changed.";
                return aPIResponse;
            }
            bool isUpdated = _userRepository.DefermentStudent(userId);
            if (isUpdated)
            {
                aPIResponse.IsSuccess = true;
                aPIResponse.Message = "Student deferment status updated successfully.";
            }
            else
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Failed to update student deferment status.";
            }
            return aPIResponse;
        }
        /// <summary>
        /// Mark a student Graduated
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public APIResponse GraduatedStudent(string userId)
        {
            APIResponse aPIResponse = new APIResponse();
            UserDTO user = _userRepository.GetUserById(userId);
            if (user == null)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "User not found.";
                return aPIResponse;
            }
            if (user.RoleId != 5)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Only students can have this status changed.";
                return aPIResponse;
            }
            bool isUpdated = _userRepository.GraduatedStudent(userId);
            if (isUpdated)
            {
                aPIResponse.IsSuccess = true;
                aPIResponse.Message = "Student graduated status updated successfully.";
            }
            else
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Failed to update student graduated status.";
            }
            return aPIResponse;
        }
    }
}
