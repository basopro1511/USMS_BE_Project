using SchedulerBusinessObject.SchedulerModels;

namespace SchedulerService.Repository.RequestRepository
{
	public interface IRequestRepository
	{
		Task<IEnumerable<RequestSchedule>> GetAllRequest();
		Task<RequestSchedule?> GetRequestById(int requestId);
		Task AddRequest(RequestSchedule createRequest);
		Task ChangeRequestStatus(RequestSchedule createRequest);
	}
}
