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
        [Column(TypeName = "NVARCHAR(8)")]
        public string StudentId { get; set; }
        [Required]
        [Column(TypeName = "NVARCHAR(4)")]
        [ForeignKey("Major")]
        public string MajorId { get; set; }
        public Major Major { get; set; }
        [Column(TypeName = "NVARCHAR(4)")]
        public int Term { get; set; }
        public virtual User User { get; set; } 
    }
}
