using ISUZU_NEXT.Server.Core.Extentions;
using NuGet.DependencyResolver;
using SchedulerBusinessObject.AppDBContext;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;

namespace SchedulerService.Repository.ExamScheduleRepository
{
    public class ExamScheduleRepository : IExamScheduleRepository
    {
        /// <summary>
        /// Get All Exam Schedule
        /// </summary>
        /// <returns>A list of all Class Subject</returns>
        /// <exception cref="Exception"></exception>
        public List<ExamScheduleDTO> GetAllExamSchedule()
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    List<ExamSchedule> examSchedules = dbContext.ExamSchedule.ToList();
                    List<ExamScheduleDTO> examScheduleDTOs = new List<ExamScheduleDTO>();
                    foreach (var examSchedule in examSchedules)
                    {
                        ExamScheduleDTO examScheduleDTO = new ExamScheduleDTO();
                        examScheduleDTO.CopyProperties(examSchedule);
                        examScheduleDTOs.Add(examScheduleDTO);
                    }
                    return examScheduleDTOs;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Retrieves all exam schedules that do not have a teacher assigned.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<ExamScheduleDTO> GetUnassignedTeacherExamSchedules()
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var allExamSchedules = GetAllExamSchedule();
                    var examSchedules = allExamSchedules.Where(x => x.TeacherId == null).ToList();
                    List<ExamScheduleDTO> examScheduleDTOs = new List<ExamScheduleDTO>();
                    foreach (var examSchedule in examSchedules)
                    {
                        ExamScheduleDTO examScheduleDTO = new ExamScheduleDTO();
                        examScheduleDTO.CopyProperties(examSchedule);
                        examScheduleDTOs.Add(examScheduleDTO);
                    }
                    return examScheduleDTOs;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Retrieves all exam schedules that do not have a room assigned.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<ExamScheduleDTO> GetUnassignedRoomExamSchedules()
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var allExamSchedules = GetAllExamSchedule();
                    var examSchedules = allExamSchedules.Where(x => x.RoomId == null).ToList();
                    List<ExamScheduleDTO> examScheduleDTOs = new List<ExamScheduleDTO>();
                    foreach (var examSchedule in examSchedules)
                    {
                        ExamScheduleDTO examScheduleDTO = new ExamScheduleDTO();
                        examScheduleDTO.CopyProperties(examSchedule);
                        examScheduleDTOs.Add(examScheduleDTO);
                    }
                    return examScheduleDTOs;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Get available room 
        /// Input: DayStart, Time Start, Time End 
        /// Output: List Room not be using and available to choose
        /// </summary>
        /// <param name="date"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Room> GetAvailableRooms(DateOnly date, TimeOnly startTime, TimeOnly endTime)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var rooms = dbContext.Room.Where(r => r.Status == 1 && r.isOnline == false  && !dbContext.ExamSchedule. // Check room status is avaialbe or not 
                    Any(es => es.RoomId == r.RoomId &&es.Date == date &&                             
                                       es.StartTime < endTime &&                                    // Determine if the time is overlap
                                       es.EndTime > startTime))                                     
                        .ToList();
                    return rooms;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching available rooms: " + ex.Message);
            }
        }

        public bool AddNewExamSchedule(ExamScheduleDTO examScheduleDTO)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var examSchedule = new ExamSchedule();
                    examSchedule.CopyProperties(examScheduleDTO);
                    examSchedule.CreatedAt = DateTime.Now;
                    dbContext.ExamSchedule.Add(examSchedule);
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        //public List<TeacherDTO> GetAvailableTeachers(DateOnly date, TimeOnly startTime, TimeOnly endTime)
        //{
        //    try
        //    {
        //        using (var dbContext = new MyDbContext())
        //        {
        //            var teachers = dbContext.Teacher
        //                .Where(t => !dbContext.ExamSchedule
        //                    .Any(es => es.TeacherId == t.TeacherId &&
        //                               es.Date == date &&
        //                               es.StartTime < endTime &&
        //                               es.EndTime > startTime))
        //                .ToList();
        //            return teachers;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error fetching available teachers: " + ex.Message);
        //    }
        //}

    }
}
