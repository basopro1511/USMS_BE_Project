using ClassDataAccess.Repositories.ClassSubjectRepository;
using ClassDataAccess.Repositories.SubjectRepository;
using ClassDataAccess.Services.ClassServices;
using ClassDataAccess.Services.SubjectServices;
using Microsoft.Extensions.DependencyInjection;
using SchedulerBusinessObject.Services;
using SchedulerDataAccess.Repositories.SemesterRepository;

namespace SchedulerDataAccess.Core
{
	public static class DependencyInjection
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            #region Register the repository
            services.AddScoped<IClassRepository, ClassRepository>();
            services.AddScoped<ISubjectRepository, SubjectRepository>();
            services.AddScoped<ISemesterRepository, SemesterRepository>();
            #endregion

            #region Register Service
            services.AddScoped<ClassSubjectService>();
            services.AddScoped<SubjectService>();
            services.AddScoped<SemesterService>();
            #endregion
        }
    }
}
