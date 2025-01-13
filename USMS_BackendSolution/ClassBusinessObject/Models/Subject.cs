using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassBusinessObject.Models
{
    public class Subject
    {
        [Key]
        [StringLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        public string SubjectId { get; set; }

        [StringLength(4)]
        [Column(TypeName = "nvarchar(4)")]
        public string MajorId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string SubjectName { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int NumberOfSlot { get; set; } 

        [StringLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "int")]
        public int Term {  get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } 

        [Required]
        public DateTime UpdatedAt { get; set; }
        [Column(TypeName = "int")]
        public int Status { get; set; }

        public virtual ICollection<ClassSubject>? ClassSubjects { get; set; }
    }
}
