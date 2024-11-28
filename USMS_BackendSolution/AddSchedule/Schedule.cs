namespace AddSchedule
{
	public class Schedule
	{
		public int SubjectID { get; set; }
		public int SlotID { get; set; }
		public bool IsOnline { get; set; }
		public int? RoomID { get; set; }
		public string? URL { get; set; }
		public string? Teacher { get; set; }
		public DateOnly Date { get; set; }
		public int Status { get; set; }
	}
}
