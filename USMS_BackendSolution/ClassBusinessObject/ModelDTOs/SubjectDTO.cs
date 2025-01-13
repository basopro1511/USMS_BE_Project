using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassBusinessObject.ModelDTOs
{
	public class SubjectDTO
	{
		[Key]
		[StringLength(10)]
		[Column(TypeName = "nvarchar(10)")]
		public string SubjectId { get; set; }

		[Required]
		[StringLength(100)]
		[Column(TypeName = "nvarchar(100)")]
		public string SubjectName { get; set; }

        [StringLength(4)]
        [Column(TypeName = "nvarchar(4)")]
        public string MajorId { get; set; }

        [Required]
		public int NumberOfSlot { get; set; }

		[StringLength(200)]
		[Column(TypeName = "nvarchar(200)")]
		public string Description { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int Term { get; set; }

        public int Status { get; set; }
	}
}
