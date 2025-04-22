using Repositories.RequestScheduleRepository;
using Repositories.RoomRepository;
using Repositories.ScheduleRepository;
using SchedulerDataAccess.Services.SchedulerServices;
using SchedulerService.Repository.SlotRepository;
using SchedulerService.Services.RequestScheduleServices.cs;
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
            services.AddScoped<ISlotRepository, SlotRepository>();
            services.AddScoped<IRequestRepository, RequestRepository>();
            #endregion

            #region Register Service
            services.AddHttpClient();
            services.AddScoped<ScheduleService>();
            services.AddScoped<RoomService>();
            services.AddScoped<SlotService>();
            services.AddScoped<RequestScheduleService>();
            #endregion
            }
        }
}
