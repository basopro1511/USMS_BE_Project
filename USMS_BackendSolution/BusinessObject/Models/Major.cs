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
        [StringLength(4)]
        [Column(TypeName = "NVARCHAR(4)")]
        public string MajorId { get; set; }
        [Required]
        [StringLength(100)]
        [Column(TypeName = "NVARCHAR(100)")]
        public string MajorName { get; set; }
    }
}
