using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AddSchedule
{
	public class APIResponse
	{
		public APIResponse() { }
		public bool isSuccess { get; set; } = true;
		public string? message { get; set; }
		public List<Error>? errors { get; set; }
		public object? result { get; set; }
	}
}
