using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject.Models
{
	public class Role
	{
		[Key]
		[Required]
		public int RoleId { get; set; }
		[Required]
		[Column(TypeName = "NVARCHAR(50)")]
		public string RoleName { get; set; } = null!;
		public ICollection<User>? Users { get; set; }
	}

}
