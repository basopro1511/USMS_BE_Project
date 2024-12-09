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
                    Room rooms = new Room();
                    rooms.CopyProperties(updateRoomDTO);
                    if (existingRoom == null)
                    {
                        return false;
                    }
                    dbContext.Entry(rooms).State = EntityState.Modified;
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
        /// Set room to disable
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool ChangeStatusRoomDisable(string roomId)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingRoom = GetRoomById(roomId);
                    Room rooms = new Room();
                    rooms.CopyProperties(existingRoom);
                    if (rooms == null)
                    {
                        return false;
                    }
                    rooms.Status = 0;
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
        /// <summary>
        /// Set room to available
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool ChangeStatusRoomAvailable(string roomId)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingRoom = GetRoomById(roomId);
                    Room rooms = new Room();
                    rooms.CopyProperties(existingRoom);
                    if (rooms == null)
                    {
                        return false;
                    }
                    rooms.Status = 1;
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
        /// <summary>
        /// Set room to maintenance
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool ChangeStatusRoomMaintenance(string roomId)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingRoom = GetRoomById(roomId);
                    Room rooms = new Room();
                    rooms.CopyProperties(existingRoom);
                    if (rooms == null)
                    {
                        return false;
                    }
                    rooms.Status = 2;
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
