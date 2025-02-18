namespace SchedulerBusinessObject.ModelDTOs
{
	public class UserDTO
	{
		public string UserId { get; set; } = null!;
		public string FirstName { get; set; } = null!;
		public string? MiddleName { get; set; }
		public string LastName { get; set; } = null!;
	}
}
