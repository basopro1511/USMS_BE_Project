using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerBusinessObject.ModelDTOs
    {
    public class StudentInExamScheduleDTO
        {
        public int StudentExamId { get; set; }
        public int ExamScheduleId { get; set; }
        public string StudentId { get; set; }
        }
    }
