using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObject.ModelDTOs
{
    public class StudentDTO
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public bool Gender { get; set; }
        public string PasswordHash { get; set; }
        public string? Email { get; set; }
        public string PersonalEmail { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public string? UserAvartar { get; set; }
        public int RoleId { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string MajorId { get; set; }
        public int Term { get; set; }
        public string RoleName { get; set; }
        public string MajorName { get; set; }
        public string Address { get; set; }
    }
    public class StudentTableDTO
    {
        public string StudentId { get; set; }
        public string? MajorId { get; set; }
        public int Term { get; set; }
        }
}
