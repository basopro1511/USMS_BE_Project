using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using SchedulerBusinessObject.AppDBContext;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.RoomRepository
{
    public class RoomRepository : IRoomRepository
    {
        /// <summary>
        /// Get All Class Subjects
        /// </summary>
        /// <returns>A list of all Class Subject</returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Room>> GetAllRooms()
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    return await dbContext.Room.ToListAsync();
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }

        /// <summary>
        /// Get 1 Room by RoomId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>1 Room that had RoomId  matched</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Room> GetRoomById(string id)
            {
            try
                {
                using (var _db = new MyDbContext())
                    {
                    return await _db.Room.FirstOrDefaultAsync(x => x.RoomId==id);
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        /// <summary>
        /// Create new Room
        /// </summary>
        /// <param name="roomDTO"></param>
        /// <returns>>true if success</returns>
        public async Task<bool> AddNewRoom(Room room)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    room.CreateAt=DateTime.Now;
                    dbContext.Room.Add(room);
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception)
                {
                return false;
                }
            }

        /// <summary>
        /// Update Room's Infor
        /// </summary>
        /// <param name="updateRoomDTO"></param>
        /// <returns>true if success</returns>
        public async Task<bool> UpdateRoom(Room updateRoom)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existingRoom = await GetRoomById(updateRoom.RoomId);
                    if (existingRoom==null)
                        {
                        return false;
                        }
                    updateRoom.UpdateAt=DateTime.Now;
                    dbContext.Entry(updateRoom).State=EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception)
                {
                return false;
                }
            }
        /// <summary>
        /// Delete a Room by its ID
        /// </summary>
        /// <param name="id">Room ID</param>
        /// <returns>true if success</returns>
        public async Task<bool> DeleteRoom(string id)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existingRoom = await GetRoomById(id);
                    if (existingRoom==null)
                        {
                        return false;
                        }
                    dbContext.Room.Remove(existingRoom);
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception)
                {
                return false;
                }
            }

        /// <summary>
        /// Change the status of a room
        /// </summary>
        /// <param name="roomId">Room ID</param>
        /// <param name="newStatus">New status to set ( 0 = Disable, 1 = Available, 2 = Maintenance)</param>
        /// <returns>true if success</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> ChangeRoomStatus(string roomId, int newStatus)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existingRoom = await GetRoomById(roomId);
                    if (existingRoom==null)
                        {
                        return false;
                        }
                    existingRoom.Status=newStatus;
                    dbContext.Entry(existingRoom).State=EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message, ex);
                }
            }

        #region
        /// <summary>
        /// Change Room selected Status 
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> ChangeRoomStatusSelected(List<string> roomIds, int status)
            {
            try
                {
                using (var _db = new MyDbContext())
                    {
                    var Ids = await _db.Room.Where(x => roomIds.Contains(x.RoomId)).ToListAsync();
                    if (!Ids.Any())
                        return false;
                    foreach (var item in Ids)
                        {
                        item.Status=status;
                        }
                    await _db.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Add list Room 
        /// <summary>
        /// Add a list of Rooms from Excel
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> AddRoomsAsyncs(List<Room> models)
            {
            try
                {
                using (var _db = new MyDbContext())
                    {
                    await _db.Room.AddRangeAsync(models);
                    await _db.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion
        }
    }
