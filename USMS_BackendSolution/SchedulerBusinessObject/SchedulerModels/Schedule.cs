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
        [ForeignKey("Room")]
        [Column(TypeName = "NVARCHAR(6)")]
        public string RoomId { get; set; }
        [Column(TypeName = "NVARCHAR(16)")]
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
        [JsonIgnore]
        public virtual ICollection<RequestSchedule> RequestSchedules { get; set; }
            = new List<RequestSchedule>();
        }
    }
