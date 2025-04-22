using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using SchedulerBusinessObject.AppDBContext;
using SchedulerBusinessObject.SchedulerModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchedulerService.Repository.SlotRepository
    {
    public class SlotRepository : ISlotRepository
        {
        #region Get all Time Slot
        /// <summary>
        /// Get All Time Slot
        /// </summary>
        /// <returns>A list of all TimeSlot model</returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<TimeSlot>> GetAllTimeSlot()
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    List<TimeSlot> timeSlots = await dbContext.TimeSlot.ToListAsync();
                    return timeSlots;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Time Slot By Id
        /// <summary>
        /// Get Time Slot by Slot ID
        /// </summary>
        /// <param name="id">Slot Id</param>
        /// <returns>A TimeSlot model corresponding to the given id</returns>
        /// <exception cref="Exception"></exception>
        public async Task<TimeSlot> GetTimeSlotById(int id)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    TimeSlot timeSlot = await dbContext.TimeSlot.FirstOrDefaultAsync(x => x.SlotId==id);
                    return timeSlot;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Add new Time Slot
        /// <summary>
        /// Create new Time Slot
        /// </summary>
        /// <param name="timeSlot">TimeSlot model object</param>
        /// <returns>true if success</returns>
        public async Task<bool> AddNewTimeSlot(TimeSlot timeSlot)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    timeSlot.Status=0; // Khi vừa tạo, mặc định trạng thái là vô hiệu hóa
                    await dbContext.TimeSlot.AddAsync(timeSlot);
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

        #region Update Time Slot
        /// <summary>
        /// Update Time Slot
        /// </summary>
        /// <param name="timeSlot">TimeSlot model object with updated data</param>
        /// <returns>true if success</returns>
        public async Task<bool> UpdateTimeSlot(TimeSlot timeSlot)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existingTimeSlot = await dbContext.TimeSlot.FirstOrDefaultAsync(x => x.SlotId==timeSlot.SlotId);
                    if (existingTimeSlot==null)
                        {
                        return false;
                        }
                    // Cập nhật các thuộc tính từ timeSlot mới vào đối tượng đã có
                    existingTimeSlot.CopyProperties(timeSlot);
                    dbContext.Entry(existingTimeSlot).State=EntityState.Modified;
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

        #region Update Time Slot Status
        /// <summary>
        /// Change Time Slot status
        /// </summary>
        /// <param name="slotId">Slot Id</param>
        /// <param name="newStatus">New status value</param>
        /// <returns>true if success</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> ChangeTimeSlotStatus(int slotId, int newStatus)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existingTimeSlot = await dbContext.TimeSlot.FirstOrDefaultAsync(x => x.SlotId==slotId);
                    if (existingTimeSlot==null)
                        {
                        return false;
                        }
                    existingTimeSlot.Status=newStatus;
                    dbContext.Entry(existingTimeSlot).State=EntityState.Modified;
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
        }
    }
