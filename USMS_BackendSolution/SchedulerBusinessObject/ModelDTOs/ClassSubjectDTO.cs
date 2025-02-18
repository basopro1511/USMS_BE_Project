namespace SchedulerBusinessObject.ModelDTOs
{
	public class ClassSubjectDTO
	{
		public int ClassSubjectId { get; set; }

		public string ClassId { get; set; } = null!;

		public string SubjectId { get; set; } = null!;

		public string SemesterId { get; set; } = null!;

		public int Status { get; set; }
	}
}
