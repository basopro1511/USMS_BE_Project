using ClassBusinessObject.ModelDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassDataAccess.Repositories.ClassSubjectRepository
{
    public interface IClassRepository
    {
        public List<ClassSubjectDTO> GetAllClassSubjects();
        public ClassSubjectDTO GetClassSubjectById(int id);
        public List<ClassSubjectDTO> GetClassSubjectByClassId(string classId);
    }
}
