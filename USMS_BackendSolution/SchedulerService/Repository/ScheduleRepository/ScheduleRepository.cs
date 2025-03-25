using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using SchedulerBusinessObject.AppDBContext;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;

namespace Repositories.ScheduleRepository
    {
    public class ScheduleRepository : IScheduleRepository
        {

        #region Get all Schedule
        /// <summary>
        /// Method Test To get all Schedule
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Schedule>> getAllSchedule()
            {
            try
                {
                using (var _db = new MyDbContext())
                    {
                    List<Schedule> schedules = await _db.Schedule.ToListAsync();
                    return schedules;
                    }
                }
            catch(Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Schedule by Id
        /// <summary>
        /// Get Schedule By ClassScheduleId
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ScheduleDTO> GetScheduleById(int classScheduleId)
            {
            try
                {
                using (var _db = new MyDbContext())
                    {
                    Schedule? schedule = await _db.Schedule.FirstOrDefaultAsync(x=> x.ScheduleId == classScheduleId);
                    ScheduleDTO scheduleDto = new ScheduleDTO();
                    scheduleDto.CopyProperties(schedule);
                    return scheduleDto;
                    }
                }
            catch(Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Add Schedule
        /// <summary>
        /// Add Schedule
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task AddSchedule(Schedule schedule)
            {
            try
                {
                using(var dbContext = new MyDbContext())
                    {
                    await dbContext.Schedule.AddAsync(schedule);
                    dbContext.SaveChanges();
                    }
                }
            catch(Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Schedules By Date and Slot
        /// <summary>
        /// Get Schedules By Date And Slot
        /// </summary>
        /// <param name="date"></param>
        /// <param name="slotId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<Schedule> GetSchedulesByDateAndSlot(DateOnly date, int slotId)
            {
            try
                {
                using(var dbContext = new MyDbContext())
                    {
                    return  dbContext.Schedule.Where(s => s.Status == 1 &&
                                                                s.Date == date &&
                                                                s.SlotId == slotId).ToList();
                    }
                }
            catch(Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get ClassSchedules by ClassSubjectIds
        public async Task<List<ViewScheduleDTO>> GetClassSchedulesByClassSubjectIds(List<int> classSubjectIds)
            {
            try
                {
                using(var _dbContext = new MyDbContext())
                    {
        
                    var schedules = await _dbContext.Schedule
                        .Where(cs => classSubjectIds.Contains(cs.ClassSubjectId))
                        .ToListAsync();
                    List<ViewScheduleDTO> classScheduleDTOs = new List<ViewScheduleDTO>();
                    foreach(var item in schedules)
                        {
                        ViewScheduleDTO classScheduleDTO = new ViewScheduleDTO();
                        classScheduleDTO.CopyProperties(item);
                        classScheduleDTOs.Add(classScheduleDTO);
                        }
                    return classScheduleDTOs;
                    }
                }
            catch(Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get ClassSchedules by ClassSubjectId
        public async Task<List<ViewScheduleDTO>> GetClassSchedulesByClassSubjectId(int classSubjectId)
            {
            try
                {
                using(var _dbContext = new MyDbContext())
                    {
                    List<Schedule> schedules =await _dbContext.Schedule.Where(cs => classSubjectId == cs.ClassSubjectId).ToListAsync();
                    List<ViewScheduleDTO> classScheduleDTOs = new List<ViewScheduleDTO>();
                    foreach(var item in schedules)
                        {
                        ViewScheduleDTO classScheduleDTO = new ViewScheduleDTO();
                        classScheduleDTO.CopyProperties(item);
                        classScheduleDTOs.Add(classScheduleDTO);
                        }
                    return classScheduleDTOs;
                    }
                }
            catch(Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Schedules By ClassSubjectId
        /// <summary>
        /// Lấy danh sách lịch (Schedule) theo ClassSubjectId
        /// </summary>
        /// <param name="classSubjectId">Id của ClassSubject</param>
        /// <returns>Danh sách Schedule</returns>
        public async Task<List<Schedule>> GetSchedulesByClassSubjectId(int classSubjectId)
            {
            try
                {
                using (var _db = new MyDbContext())
                    {
                    List<Schedule> schedules = await _db.Schedule.Where(s => s.ClassSubjectId==classSubjectId).ToListAsync();
                    return schedules;
                    }
                }
            catch(Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Update Schedule
        /// <summary>
        /// Update Schedule
        /// </summary>
        /// <param name="scheduleDto"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> UpdateSchedule(ScheduleDTO scheduleDto)
            {
            try
                {
                using(var dbContext = new MyDbContext())
                    {
                    var schedule =await dbContext.Schedule.FirstOrDefaultAsync(x => x.ScheduleId == scheduleDto.ClassScheduleId);
                    if(schedule == null)
                        return false;
                    schedule.CopyProperties(scheduleDto); 
                    dbContext.Entry(schedule).State = EntityState.Modified;
                 await   dbContext.SaveChangesAsync();
                    }
                return true;
                }
            catch(Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Delete Schedule
        /// <summary>
        /// Delete Schedule bởi Id
        /// </summary>
        /// <param name="schedule"></param>
        public async Task<bool> DeleteScheduleById(int scheduleId)
            {
            try
                {
                using(var dbContext = new MyDbContext())
                    {                                                                                   
                    var schedule =await dbContext.Schedule.FirstOrDefaultAsync(s => s.ScheduleId == scheduleId);
                    if(schedule == null)
                        return false; 
                    dbContext.Schedule.Remove(schedule);
                   await dbContext.SaveChangesAsync();
                    return true; 
                    }
                }
            catch(Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Schedules For Academic Staff
        /// <summary>
        /// Lấy danh sách lịch (Schedule) theo ClassSubjectId
        /// </summary>
        /// <param name="classSubjectId">Id của ClassSubject</param>
        /// <returns>Danh sách Schedule</returns>
        public async Task<List<ViewScheduleDTO>> GetClassSchedulesForStaff(List<int> classSubjectIds, DateTime startDay, DateTime endDay)
            {
            try
                {
                using (var _dbContext = new MyDbContext()) {
                var schedules = await _dbContext.Schedule
                         .Where(s => classSubjectIds.Contains(s.ClassSubjectId)
                                  &&s.Date>=DateOnly.FromDateTime(startDay)
                                  &&s.Date<=DateOnly.FromDateTime(endDay))
                         .Select(s => new ViewScheduleDTO
                             {
                             ClassScheduleId=s.ScheduleId,
                             ClassSubjectId=s.ClassSubjectId,
                             SlotId=s.SlotId,
                             TeacherId=s.TeacherId,
                             Date=s.Date,
                             Status=s.Status,
                             RoomId=s.RoomId,
                             SlotNoInSubject=s.SlotNoInSubject
                             })
                         .ToListAsync();
                return schedules;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Schedules For Student
        /// <summary>
        /// Lấy danh sách lịch (Schedule) theo ClassSubjectId
        /// </summary>
        /// <param name="classSubjectId">Id của ClassSubject</param>
        /// <returns>Danh sách Schedule</returns>
        public async Task<List<ViewScheduleDTO>> GetClassSchedulesForStudent(List<int> classSubjectIds, DateTime startDay, DateTime endDay)
            {
            try
                {
                using (var _dbContext = new MyDbContext())
                    {
                    var schedules =await _dbContext.Schedule
                         .Where(s => classSubjectIds.Contains(s.ClassSubjectId)
                                  &&s.Date>=DateOnly.FromDateTime(startDay)
                                  &&s.Date<=DateOnly.FromDateTime(endDay))
                         .Select(s => new ViewScheduleDTO
                             {
                             ClassScheduleId=s.ScheduleId,
                             ClassSubjectId=s.ClassSubjectId,
                             SlotId=s.SlotId,
                             TeacherId=s.TeacherId,
                             Date=s.Date,
                             Status=s.Status,
                             RoomId=s.RoomId,
                             SlotNoInSubject=s.SlotNoInSubject
                             })
                         .ToListAsync();
                    return schedules;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Schedule for Teacher   
        /// <summary>
        /// Get Schedule for teacher by Teacher Id and Range Day
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<ViewScheduleDTO>> GetScheduleForTeacher(string teacherId, DateTime startDay, DateTime endDay) {
            try
                {
                using (var dbcontext= new MyDbContext())
                    {
                    var schedules =await dbcontext.Schedule.Where(s => s.TeacherId==teacherId&&s.Date>=DateOnly.FromDateTime(startDay)
                                 &&s.Date<=DateOnly.FromDateTime(endDay)).ToListAsync();
                    List<ViewScheduleDTO> viewScheduleDTOs = new List<ViewScheduleDTO>();
                    foreach (var schedule in schedules)
                        {
                         ViewScheduleDTO viewScheduleDTO = new ViewScheduleDTO();
                        viewScheduleDTO.CopyProperties(schedule);
                        viewScheduleDTOs.Add(viewScheduleDTO);
                      await dbcontext.SaveChangesAsync();
                        }
                    return viewScheduleDTOs;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception (ex.Message);
                }
            }
        #endregion
        }
    }
