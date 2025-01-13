using ClassBusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassBusinessObject.AppDBContext
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

        public virtual DbSet<ClassSubject> ClassSubject { get; set; }
        public virtual DbSet<Semester> Semester { get; set; }
        public virtual DbSet<Subject> Subject { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>().HasData(
             new Subject { SubjectId = "PRM392", SubjectName = "Mobile Programing", NumberOfSlot = 20, Description = "Description",Term = 8, CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now , Status = true },
             new Subject { SubjectId = "PRN231", SubjectName = "Building Cross-Platform Back-End Application With .NET",Term = 8, NumberOfSlot = 20, Description = "Description", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Status = true },
             new Subject { SubjectId = "MLN122", SubjectName = "Political economics of Marxism – Leninism",Term = 8, NumberOfSlot = 16, Description = "Description", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, Status = true }
          );
            modelBuilder.Entity<Semester>().HasData(
              new Semester { SemesterId = "SP25", SemesterName = "Spring2025", StartDate = new DateOnly(2025, 01, 05), EndDate = new DateOnly(2025, 03, 24), Status = 1 },
              new Semester { SemesterId = "SU25", SemesterName = "Summer2025", StartDate = new DateOnly(2025, 05, 08), EndDate = new DateOnly(2025, 08, 11), Status = 1 },
              new Semester { SemesterId = "FA25", SemesterName = "Fall2025", StartDate = new DateOnly(2025, 09, 05), EndDate = new DateOnly(2025, 11, 25), Status = 1 }
           );
            modelBuilder.Entity<ClassSubject>().HasData(
               new ClassSubject { ClassSubjectId = 1, ClassId = "SE1702", SubjectId = "PRM392", SemesterId = "SP25", CreatedAt = new DateTime(2024, 11, 25), Status = 1},
               new ClassSubject { ClassSubjectId = 2, ClassId = "SE1702", SubjectId = "PRN231", SemesterId = "SP25", CreatedAt = new DateTime(2024, 11, 25), Status = 1},
               new ClassSubject { ClassSubjectId = 3, ClassId = "SE1702", SubjectId = "MLN122", SemesterId = "SP25", CreatedAt = new DateTime(2024, 11, 25), Status = 1}
            );

        }
    }
}
