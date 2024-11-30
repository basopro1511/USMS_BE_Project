using BusinessObject.AppDBContext;
using DataAccess.IRepository;
using DataAccess.IRepository.ICustomerRepository;
using DataAccess.Repository;
using DataAccess.Repository.MajorRepository;
using DataAccess.Repository.RoleRepository;
using DataAccess.Repository.UserRepository;
using DataAccess.Services;
using DataAccess.Services.CustomerService;
using DataAccess.Services.MajorService;
using DataAccess.Services.RoleSercvice;
using DataAccess.Services.UserService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Core
{
    public static class DependencyInjection
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            #region Register the repository
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IMajorRepository, MajorRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            #endregion

            #region Register Service
            services.AddScoped<CustomerService>();
            services.AddScoped<MajorService>();
            services.AddScoped<RoleService>();
            services.AddScoped<UserService>();
            #endregion
        }
    }
}
