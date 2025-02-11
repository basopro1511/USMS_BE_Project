using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
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
        /// Retrives information of Exam Schedule by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ExamScheduleDTO GetExamScheduleById(int id)
        {
            try
            {
                var examSchedules = GetAllExamSchedule();
                ExamScheduleDTO examScheduleDTO = examSchedules.FirstOrDefault(x => x.ExamScheduleId == id);
                return examScheduleDTO;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Update Teacher to Exam Schedule
        /// </summary>
        /// <param name="updateDTO"></param>
        /// <returns></returns>
        public bool AssignTeacherToExamSchedule(int examScheduleId, string teacherId)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingExamSchedule = GetExamScheduleById(examScheduleId);
                    ExamSchedule examSchedule = new ExamSchedule();
                    examSchedule.CopyProperties(existingExamSchedule);
                    if (existingExamSchedule == null)
                    {
                        return false;
                    }
                    examSchedule.TeacherId = teacherId;
                    dbContext.Entry(examSchedule).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Update Room in ExamSchedule
        /// </summary>
        /// <param name="updateDTO"></param>
        /// <returns></returns>
        public bool AssignRooomToExamSchedule(int examScheduleId, string roomId)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingExamSchedule = GetExamScheduleById(examScheduleId);
                    ExamSchedule examSchedule = new ExamSchedule();
                    examSchedule.CopyProperties(existingExamSchedule);
                    if (existingExamSchedule == null)
                    {
                        return false;
                    }
                    examSchedule.RoomId = roomId;
                    dbContext.Entry(examSchedule).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
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
                    var rooms = dbContext.Room.Where(r => r.Status == 1 && !dbContext.ExamSchedule. // Check room status is avaialbe or not 
                    Any(es => es.RoomId == r.RoomId &&es.Date == date &&                             
                                       es.StartTime < endTime &&                                    // Determine if the time is overlap
                                       es.EndTime > startTime))                                     
                        .ToList();
                    return rooms;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// Add new Exam Schedule
        /// </summary>
        /// <param name="examScheduleDTO"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Change Status of Exam Schedule Status
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool ChangeExamScheduleStatus(int id, int newStatus)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingExamSchedule = GetExamScheduleById(id);
                    ExamSchedule examSchedule = new ExamSchedule();
                    examSchedule.CopyProperties(existingExamSchedule);
                    if (existingExamSchedule == null)
                    {
                        return false;
                    }
                    examSchedule.Status = newStatus;
                    dbContext.Entry(examSchedule).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
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
        //        throw new Exception(ex.Message);
        //    }
        //}

    }
}
