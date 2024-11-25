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
        public bool Status { get; set; }

        // Optionally, add derived properties or custom formatting if needed
        public string FullSemesterName => $"{SemesterName} ({StartDate.ToString("yyyy-MM-dd")} - {EndDate.ToString("yyyy-MM-dd")})";
    }
}
