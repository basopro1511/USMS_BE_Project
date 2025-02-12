using UserService.Repository.MajorRepository;
using UserService.Repository.StudentRepository;
using UserService.Repository.UserRepository;
using UserService.Services.MajorServices;
using UserService.Services.StudentService;
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
			#endregion

			#region Register Service
			services.AddScoped<MajorService>();
			services.AddScoped<UserServices>();
			services.AddScoped<StudentService>();
			#endregion
		}
	}
}
