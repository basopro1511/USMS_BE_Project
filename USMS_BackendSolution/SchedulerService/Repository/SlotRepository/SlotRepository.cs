using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using SchedulerBusinessObject.AppDBContext;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;

namespace SchedulerService.Repository.SlotRepository
{
    public class SlotRepository : ISlotRepository
    {
        #region Get all Time Slot
        /// <summary>
        /// Get All Time Slot
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<TimeSlotDTO>> getAllTimeSlot()
        {
			try
			{
				using (var dbcontext = new MyDbContext())
				{
                    List<TimeSlot> timeSlots =await dbcontext.TimeSlot.ToListAsync();
                    List<TimeSlotDTO> timeSlotDTOs = new List<TimeSlotDTO>(); 
                    foreach (var timeSlot in timeSlots) { 
                    TimeSlotDTO timeSlotDTO = new TimeSlotDTO();
                        timeSlotDTO.CopyProperties(timeSlot);
                        timeSlotDTOs.Add(timeSlotDTO);
                    }
                    return timeSlotDTOs;
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
        }
        #endregion

        #region  Get Time Slot By Id
        /// <summary>
        /// Get Time Slot by Slot ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<TimeSlotDTO> GetTimeSlotById(int id)
        {
            try
            {
                using (var _db = new MyDbContext())
                    {
                    var timeSlot =await _db.TimeSlot.FirstOrDefaultAsync(x => x.SlotId==id); 
                    TimeSlotDTO? timeSlotDTO = new TimeSlotDTO();
                    timeSlotDTO.CopyProperties(timeSlot);
                    return timeSlotDTO;
                    }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region  Add new Time Slot
        /// <summary>
        /// Create new Time Slot
        /// </summary>
        /// <param name="timeSlotDTO"></param>
        /// <returns>>true if success</returns>
        public async Task<bool> AddNewTimeSlot(TimeSlotDTO timeSlotDTO)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var timeSlot = new TimeSlot();
                    timeSlot.CopyProperties(timeSlotDTO);
                    timeSlot.Status = 0; // Khi vừa tạo sẽ mặc định trạng thái là vô hiệu hóa
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

        #region  Update Time Slot
        /// <summary>
        /// Update Time Slot
        /// </summary>
        /// <param name="timeSlotDTO"></param>
        /// <returns>>true if success</returns>
        public async Task<bool> UpdateTimeSlot (TimeSlotDTO timeSlotDTO)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingTimeSlot = GetTimeSlotById(timeSlotDTO.SlotId);
                    TimeSlot timeSlot = new TimeSlot();
                    timeSlot.CopyProperties(timeSlotDTO);
                    if (existingTimeSlot == null)
                    {
                        return false;
                    }
                    dbContext.Entry(timeSlot).State = EntityState.Modified;
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
        /// <param name="slotId"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> ChangeTimeSlotStatus(int slotId, int newStatus)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingTimeSlot =await GetTimeSlotById(slotId);
                    TimeSlot timeSlot = new TimeSlot();
                    timeSlot.CopyProperties(existingTimeSlot);
                    if (existingTimeSlot == null)
                    {
                        return false;
                    }
                    timeSlot.Status = newStatus;
                    dbContext.Entry(timeSlot).State = EntityState.Modified;
                 await   dbContext.SaveChangesAsync();
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
