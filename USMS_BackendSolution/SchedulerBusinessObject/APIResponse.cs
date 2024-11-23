using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SchedulerBusinessObject
{
    public class APIResponse
    {
        public APIResponse() { }
        public bool IsSuccess { get; set; } = true;
        public string? Message { get; set; }
        public List<Error>? Errors { get; set; }
        public object? Result { get; set; }
    }
}
