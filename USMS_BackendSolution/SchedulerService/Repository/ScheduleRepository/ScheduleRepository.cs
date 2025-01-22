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
        public async Task<List<Schedule>?> GetSchedulesByDateAndSlot(DateOnly date, int slotId)
            {
            try
                {
                using(var dbContext = new MyDbContext())
                    {
                    return await dbContext.Schedule.Where(s => s.Status == 1 &&
                                                                s.Date == date &&
                                                                s.SlotId == slotId).ToListAsync();
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
        }
    }
