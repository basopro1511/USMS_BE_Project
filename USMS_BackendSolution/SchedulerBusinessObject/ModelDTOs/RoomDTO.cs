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
        public bool isOnline { get; set; }
        public string? OnlineURL { get; set; }
        public int Status { get; set; }
        public string UpdateAtFormatted => UpdateAt.ToString("dd/MM/yyyy HH:mm");
        public string CreateAtFormatted => CreateAt.ToString("dd/MM/yyyy HH:mm");
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
