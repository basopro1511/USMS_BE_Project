using System.ComponentModel.DataAnnotations;

namespace SchedulerBusinessObject.ModelDTOs
{
	// Này mục đích chính là thành list để trả về cho client
	public class RequestDTO
	{
		public int RequestId { get; set; }
		public string RequestTeacher { get; set; } = null!;
		public int ScheduleId { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public int Status { get; set; }
	}
	// Này mục đích để xem detail	
	public class ViewRequestDTO
	{
		public int RequestId { get; set; }
		public string RequestTeacher { get; set; } = null!;
		public int ScheduleId { get; set; }
		public bool IsChangeTeacher { get; set; }
		public string? AlternateTeacher { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }
		public int Status { get; set; }
	}
	// Này có cần tái sử dụng lại ViewRequest ở trên không?
	public class CreateRequestDTO
	{
		[Required]
		public string RequestTeacher { get; set; } = null!;
		[Required]
		public int ScheduleId { get; set; }
		[Required]
		public bool IsChangeTeacher { get; set; }
		public string? AlternateTeacher { get; set; }
	}
	public class UpdateStatusRequestDTO
	{
		[Required]
		public int RequestId { get; set; }
		[Required]
		public int Status { get; set; }
	}
}
