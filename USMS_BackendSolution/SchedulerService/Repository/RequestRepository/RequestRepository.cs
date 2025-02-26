using Microsoft.EntityFrameworkCore;
using SchedulerBusinessObject.AppDBContext;
using SchedulerBusinessObject.SchedulerModels;

namespace SchedulerService.Repository.RequestRepository
{
	public class RequestRepository : IRequestRepository
	{
		private readonly MyDbContext _context;

		public RequestRepository(MyDbContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<RequestSchedule>> GetAllRequest()
		{
			return await _context.RequestsSchedule.ToListAsync();
		}

		public async Task<RequestSchedule?> GetRequestById(int requestId)
		{
			return await _context.RequestsSchedule.FirstOrDefaultAsync(r => r.RequestId == requestId);
		}

		public async Task AddRequest(RequestSchedule request)
		{
			await _context.RequestsSchedule.AddAsync(request);
			await _context.SaveChangesAsync();
		}

		public async Task ChangeRequestStatus(RequestSchedule request)
		{			
			_context.RequestsSchedule.Entry(request).State = EntityState.Modified;
			await _context.SaveChangesAsync();
		}
	}
}
