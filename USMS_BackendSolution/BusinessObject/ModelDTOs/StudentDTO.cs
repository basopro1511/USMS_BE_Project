using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessObject.ModelDTOs
{
    public class StudentDTO
    {
        public string StudentId { get; set; }
        public string? MajorId { get; set; }
        public int Term { get; set; }
    }
}
