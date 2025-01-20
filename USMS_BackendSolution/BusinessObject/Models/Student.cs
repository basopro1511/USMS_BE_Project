using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObject.Models
{
    public class Student
    {
        [Key]
        [Column(TypeName = "NVARCHAR(8)")]
        public string StudentId { get; set; }
        [ForeignKey("Major")]
        [Column(TypeName = "NVARCHAR(4)")]
        public string? MajorId { get; set; }
        [Column(TypeName = "INT")]
        public int Term {  get; set; }
        [JsonIgnore]
        public Major Major { get; set; }
    }
}
