using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerBusinessObject.ModelDTOs
    {
    public class TeacherDTO
        {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullUserName
            {
            get
                {
                return $"{LastName} {MiddleName} {FirstName}";
                }
            }
        public string Email { get; set; }
        public string UserAvartar { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string MajorId { get; set; }
        public string MajorName { get; set; }
        public int Status { get; set; }
        }
    }
