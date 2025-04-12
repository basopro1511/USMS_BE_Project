using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerBusinessObject.SchedulerModels
    {
    public class RequestSchedule
        {
        [Key]
        public int RequestId { get; set; }
        [Required]
        [Column(TypeName = "NVARCHAR(16)")]
        public string UserId { get; set; }
        [Required]
        public int RequestType { get; set; }
        [Required]
        public int ScheduleId { get; set; }
        [Column(TypeName = "NVARCHAR(16)")]
        public string? AlternativeTeacher { get; set; }

        [Column(TypeName = "DATE")]
        public DateOnly OriginalDate { get; set; }
        [Required]
        public int OriginalSlotId { get; set; }
        [Column(TypeName = "NVARCHAR(6)")]
        public string OriginalRoomId { get; set; }
        [Column(TypeName = "DATE")]
        public DateOnly? NewDate { get; set; }
        public int? NewSlotId { get; set; }

        [Column(TypeName = "NVARCHAR(6)")]
        public string? NewRoomId { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? Reason { get; set; }

        [Column(TypeName = "NVARCHAR(MAX)")]
        public string? ReplyResponse { get; set; }
        public int Status { get; set; }

        public DateTime RequestDate { get; set; } = DateTime.Now;
        }
    }
