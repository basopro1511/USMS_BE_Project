using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerBusinessObject.ModelDTOs
{
    public class RoomDTO
    {
        public string RoomId { get; set; } 
        public string? Location { get; set; } 
        public bool IsOnline { get; set; } 
        public string? OnlineURL { get; set; }
        public int Status { get; set; } 
        public DateTime CreateAt { get; set; } 
        public DateTime UpdateAt { get; set; }
    }

}
