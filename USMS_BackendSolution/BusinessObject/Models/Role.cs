using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class Role
    {
        [Key]
        [Required]
        public int RoleId { get; set; }
        [Required]
        [Column(TypeName = "NVARCHAR(50)")]
        public string RoleName { get; set; }
        public ICollection<User> Users { get; set; }
    }

}
