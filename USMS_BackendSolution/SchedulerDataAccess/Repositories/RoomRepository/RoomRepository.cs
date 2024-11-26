using SchedulerBusinessObject.SchedulerModels;
using SchedulerBusinessObject.AppDBContext;
using SchedulerBusinessObject.ModelDTOs;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerDataAccess.Repositories.RoomRepository
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
                    List<Rooms> rooms = dbContext.Rooms.ToList();
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
        public RoomDTO GetExistingRoom(string roomId)
        {
            try
            {
                var rooms = GetAllRooms();
                RoomDTO roomDTO = rooms.FirstOrDefault(x => x.RoomId == roomId);
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
                    var room = new Rooms();
                    room.CopyProperties(roomDTO);
                    dbContext.Rooms.Add(room);
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
                    var existingRoom = GetRoomToUpdate(updateRoomDTO.RoomId);
                    if (existingRoom == null)
                    {
                        return false;
                    }
                    existingRoom.CopyProperties(updateRoomDTO);
                    dbContext.Entry(existingRoom).State = EntityState.Modified;
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
        /// method use to get room to update 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Rooms GetRoomToUpdate(string id)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingRoom = dbContext.Rooms.FirstOrDefault(cs => cs.RoomId == id);
                    return existingRoom;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// update status for room
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool ChangeStatusRoom(string id)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    Rooms room = dbContext.Rooms.FirstOrDefault(x => x.RoomId == id);
                    room.Status = room.Status == 1 ? 0 : 1;
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
