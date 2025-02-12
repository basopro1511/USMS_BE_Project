using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class User
    {
        [Key]
        [Required]
        [StringLength(8)]
        [Column(TypeName = "NVARCHAR(8)")]
        public string UserId { get; set; }
        [Required]
        [StringLength(20)]
        [Column(TypeName = "NVARCHAR(20)")]
        public string FirstName { get; set; }
        [Required]
        [StringLength(20)]
        [Column(TypeName = "NVARCHAR(20)")]
        public string MiddleName { get; set; }
        [Required]
        [StringLength(20)]
        [Column(TypeName = "NVARCHAR(20)")]
        public string LastName { get; set; }
        [Required]
        [StringLength(200)]
        [Column(TypeName = "NVARCHAR(200)")]
        public string PasswordHash { get; set; }
        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR(100)")]
        public string Email { get; set; }
        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR(100)")]
        public string PersonalEmail { get; set; }
        [Required]
        [StringLength(15)]
        [Column(TypeName = "NVARCHAR(15)")]
        public string PhoneNumber { get; set; }
        [Required]
        [Column(TypeName = "DATE")]
        public DateTime DateOfBirth { get; set; }
        [Required]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string UserAvatar { get; set; }
        [Required]
        [Column(TypeName = "INT")]
        public int RoleId { get; set; }
        [Required]
        [Column(TypeName = "INT")]
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [JsonIgnore]
        [ForeignKey(nameof(RoleId))]
        public virtual Role? Role { get; set; }
    }
}
