using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerBusinessObject.SchedulerModels
{
    public class Room
    {
        [Key]
        [Column(TypeName = "NVARCHAR(6)")]
        public string RoomId { get; set; }
        [Column(TypeName = "NVARCHAR(100)")]
        public string? Location { get; set; }
        [Column(TypeName = "INT")]
        public int Status   { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
