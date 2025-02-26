using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BusinessObject.Models
{
	public class Student
	{
		[Key]
		[Column(TypeName = "VARCHAR(8)")]
		[ForeignKey("User")]
		public string StudentId { get; set; } = null!;
		[ForeignKey("Major")]
		[Column(TypeName = "NVARCHAR(4)")]
		public string? MajorId { get; set; }
		[Column(TypeName = "INT")]
		public int Term { get; set; }
		[JsonIgnore]
		public Major? Major { get; set; }
		public virtual User? User { get; set; }
	}
}
