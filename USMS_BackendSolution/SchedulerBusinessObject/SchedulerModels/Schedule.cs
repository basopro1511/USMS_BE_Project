using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchedulerBusinessObject.SchedulerModels
{
	public class Schedule
    {
        [Key]
        public int ScheduleId { get; set; }
        [Required]
        [Column(TypeName = "INT")]
        public int ClassSubjectId { get; set; }
        [Required]
        [ForeignKey("Slot")]
        [Column(TypeName = "INT")]
        public int SlotId { get; set; }
        [Required]
        [ForeignKey("Room")]
        [Column(TypeName = "NVARCHAR(6)")]
        public string RoomId { get; set; }
        [StringLength(8)]
        [Column(TypeName = "NVARCHAR(8)")]
        public string? TeacherId { get; set; }
        [Required]
        [Column(TypeName = "DATE")]
        public DateOnly Date { get; set; } 
        [Required]
        [Column(TypeName = "INT")]
        public int Status { get; set; }
        public int SlotNoInSubject { get; set; }
        public virtual TimeSlot? Slot { get; set; }
        public virtual Room? Room { get; set; }
		// 1 lịch thì chỉ có 1 request là đổi giáo viên thôi chứ, chứ kiểu đổi luôn lịch là xóa rồi add lại mà đâu liên quan gì cái schedule này nữa
		public virtual RequestSchedule? RequestSchedule { get; set; }
	}
}
