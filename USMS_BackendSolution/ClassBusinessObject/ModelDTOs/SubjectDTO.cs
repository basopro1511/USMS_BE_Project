using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassBusinessObject.ModelDTOs
{
	public class SubjectDTO
	{
		[StringLength(10)]
		[Column(TypeName = "nvarchar(10)")]
		public string SubjectId { get; set; }

		[Required]
		[StringLength(100)]
		[Column(TypeName = "nvarchar(100)")]
		public string SubjectName { get; set; }

		[Required]
		public int NumberOfSlot { get; set; }

		[StringLength(200)]
		[Column(TypeName = "nvarchar(200)")]
		public string Description { get; set; }

		public bool Status { get; set; }
	}
}
