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
        public Task<List<ClassSubjectDTO>> GetAllClassSubjects();
        public Task<ClassSubjectDTO> GetClassSubjectById(int id);
        public Task<List<ClassSubjectDTO>> GetClassSubjectByClassIds(string classId);
        public Task<bool> AddNewClassSubject(AddUpdateClassSubjectDTO classSubjectDTO);
        public Task<bool> UpdateClassSubject(AddUpdateClassSubjectDTO updateClassSubjectDTO);
        public Task<bool> ChangeStatusClassSubject(int id);
        public Task<ClassSubjectDTO> GetExistingClassSubject(string classId, string subjectId, string semesterId);
        public Task<List<ClassSubjectDTO>> GetClassSubjectByMajorIdClassIdTerm(string majorId, string classId, int term);
        public Task<List<string>> GetClassIdsByMajorId(string majorId);
        public Task<List<ClassSubjectDTO>> GetSubjectInClassSubjectByMajorIdAndSemesterId(string majorId, string semesterId);
        }
    }
