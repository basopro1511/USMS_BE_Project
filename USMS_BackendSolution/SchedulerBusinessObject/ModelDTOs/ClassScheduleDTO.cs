using System.ComponentModel.DataAnnotations;

namespace SchedulerBusinessObject.ModelDTOs
{
	public class ClassScheduleDTO
	{
		[Required]
		public DateOnly Date { get; set; }

		[Required]
		public int ClassSubjectId { get; set; }

		[Required]
		public string RoomId { get; set; } = null!;

		[Required]
		public int SlotId { get; set; }

        public int SlotNoInSubject { get; set; }

        }
    }
