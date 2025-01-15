using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SchedulerBusinessObject.SchedulerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {   
            //Create Default Time Slots
            modelBuilder.Entity<TimeSlot>().HasData(
               new TimeSlot { SlotId = 1, StartTime = TimeOnly.FromTimeSpan(new TimeSpan(7, 0, 0)), EndTime = TimeOnly.FromTimeSpan(new TimeSpan(9, 15, 0)),Status=1 },
               new TimeSlot { SlotId = 2, StartTime = TimeOnly.FromTimeSpan(new TimeSpan(9, 30, 0)), EndTime = TimeOnly.FromTimeSpan(new TimeSpan(11, 45, 0)), Status = 1 },
               new TimeSlot { SlotId = 3, StartTime = TimeOnly.FromTimeSpan(new TimeSpan(13, 0, 0)), EndTime = TimeOnly.FromTimeSpan(new TimeSpan(15, 15, 0)), Status = 1 },
               new TimeSlot { SlotId = 4, StartTime = TimeOnly.FromTimeSpan(new TimeSpan(15, 30, 0)), EndTime = TimeOnly.FromTimeSpan(new TimeSpan(17, 45, 0)), Status = 1 },
               new TimeSlot { SlotId = 5, StartTime = TimeOnly.FromTimeSpan(new TimeSpan(18, 0, 0)), EndTime = TimeOnly.FromTimeSpan(new TimeSpan(20, 15, 0)), Status = 1 }
            );
            //Create Example Rooms, Status = 0 là đang disable, 1 là đang available, 2 là đang trì hoãn ( maintenance )
            modelBuilder.Entity<Room>().HasData(
              new Room {RoomId="G304", Location="Grammar Room 304", isOnline=false, OnlineURL=null, Status = 1, CreateAt=DateTime.Now, UpdateAt= DateTime.Now  },
              new Room { RoomId = "R.ON", Location = "Online", isOnline = true, OnlineURL = "https://meet.google.com/koi-kghw-tsy", Status = 1, CreateAt = DateTime.Now, UpdateAt = DateTime.Now }
           );
            //modelBuilder.Entity<ExamSchedule>().HasData(
            //  new ExamSchedule { ExamScheduleId = 1, SemesterId = "FA25", MajorId = "IT", SubjectId = "PRN231", RoomId = "G304", Date = DateOnly.Parse("02-07-2025"),
            //  StartTime = TimeOnly.FromTimeSpan(new TimeSpan(7,0,0)), EndTime = TimeOnly.FromTimeSpan(new TimeSpan(9, 15, 0)),TeacherId="HNT", Status = 1, CreatedAt = DateTime.Now }    
            //);
        }
    }
}
