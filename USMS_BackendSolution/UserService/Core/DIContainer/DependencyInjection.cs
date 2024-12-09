using BusinessObject.AppDBContext;
using IRepository;
using IRepository.ICustomerRepository;
using Services;
using Services.CustomerService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public static class DependencyInjection
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            #region Register the repository
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            #endregion

            #region Register Service
            services.AddScoped<CustomerService>();
            #endregion
        }
    }
}
