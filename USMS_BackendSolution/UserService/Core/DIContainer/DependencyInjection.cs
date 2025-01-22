using BusinessObject.AppDBContext;
using IRepository;
using IRepository.ICustomerRepository;
using Services;
using Services.CustomerService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserService.Repository.UserRepository;
using UserService.Services.UserService;
using UserService.Repository.StudentRepository;

namespace Core
{
    public static class DependencyInjection
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            #region Register the repository
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            #endregion

            #region Register Service
            services.AddScoped<CustomerService>();
            services.AddScoped<UserService.Services.UserService.UserService>();
            services.AddScoped<UserService.Services.StudentService.StudentService>();
            #endregion
        }
    }
}
