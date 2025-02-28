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
        #region GetAllExamSchedule
        /// <summary>
        /// Lấy toàn bộ Exam Schedule
        /// </summary>
        /// <returns>A list of all Class Subject</returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<ExamScheduleDTO>> GetAllExamSchedule()
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    List<ExamSchedule> examSchedules = await dbContext.ExamSchedule.ToListAsync();
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
        #endregion

        #region GetExamScheduleById
        /// <summary>
        /// Lấy thông tin Exam Schedule theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ExamScheduleDTO> GetExamScheduleById(int id)
            {
            try
                {
                var examSchedules = await GetAllExamSchedule();
                ExamScheduleDTO? examScheduleDTO = examSchedules.FirstOrDefault(x => x.ExamScheduleId==id) ?? new ExamScheduleDTO();
                return examScheduleDTO;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region AssignTeacherToExamSchedule
        /// <summary>
        /// Thêm Teacher vào Exam Schedule
        /// </summary>
        /// <param name="examScheduleId"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public async Task<bool> AssignTeacherToExamSchedule(int examScheduleId, string teacherId)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existingExamSchedule = await GetExamScheduleById(examScheduleId);
                    if (existingExamSchedule==null)
                        {
                        return false;
                        }

                    // Chuyển DTO -> Entity
                    ExamSchedule examSchedule = new ExamSchedule();
                    examSchedule.CopyProperties(existingExamSchedule);
                    examSchedule.TeacherId=teacherId;

                    dbContext.Entry(examSchedule).State=EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception)
                {
                return false;
                }
            }
        #endregion

        #region AssignRooomToExamSchedule
        /// <summary>
        /// Thêm Room vào ExamSchedule
        /// </summary>
        /// <param name="examScheduleId"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public async Task<bool> AssignRooomToExamSchedule(int examScheduleId, string roomId)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existingExamSchedule = await GetExamScheduleById(examScheduleId);
                    if (existingExamSchedule==null)
                        {
                        return false;
                        }

                    // Chuyển DTO -> Entity
                    ExamSchedule examSchedule = new ExamSchedule();
                    examSchedule.CopyProperties(existingExamSchedule);
                    examSchedule.RoomId=roomId;

                    dbContext.Entry(examSchedule).State=EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception)
                {
                return false;
                }
            }
        #endregion

        #region GetUnassignedTeacherExamSchedules
        /// <summary>
        /// Lấy danh sách ExamSchedule chưa có Teacher
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<ExamScheduleDTO>> GetUnassignedTeacherExamSchedules()
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var allExamSchedules = await GetAllExamSchedule();
                    var examSchedules = allExamSchedules.Where(x => x.TeacherId==null).ToList();

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
        #endregion

        #region GetUnassignedRoomExamSchedules
        /// <summary>
        /// Lấy danh sách ExamSchedule chưa có Room
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<ExamScheduleDTO>> GetUnassignedRoomExamSchedules()
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var allExamSchedules = await GetAllExamSchedule();
                    var examSchedules = allExamSchedules.Where(x => x.RoomId==null).ToList();

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
        #endregion

        #region GetAvailableRooms
        /// <summary>
        /// Lấy tất cả các phòng còn trống trong khoảng thời gian
        /// </summary>
        /// <param name="date"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Room>> GetAvailableRooms(DateOnly date, TimeOnly startTime, TimeOnly endTime)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var rooms = await dbContext.Room
                        .Where(r => r.Status==1&&
                                    !dbContext.ExamSchedule.Any(es => es.RoomId==r.RoomId&&
                                                                       es.Date==date&&
                                                                       es.StartTime<endTime&&
                                                                       es.EndTime>startTime))
                        .ToListAsync();
                    return rooms;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region AddNewExamSchedule
        /// <summary>
        /// Thêm mới 1 ExamSchedule
        /// </summary>
        /// <param name="examScheduleDTO"></param>
        /// <returns></returns>
        public async Task<bool> AddNewExamSchedule(ExamScheduleDTO examScheduleDTO)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var examSchedule = new ExamSchedule();
                    examSchedule.CopyProperties(examScheduleDTO);
                    examSchedule.CreatedAt=DateTime.Now;
                    await dbContext.ExamSchedule.AddAsync(examSchedule);
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception)
                {
                return false;
                }
            }
        #endregion

        #region ChangeExamScheduleStatus
        /// <summary>
        /// Thay đổi trạng thái của Exam Schedule
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> ChangeExamScheduleStatus(int id, int newStatus)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existingExamSchedule = await GetExamScheduleById(id);
                    if (existingExamSchedule==null)
                        {
                        return false;
                        }

                    // Chuyển DTO -> Entity
                    ExamSchedule examSchedule = new ExamSchedule();
                    examSchedule.CopyProperties(existingExamSchedule);
                    examSchedule.Status=newStatus;

                    dbContext.Entry(examSchedule).State=EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message, ex);
                }
            }
        #endregion

        /*
        #region GetAvailableTeachers
        /// <summary>
        /// Lấy danh sách Teacher còn trống trong khoảng thời gian
        /// </summary>
        /// <param name="date"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public async Task<List<Teacher>> GetAvailableTeachers(DateOnly date, TimeOnly startTime, TimeOnly endTime)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var teachers = await dbContext.Teacher
                        .Where(t => !dbContext.ExamSchedule.Any(es => es.TeacherId == t.TeacherId &&
                                                                      es.Date == date &&
                                                                      es.StartTime < endTime &&
                                                                      es.EndTime > startTime))
                        .ToListAsync();
                    return teachers;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        */
        }
    }
