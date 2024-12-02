using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassBusinessObject.Models
{
    public class ClassSubject
    {
        [Key]
        public int ClassSubjectId { get; set; }

        [Required]
        [StringLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        public string ClassId { get; set; }

        [Required]
        [StringLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        public string SubjectId { get; set; }

        [Required]
        [StringLength(4)]
        [Column(TypeName = "nvarchar(4)")]
        public string SemesterId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        [Required]
        [Column(TypeName = "bit")]
        public bool Status { get; set; }

        [ForeignKey("SubjectId")]
        public virtual Subject? Subject { get; set; }

        [ForeignKey("SemesterId")]
        public virtual Semester? Semester { get; set; }
    }
}
