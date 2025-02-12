using BusinessObject.AppDBContext;
using IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserService.Services;
using IRepository.IRoleRepository;

namespace Core
{
    public static class DependencyInjection
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            #region Register the repository
            services.AddScoped<IRoleRepository, RoleRepository>();
            #endregion

            #region Register Service
            services.AddScoped<UsersService>();
            #endregion
        }
    }
}
