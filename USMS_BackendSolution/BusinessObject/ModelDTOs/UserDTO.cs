using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.ModelDTOs
{
    public class UserDTO
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string PersonalEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string? UserAvartar { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public int RoleId { get; set; }
        public string MajorId { get; set; }
        public int Status { get; set; }
        public string Address { get; set; }
        public int Term { get; set; }
        }

    public class AddUserDTO
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string MajorId { get; set; }
        public string FullUserName
        {
            get
            {
                return $"{FirstName} {MiddleName} {LastName}";
            }
        }
        public string PasswordHash { get; set; }
        public string? UserAvartar { get; set; }
        public DateOnly DateOfBirth { get; set; }

        public string PersonalEmail { get; set; }
        public string PhoneNumber { get; set; }
        public int RoleId { get; set; }
        public int Status { get; set; }
        public string? Address { get; set; }
    }

    public class UpdateUserDTO
    {
        public string PersonalEmail { get; set; }
        public string PhoneNumber { get; set; }
    }
    public class UpdateInforDTO
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string UserAvartar { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string PersonalEmail { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
    //public class UpdateStudentStatusDTO
    //{
    //    public int Status { get; set; }
    //}
}
