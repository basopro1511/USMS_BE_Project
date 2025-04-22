using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using SchedulerBusinessObject.AppDBContext;
using SchedulerBusinessObject.SchedulerModels;

namespace Repositories.RequestScheduleRepository
    {
    public class RequestRepository : IRequestRepository
        {
        #region Get all Request Schedule
        /// <summary>
        /// Get All Request Schedule
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<RequestSchedule>> GetRequestSchedules()
            {
            try
                {
                using (var _db = new MyDbContext())
                    {
                        return await _db.RequestSchedule.ToListAsync();
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Request Schedule by Id
        /// <summary>
        /// Get Request Schedule Data by Request Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<RequestSchedule> GetRequestScheduleById(int id)
            {
            try
                {
                using (var _db = new MyDbContext())
                    {
                    return await _db.RequestSchedule.FirstOrDefaultAsync(x=> x.RequestId == id);
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Add Request  
        /// <summary>
        /// Add New Request
        /// </summary>
        /// <param name="requestSchedule"></param>
        /// <returns></returns>
        public async Task<bool> CreateRequest(RequestSchedule requestSchedule)
            {
            try
                {
                using (var _db = new MyDbContext())
                    {
                     await _db.AddAsync(requestSchedule);
                     await _db.SaveChangesAsync();
                    return true;
                        }
                    }
            catch (Exception ex)
                {
                throw new(ex.Message);
                }
            }
        #endregion

        #region Change Request Status
        /// <summary>
        /// Change Requset Status
        /// </summary>
        /// <param name="requestId"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> ChangeRequestStatus(int requestId, int newStatus)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existing = await GetRequestScheduleById(requestId);
                    if (existing==null)
                        {
                        return false;
                        }
                    existing.Status=newStatus;
                    dbContext.Entry(existing).State=EntityState.Modified;
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

        #region Update Request
        public async Task<bool> UpdateRequest(RequestSchedule update)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existing = await GetRequestScheduleById(update.RequestId);
                    if (existing==null)
                        {
                        return false;
                        }
                    existing.CopyProperties(update);
                    dbContext.Entry(existing).State=EntityState.Modified;
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
        }
    }
