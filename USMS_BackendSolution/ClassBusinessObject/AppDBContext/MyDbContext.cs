using ClassBusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
        public virtual DbSet<StudentInClass> StudentInClass { get; set; }
        protected override void OnModelCreating(ModelBuilder optionBuilder)
            {
            optionBuilder.Entity<Semester>().HasData(
                new Semester { SemesterId="SP25", SemesterName="Spring2025", StartDate=new DateOnly(2025, 01, 05), EndDate=new DateOnly(2025, 03, 24), Status=1 },
                new Semester { SemesterId="SU25", SemesterName="Summer2025", StartDate=new DateOnly(2025, 05, 08), EndDate=new DateOnly(2025, 08, 11), Status=1 },
                new Semester { SemesterId="FA25", SemesterName="Fall2025", StartDate=new DateOnly(2025, 09, 05), EndDate=new DateOnly(2025, 11, 25), Status=1 }
                );
            SeedSubjects(optionBuilder);
            //optionBuilder.Entity<StudentInClass>().HasData(
            //    new StudentInClass { StudentClassId=1, ClassSubjectId=1, StudentId="SE0001" },
            //    new StudentInClass { StudentClassId=2, ClassSubjectId=1, StudentId="SE0002" },
            //    new StudentInClass { StudentClassId=3, ClassSubjectId=2, StudentId="SE0001" },
            //    new StudentInClass { StudentClassId=4, ClassSubjectId=3, StudentId="SE0001" }
            //     );
            //SeedClassSubjects(optionBuilder);
            }
        private void SeedSubjects(ModelBuilder modelBuilder)
            {
            modelBuilder.Entity<Subject>().HasData(
           //IT SUBJECT
           new Subject { SubjectId="MAE101", MajorId="SE", SubjectName="Mathematics for Engineering", NumberOfSlot=25, Description="Engineering mathematics fundamentals", Term=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, Status=1 },
           new Subject { SubjectId="PRF192", MajorId="SE", SubjectName="Programming Fundamentals", NumberOfSlot=20, Description="Learn the basics of programming", Term=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, Status=1 },
           new Subject { SubjectId="CEA201", MajorId="SE", SubjectName="Computer Organization and Architecture", NumberOfSlot=18, Description="Introduction to computer architecture", Term=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, Status=1 },
           new Subject { SubjectId="SSL101c", MajorId=null, SubjectName="Academic Skills for University Success", NumberOfSlot=15, Description="Skills for academic excellence", Term=1, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, Status=1 },
           new Subject { SubjectId="MAD101", MajorId="SE", SubjectName="Discrete Mathematics", NumberOfSlot=20, Description="Fundamentals of discrete math", Term=2, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, Status=1 },
           new Subject { SubjectId="NWC203c", MajorId="SE", SubjectName="Computer Networking", NumberOfSlot=18, Description="Introduction to networking concepts", Term=2, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, Status=1 },
           new Subject { SubjectId="DBI202", MajorId="SE", SubjectName="Database Systems", NumberOfSlot=20, Description="Database concepts and design", Term=3, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, Status=1 },
           new Subject { SubjectId="LAB211", MajorId="SE", SubjectName="OOP with Java Lab", NumberOfSlot=20, Description="Practice Object-Oriented Programming", Term=3, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, Status=1 },
           new Subject { SubjectId="PRJ301", MajorId="SE", SubjectName="Java Web Application Development", NumberOfSlot=25, Description="Developing web apps using Java", Term=4, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, Status=1 },
           new Subject { SubjectId="MAS291", MajorId="SE", SubjectName="Statistics & Probability", NumberOfSlot=20, Description="Introduction to statistics", Term=4, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, Status=1 },
           new Subject { SubjectId="SWP391", MajorId="SE", SubjectName="Software Development Project", NumberOfSlot=30, Description="Capstone project in software development", Term=5, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, Status=1 },
           new Subject { SubjectId="PRN231", MajorId="SE", SubjectName="Building Cross-Platform Back-End Application With .NET", NumberOfSlot=20, Description="Developing cross-platform applications", Term=7, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, Status=0 },
           new Subject { SubjectId="MLN111", MajorId=null, SubjectName="Philosophy of Marxism – Leninism", NumberOfSlot=16, Description="Core principles of Marxism-Leninism", Term=8, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, Status=2 },
           new Subject { SubjectId="WDU203c", MajorId="SE", SubjectName="UI/UX Design", NumberOfSlot=18, Description="Introduction to UI/UX principles", Term=8, CreatedAt=DateTime.Now, UpdatedAt=DateTime.Now, Status=1 }
           //Another Major Subject
           );
            }
        // Phương thức seed dữ liệu ClassSubject sau khi Subject đã tồn tại
        private void SeedClassSubjects(ModelBuilder modelBuilder)
            {
            modelBuilder.Entity<ClassSubject>().HasData(
                new ClassSubject { ClassSubjectId=1, ClassId="SE0001", SubjectId="PRM392", SemesterId="SP25", MajorId="SE", Term=1, CreatedAt=new DateTime(2024, 11, 25), Status=1 },
                new ClassSubject { ClassSubjectId=2, ClassId="SE0002", SubjectId="PRN231", SemesterId="SP25", MajorId="SE", Term=1, CreatedAt=new DateTime(2024, 11, 25), Status=1 },
                new ClassSubject { ClassSubjectId=3, ClassId="SE0003", SubjectId="MLN122", SemesterId="SP25", MajorId="SE", Term=1, CreatedAt=new DateTime(2024, 11, 25), Status=1 }
                );
            }
        }
    }
