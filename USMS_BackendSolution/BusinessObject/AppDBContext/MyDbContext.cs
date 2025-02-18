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
            //optionsBuilder.UseSqlServer(configuration.GetConnectionString("HoangConnection"));
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("NamConnection"));
            }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Major> Major { get; set; }
        public virtual DbSet<Student> Student { get; set; }
        protected override void OnModelCreating(ModelBuilder optionsBuilder)
            {
            optionsBuilder.Entity<Role>().HasData(
           new Role { RoleId=1, RoleName="Admin" },
           new Role { RoleId=2, RoleName="Academic Staff" },
           new Role { RoleId=3, RoleName="Chairperson" },
           new Role { RoleId=4, RoleName="Teacher" },
           new Role { RoleId=5, RoleName="Student" }
           );

            optionsBuilder.Entity<Major>().HasData(
           new Major { MajorId="SE", MajorName="Kỹ thuật phần mềm" },
           new Major { MajorId="BA", MajorName="Quản trị kinh doanh" },
           new Major { MajorId="LG", MajorName="Ngôn ngữ" },
           new Major { MajorId="CT", MajorName="Công nghệ truyền thông" }
             );
            // Thêm dữ liệu User trước
            SeedUsers(optionsBuilder);
            // Thêm dữ liệu Student sau
            SeedStudents(optionsBuilder);
            }
        private void SeedUsers(ModelBuilder modelBuilder)
            {
            modelBuilder.Entity<User>().HasData(
                 new User
                     {
                     UserId="AD0001",
                     FirstName="Nguyễn",
                     MiddleName="Quốc",
                     LastName="Hoàng",
                     MajorId="SE",
                     PasswordHash="8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92",
                     Email="Admin@fpt.edu.vn",
                     PersonalEmail="Admin@gmail.com",
                     PhoneNumber="0333744591",
                     DateOfBirth=new DateOnly(2005, 11, 15),
                     UserAvartar=null,
                     RoleId=1,
                     Status=1,
                     CreatedAt=DateTime.Now,
                     UpdatedAt=DateTime.Now,
                     Address="FPT Can Tho"
                     },
                new User
                    {
                    UserId="SE0001",
                    FirstName="Nguyễn",
                    MiddleName="Quốc",
                    LastName="Hoàng",
                    MajorId="SE",
                    PasswordHash="8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92",
                    Email="HoangNQCE170288@fpt.edu.vn",
                    PersonalEmail="Hoang@gmail.com",
                    PhoneNumber="0333744591",
                    DateOfBirth=new DateOnly(2005, 11, 15),
                    UserAvartar=null,
                    RoleId=5,
                    Status=1,
                    CreatedAt=DateTime.Now,
                    UpdatedAt=DateTime.Now,
                    Address="Can Tho"
                    },
        new User
            {
            UserId="SE0002",
            FirstName="Trần",
            MiddleName="Thanh",
            LastName="Tùng",
            MajorId="SE",
            PasswordHash="8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92",
            Email="TungTTCE170289@fpt.edu.vn",
            PersonalEmail="Tung@gmail.com",
            PhoneNumber="0322114477",
            DateOfBirth=new DateOnly(2004, 9, 10),
            UserAvartar=null,
            RoleId=5,
            Status=1,
            CreatedAt=DateTime.Now,
            UpdatedAt=DateTime.Now,
            Address="Can Tho"
            },
        new User
            {
            UserId="BA0003",
            FirstName="Lê",
            MiddleName="Hồng",
            LastName="Nhung",
            MajorId="BA",
            PasswordHash="8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92",
            Email="NhungLHBA170290@fpt.edu.vn",
            PersonalEmail="Nhung@gmail.com",
            PhoneNumber="0987654321",
            DateOfBirth=new DateOnly(2003, 12, 5),
            UserAvartar=null,
            RoleId=5,
            Status=1,
            CreatedAt=DateTime.Now,
            UpdatedAt=DateTime.Now,
            Address="Can Tho"
            },
        new User
            {
            UserId="LG0004",
            FirstName="Phạm",
            MiddleName="Minh",
            LastName="Châu",
            MajorId="LG",
            PasswordHash="8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92",
            Email="ChauPMLG170291@fpt.edu.vn",
            PersonalEmail="Chau@gmail.com",
            PhoneNumber="0978111222",
            DateOfBirth=new DateOnly(2002, 5, 20),
            UserAvartar=null,
            RoleId=5,
            Status=1,
            CreatedAt=DateTime.Now,
            UpdatedAt=DateTime.Now,
            Address="Can Tho"
            },
        new User
            {
            UserId="CT0005",
            FirstName="Đinh",
            MiddleName="Văn",
            LastName="Dũng",
            MajorId="CT",
            PasswordHash="8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92",
            Email="DungDVCT170292@fpt.edu.vn",
            PersonalEmail="Dung@gmail.com",
            PhoneNumber="0966998844",
            DateOfBirth=new DateOnly(2003, 7, 30),
            UserAvartar=null,
            RoleId=5,
            Status=1,
            CreatedAt=DateTime.Now,
            UpdatedAt=DateTime.Now,
            Address="Can Tho"
            },
        new User
            {
            UserId="SE0006",
            FirstName="Vũ",
            MiddleName="Mạnh",
            LastName="Cường",
            MajorId="SE",
            PasswordHash="8d969eef6ecad3c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c92",
            Email="CuongVMIT170293@fpt.edu.vn",
            PersonalEmail="Cuong@gmail.com",
            PhoneNumber="0356677889",
            DateOfBirth=new DateOnly(2001, 8, 25),
            UserAvartar=null,
            RoleId=5,
            Status=1,
            CreatedAt=DateTime.Now,
            UpdatedAt=DateTime.Now,
            Address="Can Tho"
            }
            );
            }

        // Phương thức seed dữ liệu Student sau khi User đã tồn tại
        private void SeedStudents(ModelBuilder modelBuilder)
            {
            modelBuilder.Entity<Student>().HasData(
        new Student { StudentId="SE0001", MajorId="SE", Term=1 },
        new Student { StudentId="SE0002", MajorId="SE", Term=1 },
        new Student { StudentId="BA0003", MajorId="BA", Term=2 },
        new Student { StudentId="LG0004", MajorId="LG", Term=3 },
        new Student { StudentId="CT0005", MajorId="CT", Term=4 },
        new Student { StudentId="SE0006", MajorId="SE", Term=5 });
            }
        }
    }
