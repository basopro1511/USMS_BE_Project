using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchedulerBusinessObject.SchedulerModels;

namespace SchedulerBusinessObject.AppDBContext
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

        public virtual DbSet<Schedule> Schedule { get; set; }
        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<TimeSlot> TimeSlot { get; set; }
        public virtual DbSet<ExamSchedule> ExamSchedule { get; set; }
        public virtual DbSet<StudentInExamSchedule> StudentInExamSchedule { get; set; }
        public virtual DbSet<RequestSchedule> RequestSchedule { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            //Create Default Time Slots
            modelBuilder.Entity<TimeSlot>().HasData(
               new TimeSlot { SlotId=1, StartTime=TimeOnly.FromTimeSpan(new TimeSpan(7, 0, 0)), EndTime=TimeOnly.FromTimeSpan(new TimeSpan(9, 15, 0)), Status=1 },
               new TimeSlot { SlotId=2, StartTime=TimeOnly.FromTimeSpan(new TimeSpan(9, 30, 0)), EndTime=TimeOnly.FromTimeSpan(new TimeSpan(11, 45, 0)), Status=1 },
               new TimeSlot { SlotId=3, StartTime=TimeOnly.FromTimeSpan(new TimeSpan(13, 0, 0)), EndTime=TimeOnly.FromTimeSpan(new TimeSpan(15, 15, 0)), Status=1 },
               new TimeSlot { SlotId=4, StartTime=TimeOnly.FromTimeSpan(new TimeSpan(15, 30, 0)), EndTime=TimeOnly.FromTimeSpan(new TimeSpan(17, 45, 0)), Status=1 },
               new TimeSlot { SlotId=5, StartTime=TimeOnly.FromTimeSpan(new TimeSpan(18, 0, 0)), EndTime=TimeOnly.FromTimeSpan(new TimeSpan(20, 15, 0)), Status=1 }
            );
            //Create Example Rooms, Status = 0 là đang disable, 1 là đang available, 2 là đang trì hoãn ( maintenance )
            modelBuilder.Entity<Room>().HasData(
               new Room { RoomId="G201", Location="Grammar Room 201", Status=1, CreateAt=DateTime.Now, UpdateAt=DateTime.Now },
               new Room { RoomId="G202", Location="Grammar Room 202", Status=1, CreateAt=DateTime.Now, UpdateAt=DateTime.Now },
               new Room { RoomId="G203", Location="Grammar Room 203", Status=1, CreateAt=DateTime.Now, UpdateAt=DateTime.Now },
               new Room { RoomId="G204", Location="Grammar Room 204", Status=1, CreateAt=DateTime.Now, UpdateAt=DateTime.Now },
               new Room { RoomId="G205", Location="Grammar Room 205", Status=1, CreateAt=DateTime.Now, UpdateAt=DateTime.Now },
               new Room { RoomId="G301", Location="Grammar Room 301", Status=1, CreateAt=DateTime.Now, UpdateAt=DateTime.Now },
               new Room { RoomId="G302", Location="Grammar Room 302", Status=1, CreateAt=DateTime.Now, UpdateAt=DateTime.Now },
               new Room { RoomId="G303", Location="Grammar Room 303", Status=1, CreateAt=DateTime.Now, UpdateAt=DateTime.Now },
               new Room { RoomId="G304", Location="Grammar Room 304", Status=1, CreateAt=DateTime.Now, UpdateAt=DateTime.Now },
               new Room { RoomId="G305", Location="Grammar Room 305", Status=1, CreateAt=DateTime.Now, UpdateAt=DateTime.Now }
            );

            //modelBuilder.Entity<ExamSchedule>().HasData(
            //  new ExamSchedule { ExamScheduleId = 1, SemesterId = "FA25", MajorId = "IT", SubjectId = "PRN231", RoomId = "G304", Date = DateOnly.Parse("02-07-2025"),
            //  StartTime = TimeOnly.FromTimeSpan(new TimeSpan(7,0,0)), EndTime = TimeOnly.FromTimeSpan(new TimeSpan(9, 15, 0)),TeacherId="HNT", Status = 1, CreatedAt = DateTime.Now }    
            //);
            }
        }
    }
