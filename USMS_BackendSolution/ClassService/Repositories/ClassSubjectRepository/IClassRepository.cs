using ClassBusinessObject.ModelDTOs;
using ClassBusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ClassSubjectRepository
    {
    public interface IClassRepository
        {
        Task<List<ClassSubject>> GetAllClassSubjects();
        Task<ClassSubject> GetClassSubjectById(int id);
        Task<List<ClassSubject>> GetClassSubjectByClassIds(string classId);
        Task<bool> AddNewClassSubject(ClassSubject classSubject);
        Task<bool> UpdateClassSubject(ClassSubject updateClassSubject);
        Task<bool> ChangeStatusClassSubject(int id);
        Task<ClassSubject> GetExistingClassSubject(string classId, string subjectId, string semesterId);
        Task<List<ClassSubject>> GetClassSubjectByMajorIdClassIdTerm(string majorId, string classId, int term);
        Task<List<string>> GetClassIdsByMajorId(string majorId);
        Task<List<ClassSubject>> GetSubjectInClassSubjectByMajorIdAndSemesterId(string majorId, string semesterId);
        Task<bool> ChangeClassStatusSelected(List<int> classIds, int status);
        }
    }
