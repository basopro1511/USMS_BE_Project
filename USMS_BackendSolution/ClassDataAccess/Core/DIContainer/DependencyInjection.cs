using ClassDataAccess.Repositories.ClassSubjectRepository;
using ClassDataAccess.Services.ClassServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SchedulerDataAccess.Core
{
    public static class DependencyInjection
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            #region Register the repository
            services.AddScoped<IClassRepository, ClassRepository>();
            #endregion

            #region Register Service
            services.AddScoped<ClassSubjectService>();
            #endregion
        }
    }
}
