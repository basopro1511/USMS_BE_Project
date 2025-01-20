using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class User
    {
        [Key]
        [Column(TypeName = "NVARCHAR(8)")]
        public string UserId { get; set; }

        [Column(TypeName = "NVARCHAR(20)")]
        [Required]
        public string FirstName { get; set; }
        [Column(TypeName = "NVARCHAR(20)")]
        public string? MiddleName { get; set; }
        [Column(TypeName = "NVARCHAR(20)")]
        [Required]
        public string LastName { get; set; }
        [Column(TypeName = "NVARCHAR(200)")]
        [Required]
        public string PasswordHash { get; set; }
        [Column(TypeName = "NVARCHAR(100)")]
        public string? Email { get; set; }
        [Column(TypeName = "NVARCHAR(100)")]
        [Required]
        public string PersonalEmail { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(15)")]
        public string PhoneNumber { get; set; }

        [Required]
        public DateOnly DateOfBirth { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? UserAvartar { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }

        public Role Role { get; set; }
        [ForeignKey("Major")]
        public string? MajorId { get; set; }
        public Major Major { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
