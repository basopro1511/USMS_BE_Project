using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class Student
    {
        [Key]
        [Required]
        [StringLength(8)]
        [Column(TypeName = "NVARCHAR(8)")]
        public string StudentId { get; set; }
        [Required]
        [StringLength(4)]
        [Column(TypeName = "NVARCHAR(4)")]
        [ForeignKey("Major")]
        public string MajorId { get; set; }
        public Major Major { get; set; }

        public int Term { get; set; }

        public virtual User User { get; set; }

       
    }
}
