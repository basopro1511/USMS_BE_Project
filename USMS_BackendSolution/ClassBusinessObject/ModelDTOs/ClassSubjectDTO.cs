using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public bool Status { get; set; }
    }
    public class AddUpdateClassSubjectDTO
    {
        public int ClassSubjectId { get; set; }
        public string ClassId { get; set; }
        public string SubjectId { get; set; }
        public string SemesterId { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Status { get; set; }
    }
}
