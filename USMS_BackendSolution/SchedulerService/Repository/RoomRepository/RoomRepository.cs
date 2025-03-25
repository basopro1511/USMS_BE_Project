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
        public async Task<List<RoomDTO>> GetAllRooms()
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    List<Room> rooms =await dbContext.Room.ToListAsync();
                    List<RoomDTO> roomDTOs = new List<RoomDTO>();
                    foreach (var room in rooms)
                    {
                        RoomDTO RoomDTO = new RoomDTO();
                        RoomDTO.CopyProperties(room);
                        roomDTOs.Add(RoomDTO);
                    }
                    return roomDTOs;
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
        public async Task<RoomDTO> GetRoomById(string id)
        {
            try
            {
                using (var _db = new MyDbContext())
                    {
                    var room = await _db.Room.FirstOrDefaultAsync(x => x.RoomId==id);
                    if (room==null) { return null; }
                    RoomDTO roomDTO = new RoomDTO();
                    roomDTO.CopyProperties(room);
                    return roomDTO;
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
        public async Task<bool> AddNewRoom(RoomDTO roomDTO)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var room = new Room();
                    room.CopyProperties(roomDTO);
                    room.CreateAt = DateTime.Now;
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
        public async Task<bool> UpdateRoom(RoomDTO updateRoomDTO)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingRoom =await GetRoomById(updateRoomDTO.RoomId);
                    Room room = new Room();
                    room.CopyProperties(updateRoomDTO);
                    if (existingRoom == null)
                    {
                        return false;
                    }
                    room.UpdateAt = DateTime.Now;                            
                    dbContext.Entry(room).State = EntityState.Modified;
                  await  dbContext.SaveChangesAsync();
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
                    var existingRoom = GetRoomById(id);
                    Room rooms = new Room();
                    rooms.CopyProperties(existingRoom);
                    if (existingRoom == null)
                    {
                        return false;
                    }
                    dbContext.Room.Remove(rooms);
                  await  dbContext.SaveChangesAsync();
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
                    var existingRoom =await GetRoomById(roomId);
                    Room rooms = new Room();
                    rooms.CopyProperties(existingRoom);
                    if (existingRoom == null)
                    {
                        return false;
                    }
                    rooms.Status = newStatus;
                    dbContext.Entry(rooms).State = EntityState.Modified;
                  await  dbContext.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #region Get Available Rooms to Add Exam Schedule
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

        }
    }
