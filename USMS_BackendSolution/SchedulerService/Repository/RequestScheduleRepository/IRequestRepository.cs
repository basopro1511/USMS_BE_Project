using SchedulerBusinessObject.SchedulerModels;

namespace Repositories.RequestScheduleRepository
    {
    public interface IRequestRepository
        {
        public Task<List<RequestSchedule>> GetRequestSchedules();
        public Task<bool> CreateRequest(RequestSchedule requestSchedule);
        public Task<RequestSchedule> GetRequestScheduleById(int id);
        public Task<bool> ChangeRequestStatus(int requestId, int newStatus);
        public Task<bool> UpdateRequest(RequestSchedule update);

        }
    }
