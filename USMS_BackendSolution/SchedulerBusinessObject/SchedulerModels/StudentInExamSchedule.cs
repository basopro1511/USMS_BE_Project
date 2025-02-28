using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SchedulerBusinessObject.SchedulerModels
    {
    public class StudentInExamSchedule
        {
        [Key]
        [Column(TypeName = "INT")]
        public int StudentExamId { get; set; }
        [Column(TypeName = "INT")]
        public int ExamScheduleId { get; set; }
        [Column(TypeName = "NVARCHAR(16)")]
        public string StudentId { get; set; }
        }
    }
