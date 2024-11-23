using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerBusinessObject.SchedulerModels
{
    public class Rooms
    {
        [Key]
        [StringLength(6)] 
        [Column(TypeName = "NVARCHAR(6)")]
        public string RoomId { get; set; }
        [Column(TypeName = "NVARCHAR(100)")]
        public string? Location { get; set; }
        public bool isOnline { get; set; }
        public string? OnlineURL { get; set; }
        public int Status   { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        public virtual ICollection<Schedules> Schedules { get; set; } = new List<Schedules>();
    }
}
