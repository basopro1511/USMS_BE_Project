using System.ComponentModel.DataAnnotations;

namespace SchedulerBusinessObject.ModelDTOs
{
	public class AssignTeacherDTO
	{
		[Required]
		public int ScheduleId { get; set; }

		[StringLength(8)]
		[Required]
		public string UserId { get; set; } = null!;
	}
}
