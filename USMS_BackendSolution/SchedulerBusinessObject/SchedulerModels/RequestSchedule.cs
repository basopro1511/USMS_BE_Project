using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchedulerBusinessObject.SchedulerModels
{
	[Table("RequestSchedule")]
	public class RequestSchedule
	{
		[Key]
		[Required]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		// Không để string tại vì request được tạo ra quá dễ đi, đã vậy cancel cũng không xóa => gây tốn bộ nhớ, xử lý dư thừa
		public int RequestId { get; set; }
		[Required]
		[Column(TypeName = ("VARCHAR(8)"))]
		// Nvarchar tốn 2 byte, varchar tốn 1 byte
		// Thành ra cái nào không cần hỗ trợ unicode(có dấu) thì dùng varchar
		// Thành ra nữa tao sửa mấy cái thành varchar
		public string RequestTeacher { get; set; } = null!;
		[Required]
		[Column(TypeName = ("INT"))]
		public int ScheduleId { get; set; }
		public Schedule Schedule { get; set; } = null!;
		[Required]
		// 1 for Alternate Teacher, 2 for Change Schedule
		// Nếu chỉ có 2 loại request thì dùng bool
		// String tốn 8 byte, bool tốn 1 byte, int tốn 4 byte
		// Xem request có phải là thay đổi giáo viên không?
		public bool IsChangeTeacher { get; set; }
		// ID của giáo viên thay thế
		public string? AlternateTeacher { get; set; }
		[Required]
		public DateTime CreatedAt { get; set; }
		[Required]
		public DateTime UpdatedAt { get; set; }
		[Required]
		// 1 for Pending, 2 for Approved, 3 for Rejected, 4 for Cancelled
		public int Status { get; set; }
	}
}
