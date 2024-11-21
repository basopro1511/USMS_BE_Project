using BusinessObject.AppDBContext;
using DataAccess.IRepository;
using DataAccess.IRepository.ICustomerRepository;
using DataAccess.Repository;
using DataAccess.Services;
using DataAccess.Services.CustomerService;
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
            #endregion

            #region Register Service
            services.AddScoped<CustomerService>();
            #endregion
        }
    }
}
