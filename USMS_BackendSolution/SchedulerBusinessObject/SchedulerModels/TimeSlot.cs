using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SchedulerBusinessObject.SchedulerModels
{
	public class TimeSlot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(TypeName = "INT")]
        public int SlotId { get; set; }
        [Required]
        [Column(TypeName = "TIME(0)")]
        public TimeOnly StartTime { get; set; }
        [Required]
        [Column(TypeName = "TIME(0)")]
        public TimeOnly EndTime { get; set; }
        [Required]
        [Column(TypeName = "INT")]
        public int Status { get; set; }
        [JsonIgnore]
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
