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
           new Major { MajorId = "SE", MajorName = "Kỹ thuật phần mềm" },
           new Major { MajorId = "BA", MajorName = "Quản trị kinh doanh" },
           new Major { MajorId = "LG", MajorName = "Ngôn ngữ" },
           new Major { MajorId = "CT", MajorName = "Công nghệ truyền thông" }
             );

            optionsBuilder.Entity<User>().HasData(
                   new User
                       {
                       UserId = "SE170288",
                       FirstName = "Nguyễn",
                       MiddleName = "Quốc",
                       LastName = "Hoàng",
                       MajorId = "SE",
                       PasswordHash = "123456",
                       Email = "HoangNQSE170288@fpt.edu.vn",
                       PersonalEmail = "Hoang@gmail.com",
                       PhoneNumber = "0333744591",
                       DateOfBirth = new DateOnly(2003, 11, 15),
                       UserAvartar = null,
                       RoleId = 5,
                       Status = 1,
                       CreatedAt = DateTime.Now,
                       UpdatedAt = DateTime.Now
                       },
                   new User
                       {
                       UserId = "SE160815",
                       FirstName = "Nguyễn",
                       MiddleName = "Toàn",
                       LastName = "Thắnng",
                       MajorId = "SE",
                       PasswordHash = "123456",
                       Email = "ThangNTSE160815@fpt.edu.vn",
                       PersonalEmail = "Thang@gmail.com",
                       PhoneNumber = "0325134391",
                       DateOfBirth = new DateOnly(2002, 11, 20),
                       UserAvartar = null,
                       RoleId = 5,
                       Status = 1,
                       CreatedAt = DateTime.Now,
                       UpdatedAt = DateTime.Now
                       },
                   new User
                       {
                       UserId = "SE160461 ",
                       FirstName = "Nguyễn",
                       MiddleName = "Tuấn",
                       LastName = "Thịnh",
                       MajorId = "SE",
                       PasswordHash = "123456",
                       Email = "ThinhNTCE160461@fpt.edu.vn",
                       PersonalEmail = "thinh@gmail.com",
                       PhoneNumber = "0865561430",
                       DateOfBirth = new DateOnly(2002, 09, 10),
                       UserAvartar = null,
                       RoleId = 5,
                       Status = 1,
                       CreatedAt = DateTime.Now,
                       UpdatedAt = DateTime.Now
                       },
                   new User
                       {
                       UserId = "SE170289",
                       FirstName = "Trần",
                       MiddleName = "Thanh",
                       LastName = "Tùng",
                       MajorId = "SE",
                       PasswordHash = "123456",
                       Email = "TungTTSE170289@fpt.edu.vn",
                       PersonalEmail = "Tung@gmail.com",
                       PhoneNumber = "0322114477",
                       DateOfBirth = new DateOnly(2004, 9, 10),
                       UserAvartar = null,
                       RoleId = 5,
                       Status = 1,
                       CreatedAt = DateTime.Now,
                       UpdatedAt = DateTime.Now
                       },
                   new User
                       {
                       UserId = "BA170290",
                       FirstName = "Lê",
                       MiddleName = "Hồng",
                       LastName = "Nhung",
                       MajorId = "BA",
                       PasswordHash = "123456",
                       Email = "NhungLHBA170290@fpt.edu.vn",
                       PersonalEmail = "Nhung@gmail.com",
                       PhoneNumber = "0987654321",
                       DateOfBirth = new DateOnly(2003, 12, 5),
                       UserAvartar = null,
                       RoleId = 5,
                       Status = 1,
                       CreatedAt = DateTime.Now,
                       UpdatedAt = DateTime.Now
                       },
                   new User
                       {
                       UserId = "LG170291",
                       FirstName = "Phạm",
                       MiddleName = "Minh",
                       LastName = "Châu",
                       MajorId = "LG",
                       PasswordHash = "123456",
                       Email = "ChauPMLG170291@fpt.edu.vn",
                       PersonalEmail = "Chau@gmail.com",
                       PhoneNumber = "0978111222",
                       DateOfBirth = new DateOnly(2002, 5, 20),
                       UserAvartar = null,
                       RoleId = 5,
                       Status = 1,
                       CreatedAt = DateTime.Now,
                       UpdatedAt = DateTime.Now
                       },
                   new User
                       {
                       UserId = "CT170292",
                       FirstName = "Đinh",
                       MiddleName = "Văn",
                       LastName = "Dũng",
                       MajorId = "CT",
                       PasswordHash = "123456",
                       Email = "DungDVCT170292@fpt.edu.vn",
                       PersonalEmail = "Dung@gmail.com",
                       PhoneNumber = "0966998844",
                       DateOfBirth = new DateOnly(2003, 7, 30),
                       UserAvartar = null,
                       RoleId = 5,
                       Status = 1,
                       CreatedAt = DateTime.Now,
                       UpdatedAt = DateTime.Now
                       },
                   new User
                       {
                       UserId = "IT170293",
                       FirstName = "Vũ",
                       MiddleName = "Mạnh",
                       LastName = "Cường",
                       MajorId = "SE",
                       PasswordHash = "123456",
                       Email = "CuongVMIT170293@fpt.edu.vn",
                       PersonalEmail = "Cuong@gmail.com",
                       PhoneNumber = "0356677889",
                       DateOfBirth = new DateOnly(2001, 8, 25),
                       UserAvartar = null,
                       RoleId = 5,
                       Status = 1,
                       CreatedAt = DateTime.Now,
                       UpdatedAt = DateTime.Now
                       }
            );

            optionsBuilder.Entity<Student>().HasData(
               new Student { StudentId = "CE170288", MajorId = "SE", Term = 1 },
               new Student { StudentId = "CE170289", MajorId = "SE", Term = 1 },
               new Student { StudentId = "BA170290", MajorId = "BA", Term = 2 },
               new Student { StudentId = "LG170291", MajorId = "LG", Term = 3 },
               new Student { StudentId = "CT170292", MajorId = "CT", Term = 1 },
               new Student { StudentId = "IT170293", MajorId = "SE", Term = 4 }
            );
            }
        }
    }
