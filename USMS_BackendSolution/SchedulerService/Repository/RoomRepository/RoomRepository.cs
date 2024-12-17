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
        public List<RoomDTO> GetAllRooms()
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    List<Room> rooms = dbContext.Room.ToList();
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
        public RoomDTO GetRoomById(string id)
        {
            try
            {
                var rooms = GetAllRooms();
                RoomDTO roomDTO = rooms.FirstOrDefault(x => x.RoomId == id);
                return roomDTO;
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
        public bool AddNewRoom(RoomDTO roomDTO)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var room = new Room();
                    room.CopyProperties(roomDTO);
                    room.CreateAt = DateTime.Now;
                    dbContext.Room.Add(room);
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
        /// Update Room's Infor
        /// </summary>
        /// <param name="updateRoomDTO"></param>
        /// <returns>true if success</returns>
        public bool UpdateRoom(RoomDTO updateRoomDTO)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingRoom = GetRoomById(updateRoomDTO.RoomId);
                    Room room = new Room();
                    room.CopyProperties(updateRoomDTO);
                    if (existingRoom == null)
                    {
                        return false;
                    }
                    room.UpdateAt = DateTime.Now;                            
                    dbContext.Entry(room).State = EntityState.Modified;
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
        /// Delete a Room by its ID
        /// </summary>
        /// <param name="id">Room ID</param>
        /// <returns>true if success</returns>
        public bool DeleteRoom(string id)
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
        /// Change the status of a room
        /// </summary>
        /// <param name="roomId">Room ID</param>
        /// <param name="newStatus">New status to set ( 0 = Disable, 1 = Available, 2 = Maintenance)</param>
        /// <returns>true if success</returns>
        /// <exception cref="Exception"></exception>
        public bool ChangeRoomStatus(string roomId, int newStatus)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingRoom = GetRoomById(roomId);
                    Room rooms = new Room();
                    rooms.CopyProperties(existingRoom);
                    if (existingRoom == null)
                    {
                        return false;
                    }
                    rooms.Status = newStatus;
                    dbContext.Entry(rooms).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
