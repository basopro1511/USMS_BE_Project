using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.AppDBContext
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }
        public MyDbContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            IConfigurationRoot configuration = builder.Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("HoangConnection"));
        }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Student> Student { get; set; }

        protected override void OnModelCreating(ModelBuilder optionsBuilder)
        {
            base.OnModelCreating(optionsBuilder);
            optionsBuilder.Entity<User>().HasData(
            new User { UserId = "IT0001", FirstName = "Hoàng", MiddleName="Quốc", LastName="Nguyễn", PhoneNumber = "0333744591", PasswordHash  = "123", Email="HoangNQIT0001@gmail.com", PersonalEmail="nqh@gmail.com", RoleId=5, UserAvartar="123", DateOfBirth = new DateOnly(1990, 1, 1), Status = 1, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now},
            new User { UserId = "IT0002", FirstName = "Thịnh", MiddleName = "Tuấn", LastName = "Nguyễn", PhoneNumber = "0333744591", PasswordHash = "123", Email = "ThinhNTIT0002@gmail.com", PersonalEmail = "nqh@gmail.com", RoleId = 5, UserAvartar = "123", DateOfBirth = new DateOnly(1990, 1, 1), Status = 1, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new User { UserId = "IB0001", FirstName = "Thắng", MiddleName = "Toàn", LastName = "Nguyễn", PhoneNumber = "0333744591", PasswordHash = "123", Email = "ThangNTIB0001@gmail.com", PersonalEmail = "nqh@gmail.com", RoleId = 5, UserAvartar = "123", DateOfBirth = new DateOnly(1990, 1, 1), Status = 1, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new User { UserId = "IB0002", FirstName = "Ân", MiddleName = "Đức", LastName = "Lê", PhoneNumber = "0333744591", PasswordHash = "123", Email = "AnLDIB0002@gmail.com", PersonalEmail = "nqh@gmail.com", RoleId = 5, UserAvartar = "123", DateOfBirth = new DateOnly(1990, 1, 1), Status = 1, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
            );
            optionsBuilder.Entity<Student>().HasData(
            new Student { StudentId = "IT0001", MajorId= "IT", Term= 2},
            new Student { StudentId = "IT0002", MajorId = "IT", Term = 3 },
            new Student { StudentId = "IB0001", MajorId = "IB", Term = 4 },
            new Student { StudentId = "IB0002", MajorId = "IB", Term = 5 }

           );
            optionsBuilder.Entity<Role>().HasData(
            new Role { RoleId = 1, RoleName = "Admin" },
            new Role { RoleId = 2, RoleName= "AcademicStaff"},
            new Role { RoleId = 3, RoleName = "Chairperson" },
            new Role { RoleId = 4, RoleName = "Teacher" },
            new Role { RoleId = 5, RoleName = "Student" }
           );
            optionsBuilder.Entity<Major>().HasData(
           new Major { MajorId = "IB", MajorName= "International Business" },
           new Major { MajorId = "IT", MajorName = "Information Technology" }
          );
        }
    }
}
