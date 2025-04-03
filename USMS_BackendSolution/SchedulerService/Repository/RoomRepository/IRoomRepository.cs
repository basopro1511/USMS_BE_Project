using SchedulerBusinessObject.SchedulerModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.RoomRepository
    {
    public interface IRoomRepository
        {
        Task<List<Room>> GetAllRooms();
        Task<Room> GetRoomById(string id);
        Task<bool> AddNewRoom(Room room);
        Task<bool> UpdateRoom(Room updateRoom);
        Task<bool> DeleteRoom(string id);
        Task<bool> ChangeRoomStatus(string roomId, int newStatus);
        Task<List<Room>> GetAvailableRooms(DateOnly date, TimeOnly startTime, TimeOnly endTime);
        Task<bool> ChangeRoomStatusSelected(List<string> roomIds, int status);
        }
    }
