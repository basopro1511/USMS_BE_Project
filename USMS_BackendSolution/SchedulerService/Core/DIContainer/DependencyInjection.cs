
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repositories.RoomRepository;
using Repositories.ScheduleRepository;
using SchedulerDataAccess.Services.SchedulerServices;
using SchedulerService.Repository.ExamScheduleRepository;
using SchedulerService.Services.ExamScheduleServices;
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

            #endregion

            #region Register Service
            services.AddScoped<ScheduleService>();
            services.AddScoped<RoomService>();
            services.AddScoped<ExamScheduleService>();
            #endregion
        }
    }
}
