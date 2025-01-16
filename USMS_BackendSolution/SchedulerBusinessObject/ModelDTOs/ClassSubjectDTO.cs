using System.ComponentModel.DataAnnotations;

namespace SchedulerBusinessObject.ModelDTOs
{
	public class ClassSubjectDTO
	{
		public int ClassSubjectId { get; set; }

		[StringLength(10)]
		public string ClassId { get; set; } = null!;

		[StringLength(10)]
		public string SubjectId { get; set; } = null!;

		[StringLength(4)]
		public string SemesterId { get; set; } = null!;

		public bool Status { get; set; }
	}
}
