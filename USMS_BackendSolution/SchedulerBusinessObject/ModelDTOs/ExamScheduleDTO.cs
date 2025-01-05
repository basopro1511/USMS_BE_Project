using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerBusinessObject.ModelDTOs
{
    public class ExamScheduleDTO
    {
        public int ExamScheduleId { get; set; }
        public string SubjectId { get; set; }
        public string SemesterId { get; set; }
        public string RoomId { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string TeacherId { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
