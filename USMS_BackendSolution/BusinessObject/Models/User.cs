using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models
{
	public class User
	{
		[Key]
		[Column(TypeName = "VARCHAR(8)")]
		public string UserId { get; set; } = null!;
		[Column(TypeName = "NVARCHAR(20)")]
		[Required]
		public string FirstName { get; set; } = null!;
		[Column(TypeName = "NVARCHAR(20)")]
		public string? MiddleName { get; set; }
		[Column(TypeName = "NVARCHAR(20)")]
		[Required]
		public string LastName { get; set; } = null!;
		[Column(TypeName = "VARCHAR(200)")]
		[Required]
		public string PasswordHash { get; set; } = null!;
		[Column(TypeName = "VARCHAR(100)")]
		public string? Email { get; set; }
		[Column(TypeName = "VARCHAR(100)")]
		[Required]
		public string PersonalEmail { get; set; } = null!;
		[Column(TypeName = "NVARCHAR(MAX)")]
		[Required]
		public string Address { get; set; } = null!;

		[Required]
		[Column(TypeName = "VARCHAR(15)")]
		public string PhoneNumber { get; set; } = null!;
		[Required]
		public DateOnly DateOfBirth { get; set; }
		[Column(TypeName = "NVARCHAR(MAX)")]
		public string? UserAvartar { get; set; }
		[ForeignKey("Role")]
		public int RoleId { get; set; }
		public Role Role { get; set; } = null!;
		[ForeignKey("Major")]
		public string? MajorId { get; set; }
		public Major? Major { get; set; }
		public int Status { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public virtual Student Student { get; set; } = null!;
	}
}
