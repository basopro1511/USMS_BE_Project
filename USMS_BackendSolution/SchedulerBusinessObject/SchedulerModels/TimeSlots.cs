using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchedulerBusinessObject.SchedulerModels
{
    public class TimeSlots
    {
        [Key]
        public int SlotId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        [JsonIgnore]
        public virtual ICollection<Schedules> Schedules { get; set; } = new List<Schedules>();
    }
}
