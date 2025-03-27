using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.ModelDTOs
    {
    public class LoginDTO
        {
        public string Email { get; set; }
        public string Password { get; set; }
        }

    public class ResetPasswordDTO
        {
        public string UserId { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
        }
    }
