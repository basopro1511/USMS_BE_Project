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
    public class TimeSlot
    {
        [Key]
        [Column(TypeName = "INT")]
        public int SlotId { get; set; }
        [Required]
        [Column(TypeName = "TIME(7)")]
        public TimeOnly StartTime { get; set; }
        [Required]
        [Column(TypeName = "TIME(7)")]
        public TimeOnly EndTime { get; set; }
        [JsonIgnore]
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
