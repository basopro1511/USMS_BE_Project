namespace ClassBusinessObject.ModelDTOs
{
	public class ClassSubjectDTO
	{
		public int ClassSubjectId { get; set; }
		public string ClassId { get; set; }
		public string SubjectId { get; set; }
		public string SemesterId { get; set; }
		public string? ClassSubjectName
		{
			get
			{
				return $"{SemesterId}_{SubjectId}_{ClassId}";  // Dung de lay chuoi string de hien thi lich cho teacher
			}
		}
		public DateTime CreatedAt { get; set; }
	}
	public class AddClassSubjectDTO
	{
		public int ClassSubjectId { get; set; }
		public string ClassId { get; set; }
		public string SubjectId { get; set; }
		public string SemesterId { get; set; }
		public DateTime CreatedAt { get; set; }
	}
}
