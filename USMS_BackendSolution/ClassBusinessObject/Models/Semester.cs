using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassBusinessObject.Models
{
    public class Semester
    {
        [Key]
        [Column(TypeName = "nvarchar(4)")]
        public string SemesterId { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(20)")]
        public string SemesterName { get; set; }

        [Required]
        public DateOnly StartDate { get; set; }

        [Required]
        public DateOnly EndDate { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int Status { get; set; }

        public virtual ICollection<ClassSubject>? ClassSubjects { get; set; }
    }
}
