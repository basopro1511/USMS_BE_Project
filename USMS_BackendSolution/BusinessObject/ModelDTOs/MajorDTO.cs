using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.ModelDTOs
{
    public class MajorDTO
    {
        public string MajorId { get; set; }
        public string MajorName { get; set; }
    }
}
