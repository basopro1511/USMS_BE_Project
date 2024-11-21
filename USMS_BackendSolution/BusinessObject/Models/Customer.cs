using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class Customer
    {
        [Key]
        [Required]
        [StringLength(8)]
        [Column(TypeName = "NVARCHAR(8)")]
        public string Id { get; set; }
        [Required]
        [StringLength(30)]
        [Column(TypeName = "NVARCHAR(30)")]
        public string Name { get; set; }
        [Required]
        [StringLength(11)]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Phone { get; set; }
        [Required]
        [Column(TypeName = "NVARCHAR(MAX)")]
        public string Address { get; set; }
    }
}
