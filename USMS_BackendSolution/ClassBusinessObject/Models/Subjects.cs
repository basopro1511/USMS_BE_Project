using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassBusinessObject.Models
{
    public class Subjects
    {
        [Key]
        [StringLength(10)]
        [Column(TypeName = "nvarchar(10)")]
        public string SubjectId { get; set; }

        [Required]
        [StringLength(100)]
        [Column(TypeName = "nvarchar(100)")]
        public string SubjectName { get; set; }

        [Required]
        public int NumberOfSlot { get; set; } 

        [StringLength(200)]
        [Column(TypeName = "nvarchar(200)")]
        public string Description { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } 

        [Required]
        public DateTime UpdatedAt { get; set; } 

        public virtual ICollection<ClassSubjects>? ClassSubjects { get; set; }
    }
}
