using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerBusinessObject.SchedulerModels
{
    public class ExamSchedule
    {
        [Key]
        public int ExamScheduleId { get; set; }

        [Required]
        [Column(TypeName = "NVARCHAR(4)")]
        public string SemesterId { get; set; }

        [Column(TypeName ="NVARCHAR(4)")]
        public string? MajorId { get; set; }
        
        [Required]
        [Column(TypeName = "NVARCHAR(10)")]
        public string SubjectId { get; set; }

        [Column(TypeName = "NVARCHAR(6)")]
        public string? RoomId { get; set; }

        [Required]
        [Column(TypeName = "DATE")]
        public DateOnly Date { get; set; }

        [Required]
        [Column(TypeName = "TIME(7)")]
        public TimeOnly StartTime { get; set; }

        [Required]
        [Column(TypeName = "TIME(7)")]
        public TimeOnly EndTime { get; set; }

        [Column(TypeName = "NVARCHAR(8)")]
        public string? TeacherId { get; set; }

        [Required]
        public int Status { get; set; }

        [Required]
        [Column(TypeName = "DATETIME")]
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<StudentInExamSchedule>? StudentInExamSchedules { get; set; }

        }
    }
