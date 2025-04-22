using BusinessObject.AppDBContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UserService.Repository.MajorRepository;
using UserService.Repository.StaffRepository;
using UserService.Repository.StudentRepository;
using UserService.Repository.TeacherRepository;
using UserService.Repository.UserRepository;
using UserService.Services.LoginServices;
using UserService.Services.MajorServices;
using UserService.Services.StaffServices;
using UserService.Services.StudentServices;
using UserService.Services.TeacherService;
using UserService.Services.UserServices;

namespace Core
{
    public static class DependencyInjection
    {
        public static void ConfigureDependencyInjection(this IServiceCollection services)
        {
            #region Register the repository
            services.AddScoped<IMajorRepository, MajorRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<IStaffRepository, StaffRepository>();
            #endregion

            #region Register Service
            services.AddScoped<MajorService>();
            services.AddScoped<UserService.Services.UserServices.UserService>();
            services.AddScoped<StudentService>();
            services.AddScoped<TeacherService>();
            services.AddScoped<StaffService>();
            services.AddScoped<LoginService>();
            services.AddHttpClient();
            services.AddAuthorization();
            #endregion
            }
        }
}
