using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace SchedulerBusinessObject.ModelDTOs
{
	public class ClassSubjectDTO
	{
		public int ClassSubjectId { get; set; }

		public string ClassId { get; set; } = null!;

		public string SubjectId { get; set; } = null!;

		public string SemesterId { get; set; } = null!;
        public string MajorId { get; set; } = null!;
        public int Term { get; set; }
		public int Status { get; set; }
	}
}
