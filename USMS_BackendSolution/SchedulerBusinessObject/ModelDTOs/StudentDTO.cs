using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerBusinessObject.ModelDTOs
    {
    public class StudentDTO
        {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public int RoleId { get; set; }
        public string MajorId { get; set; }
        public int Term { get; set; }
        }
    }
