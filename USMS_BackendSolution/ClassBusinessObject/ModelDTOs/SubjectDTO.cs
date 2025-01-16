using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ClassBusinessObject.ModelDTOs
{
	public class SubjectDTO
	{
		public string SubjectId { get; set; }
		public string SubjectName { get; set; }
        public string MajorId { get; set; }
		public int NumberOfSlot { get; set; }
		public string Description { get; set; }
        public int Term { get; set; }
        public int Status { get; set; }
	}
}
