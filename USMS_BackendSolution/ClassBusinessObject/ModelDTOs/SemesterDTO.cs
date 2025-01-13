using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerBusinessObject.ModelDTOs
{
    public class SemesterDTO
    {
        public string SemesterId { get; set; }
        public string SemesterName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int Status { get; set; }
    }
    public class StatusSemesterDTO
    {
        public string SemesterId { get; set; }
        public int Status { get; set; }
    }

}
