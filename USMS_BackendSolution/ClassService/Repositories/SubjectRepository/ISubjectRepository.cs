using ClassBusinessObject.ModelDTOs;
using ClassBusinessObject.Models;

namespace Repositories.SubjectRepository
    {
    public interface ISubjectRepository
        {
        public Task<bool> CreateSubject(Subject subjectDTO);
        public Task<bool> UpdateSubject(Subject SubjectDTO);
        public Task<Subject> GetSubjectsById(string subjectId);
        public Task<List<Subject>?> GetAllSubjects();
        public Task<bool> SwitchStateSubject(string subjectId, int status);
        public Task<List<Subject>>? GetAllSubjectsAvailable();
        public Task<bool> ChangeSubjectStatusSelected(List<string> subjectIds, int status);
        }
    }
