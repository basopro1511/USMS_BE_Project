namespace SchedulerBusinessObject.ModelDTOs
{
	public class ScheduleDTO
	{
		public int ClassScheduleId { get; set; }
		public int ClassSubjectId { get; set; }
		public int SlotId { get; set; }
		public string RoomId { get; set; }
		public string? TeacherId { get; set; }
		public DateOnly Date { get; set; }
		public int Status { get; set; }
		public int SlotNoInSubject { get; set; }
	}

	public class ViewScheduleDTO
	{
		public int ClassScheduleId { get; set; }
		public int ClassSubjectId { get; set; }
		public string ClassId { get; set; }
		public string SubjectId { get; set; }
		public string? MajorId { get; set; }
		public int SlotId { get; set; }

		public string RoomId { get; set; }
		public string? TeacherId { get; set; }
		public DateOnly Date { get; set; }
		public int SlotNoInSubject { get; set; }
		public int Status { get; set; }
	}
	public class ViewScheduleDetailsDTO
	{
		public DateOnly Date { get; set; }
		public TimeOnly startTime { get; set; }
		public TimeOnly endTime { get; set; }
		public string ClassName { get; set; } = null!;
		public string SubjectName { get; set; } = null!;
		public string? Teacher { get; set; }
		public int SlotNoInSubject { get; set; }
	}
}
