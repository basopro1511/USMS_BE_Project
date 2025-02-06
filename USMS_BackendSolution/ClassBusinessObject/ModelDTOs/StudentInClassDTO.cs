using ClassBusinessObject.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassBusinessObject.ModelDTOs
    {
    public class StudentInClassDTO
        {
        public int StudentClassId { get; set; }

        public int ClassSubjectId { get; set; }

        public string StudentId { get; set; }
        }
    }
