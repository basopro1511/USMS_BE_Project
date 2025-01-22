using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassBusinessObject.Models
{
   public class StudentInClass
    {
        [Key]
        public int StudentClassId { get; set; }
        [Column(TypeName = "INT")]
        [ForeignKey("ClassSubject")]
        public int ClassSubjectId { get; set; }
        [Column(TypeName = "nvarchar(8)")]
        public string StudentId { get; set; }
        [ForeignKey("ClassSubjectId")]
        public ClassSubject ClassSubject { get; set; } // Many-to-One
        }
}
