using BusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Major> Major { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        protected override void OnModelCreating(ModelBuilder optionsBuilder)
        {
            optionsBuilder.Entity<Role>().HasData(
           new Role { RoleId = 1, RoleName = "Admin" },
           new Role { RoleId = 2, RoleName = "Academic Staff" },
           new Role { RoleId = 3, RoleName = "Chairperson" },
           new Role { RoleId = 4, RoleName = "Teacher" },
           new Role { RoleId = 5, RoleName = "Student" }
           );

            optionsBuilder.Entity<Major>().HasData(
           new Major { MajorId = "IT", MajorName = "Information Technology" },
           new Major { MajorId = "BA", MajorName = "Business Administration" },
           new Major { MajorId = "MT", MajorName = "Media Technology" }
             );

            optionsBuilder.Entity<User>().HasData(
             new User
             {
                 UserId = "CE170288",
                 FirstName = "Nguyễn",
                 MiddleName = "Quốc",
                 LastName = "Hoàng",
                 MajorId = "IT",
                 PasswordHash = "123456",
                 Email = "HoangNQCE170288@fpt.edu.vn"
             ,
                 PersonalEmail = "Hoang@gmail.com",
                 PhoneNumber = "0333744591",
                 DateOfBirth = new DateOnly(2025, 11, 15),
                 UserAvartar = null,
                 RoleId = 1,
                 Status = 1,
                 CreatedAt = DateTime.Now,
                 UpdatedAt = DateTime.Now
             }
            );

            optionsBuilder.Entity<Student>().HasData(
            new Student { StudentId = "CE170288", MajorId ="IT", Term = 1}
       );
        }
    }
}
