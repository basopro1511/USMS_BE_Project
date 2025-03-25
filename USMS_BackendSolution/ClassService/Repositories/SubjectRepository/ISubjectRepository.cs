using ClassBusinessObject.ModelDTOs;

namespace Repositories.SubjectRepository
    {
    public interface ISubjectRepository
        {
        public Task<bool> CreateSubject(SubjectDTO subjectDTO);
        public Task<bool> UpdateSubject(SubjectDTO SubjectDTO);
        public Task<SubjectDTO> GetSubjectsById(string subjectId);
        public Task<List<SubjectDTO>?> GetAllSubjects();
        public Task<bool> SwitchStateSubject(string subjectId, int status);
        public Task<List<SubjectDTO>>? GetAllSubjectsAvailable();
        }
    }
