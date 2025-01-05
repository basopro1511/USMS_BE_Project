using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerBusinessObject.ModelDTOs
{
    public class SubjectDTO
    {
        public string SubjectId { get; set; }
        public string SubjectName { get; set; }
        public int NumberOfSlot { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
    }
}
