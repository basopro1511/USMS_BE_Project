using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassBusinessObject.Models
{
	public class Semesters
    {
        [Key]
        [StringLength(4)]
        [Column(TypeName = "nvarchar(4)")]
        public string SemesterId { get; set; }

        [Required]
        [StringLength(20)]
        [Column(TypeName = "nvarchar(20)")]
        public string SemesterName { get; set; }

        [Required]
        public DateOnly StartDate { get; set; }

        [Required]
        public DateOnly EndDate { get; set; }

        [Required]
        public int Status { get; set; }

        public virtual ICollection<ClassSubjects>? ClassSubjects { get; set; }
    }
}
