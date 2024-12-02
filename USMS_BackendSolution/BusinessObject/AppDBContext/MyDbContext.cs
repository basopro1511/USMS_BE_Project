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
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("ThinhConnection"));
        }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Major> Major { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed data for Customer
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = "CE170288", Name = "Nguyễn Quốc Hoàng", Phone = "0333744591", Address = "Phong Điền, Cần Thơ" }
            );

            // Seed data for Major
            modelBuilder.Entity<Major>().HasData(
                new Major { MajorId = "IB", MajorName = "International Business" },
                new Major { MajorId = "IT", MajorName = "Information Technology" }
            );

            // Seed data for Role
            modelBuilder.Entity<Role>().HasData(
                new Role { RoleId = 1, RoleName = "Admin" },
                new Role { RoleId = 2, RoleName = "Academic Staff" },
                new Role { RoleId = 3, RoleName = "Chairperson" },
                new Role { RoleId = 4, RoleName = "Lecture" },
                new Role { RoleId = 5, RoleName = "Student" }
            );

            // Seed data for User with auto-generated UserId for Students
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = "IT0001",
                    FirstName = "Nguyen",
                    MiddleName = "Van",
                    LastName = "A",
                    PasswordHash = "hashedpassword1",
                    Email = "ANVIT0001.a@university.edu",
                    PersonalEmail = "van.a@gmail.com",
                    PhoneNumber = "0123456789",
                    RoleId = 5, // Student
                    Status = 1,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
            new User
            {
                    UserId = "IB0001",
                    FirstName = "Tran",
                    MiddleName = "Thi",
                    LastName = "B",
                    PasswordHash = "hashedpassword2",
                    Email = "BTTIB0001.b@university.edu",
                    PersonalEmail = "thi.b@gmail.com",
                    PhoneNumber = "0987654321",
                    RoleId = 5, // Student
                    Status = 1,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
            },
            new User
            {
                    UserId = "AD0001",
                    FirstName = "Nguyen",
                    MiddleName = "Tuan",
                    LastName = "Admin",
                    PasswordHash = "hashedpassword3",
                    Email = "admin0001@university.edu",
                    PersonalEmail = "admin0001@gmail.edu",
                    PhoneNumber = "0123456780",
                    RoleId = 1, // Admin
                    Status = 1,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
            }
);

        }

    }
}
