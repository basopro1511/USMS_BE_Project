using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        [StringLength(6)]
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

        [JsonIgnore]
        public virtual TimeSlot Slot { get; set; }
        [JsonIgnore]
        public virtual Room? Room { get; set; }

    }
}
