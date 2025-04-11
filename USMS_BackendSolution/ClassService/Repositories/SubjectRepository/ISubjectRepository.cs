using ClassBusinessObject.ModelDTOs;
using ClassBusinessObject.Models;

namespace Repositories.SubjectRepository
    {
    public interface ISubjectRepository
        {
        public Task<List<Subject>?> GetAllSubjects();
        public Task<List<Subject>>? GetAllSubjectsAvailable();
        public Task<Subject> GetSubjectsById(string subjectId);
        public Task<bool> CreateSubject(Subject subjectDTO);
        public Task<bool> UpdateSubject(Subject SubjectDTO);
        public Task<bool> SwitchStateSubject(string subjectId, int status);
        public Task<bool> ChangeSubjectStatusSelected(List<string> subjectIds, int status);
        public Task<bool> AddSubjectsAsyncs(List<Subject> models);
        }
    }
