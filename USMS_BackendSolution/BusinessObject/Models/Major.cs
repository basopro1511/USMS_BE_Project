using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class Major
    {
        [Key]
        [Required]
        [Column(TypeName = "NVARCHAR(4)")]
        public string MajorId { get; set; }
        [Required]
        [Column(TypeName = "NVARCHAR(100)")]
        public string MajorName { get; set; }
        public ICollection<User> Users { get; set; } // One-to-Many
        public ICollection<Student> Students { get; set; } // One-to-Many

        }
}
