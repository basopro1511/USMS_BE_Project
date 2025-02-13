using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassBusinessObject.ModelDTOs
    {
    public class StudentDTO
        {
        public int StudentClassId { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public int RoleId { get; set; }
        public int Status { get; set; }
        public string MajorId { get; set; }
        public int Term { get; set; }
        public string RoleName { get; set; }
        public string MajorName { get; set; }
        }
    }
