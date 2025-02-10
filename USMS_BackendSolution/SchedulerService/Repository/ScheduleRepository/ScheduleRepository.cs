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
        public List<Schedule> getAllSchedule()
            {
            try
                {
                var dbContext = new MyDbContext();
                List<Schedule> schedules = dbContext.Schedule.ToList();
                return schedules;
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
        public ScheduleDTO GetScheduleById(int classScheduleId)
            {
            try
                {
                var dbContext = new MyDbContext();
                Schedule schedule = getAllSchedule().FirstOrDefault(x => x.ScheduleId == classScheduleId);
                if(schedule == null)
                    {
                    return null; // Không tìm thấy lịch học => trả về null
                    }
                ScheduleDTO scheduleDto = new ScheduleDTO();
                scheduleDto.CopyProperties(schedule);
                return scheduleDto;
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
        public List<Schedule>? GetSchedulesByDateAndSlot(DateOnly date, int slotId)
            {
            try
                {
                using(var dbContext = new MyDbContext())
                    {
                    return dbContext.Schedule.Where(s => s.Status == 1 &&
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
        public List<ScheduleDTO> GetClassSchedulesByClassSubjectIds(List<int> classSubjectIds)
            {
            try
                {
                using(var _dbContext = new MyDbContext())
                    {
                    var schedules = _dbContext.Schedule
                        .Where(cs => classSubjectIds.Contains(cs.ClassSubjectId))
                        .ToList();
                    List<ScheduleDTO> classScheduleDTOs = new List<ScheduleDTO>();
                    foreach(var item in schedules)
                        {
                        ScheduleDTO classScheduleDTO = new ScheduleDTO();
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
        public List<ScheduleDTO> GetClassSchedulesByClassSubjectId(int classSubjectId)
            {
            try
                {
                using(var _dbContext = new MyDbContext())
                    {
                    List<Schedule> schedules = _dbContext.Schedule.Where(cs => classSubjectId == cs.ClassSubjectId).ToList();
                    List<ScheduleDTO> classScheduleDTOs = new List<ScheduleDTO>();
                    foreach(var item in schedules)
                        {
                        ScheduleDTO classScheduleDTO = new ScheduleDTO();
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
        public List<Schedule> GetSchedulesByClassSubjectId(int classSubjectId)
            {
            try
                {
                var dbContext = new MyDbContext();
                List<Schedule> schedules = dbContext.Schedule.Where(s => s.ClassSubjectId == classSubjectId)
                                 .ToList();
                return schedules;
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
        public bool UpdateSchedule(ScheduleDTO scheduleDto)
            {
            try
                {
                using(var dbContext = new MyDbContext())
                    {
                    var schedule = dbContext.Schedule.FirstOrDefault(x => x.ScheduleId == scheduleDto.ClassScheduleId);
                    if(schedule == null)
                        return false;
                    schedule.CopyProperties(scheduleDto); 
                    dbContext.Entry(schedule).State = EntityState.Modified;
                    dbContext.SaveChanges();
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
        public bool DeleteScheduleById(int scheduleId)
            {
            try
                {
                using(var dbContext = new MyDbContext())
                    {                                                                                   
                    var schedule = dbContext.Schedule.FirstOrDefault(s => s.ScheduleId == scheduleId);
                    if(schedule == null)
                        return false; 
                    dbContext.Schedule.Remove(schedule);
                    dbContext.SaveChanges();
                    return true; 
                    }
                }
            catch(Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion
        }
    }
