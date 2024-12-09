using Repositories.ClassSubjectRepository;
using Repositories.SubjectRepository;
using Microsoft.Extensions.DependencyInjection;
using Repositories.SemesterRepository;
using Services.ClassServices;
using Services.SubjectServices;
using Services.SemesterServices;

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
