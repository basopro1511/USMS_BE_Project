using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BusinessObject
{
    public class APIRequest
    {
        public APIRequest() { }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
