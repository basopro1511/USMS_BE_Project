using BusinessObject.AppDBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserService.Repository.MajorRepository;
using UserService.Repository.UserRepository;
using UserService.Services.MajorServices;

namespace Core
{
    public static class DependencyInjection
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            #region Register the repository
            services.AddScoped<IMajorRepository, MajorRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            #endregion

            #region Register Service
            services.AddScoped<MajorService>();
            services.AddScoped<UserService.Services.UserService.UserService>();
            #endregion
            }
        }
}
