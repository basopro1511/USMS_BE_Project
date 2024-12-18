using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class User
    {
        [Key]
        [Required]
        [StringLength(20)]
        [Column(TypeName = "NVARCHAR(20)")]
        public string Email { get; set; }
        [Required]
        [StringLength(16)]
        [Column(TypeName = "NVARCHAR(16)")]
        public string Password { get; set; }
        [Required]
        [StringLength(10)]
        [Column(TypeName = "NVARCHAR(10)")]
        public string Role { get; set; }
    }
}
