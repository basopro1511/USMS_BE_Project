using Repositories.RoomRepository;
using Repositories.ScheduleRepository;
using SchedulerDataAccess.Services.SchedulerServices;
using SchedulerService.Repository.ExamScheduleRepository;
using SchedulerService.Repository.RequestRepository;
using SchedulerService.Repository.SlotRepository;
using SchedulerService.Services.ExamScheduleServices;
using SchedulerService.Services.RequestServices;
using SchedulerService.Services.SlotServices;
using Services.RoomServices;

namespace SchedulerDataAccess.Core
{
	public static class DependencyInjection
	{
		public static void ConfigureDependencyInjection(this IServiceCollection services)
		{
			#region Register the repository
			services.AddScoped<IScheduleRepository, ScheduleRepository>();
			services.AddScoped<IRoomRepository, RoomRepository>();
			services.AddScoped<IExamScheduleRepository, ExamScheduleRepository>();
			services.AddScoped<ISlotRepository, SlotRepository>();
			services.AddScoped<IRequestRepository, RequestRepository>();
			#endregion

			#region Register Service
			services.AddScoped<ScheduleService>();
			services.AddScoped<RoomService>();
			services.AddScoped<ExamScheduleService>();
			services.AddScoped<SlotService>();
			services.AddScoped<RequestService>();
			#endregion
		}
	}
}
