using ClassBusinessObject.ModelDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ClassSubjectRepository
{
    public interface IClassRepository
    {
        public List<ClassSubjectDTO> GetAllClassSubjects();
        public ClassSubjectDTO GetClassSubjectById(int id);
        public List<ClassSubjectDTO> GetClassSubjectByClassIds(string classId);
        public bool AddNewClassSubject(AddUpdateClassSubjectDTO classSubjectDTO);
        public bool UpdateClassSubject(AddUpdateClassSubjectDTO updateClassSubjectDTO);
        public bool ChangeStatusClassSubject(int id);
        public ClassSubjectDTO GetExistingClassSubject(string classId, string subjectId, string semesterId);
        public List<ClassSubjectDTO> GetClassSubjectByMajorIdClassIdTerm(string majorId, string classId, int term);
        List<string> GetClassIdsByMajorId(string majorId);
        }
    }
