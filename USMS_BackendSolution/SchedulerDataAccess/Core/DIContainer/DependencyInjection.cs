
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SchedulerDataAccess.Repositories.ScheduleRepository;
using SchedulerDataAccess.Services.SchedulerServices;

namespace SchedulerDataAccess.Core
{
    public static class DependencyInjection
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            #region Register the repository
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            #endregion

            #region Register Service
            services.AddScoped<ScheduleService>();
            #endregion
        }
    }
}
