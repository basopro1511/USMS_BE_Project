using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.RoomRepository
{
    public interface IRoomRepository
    {
        public Task<List<RoomDTO>> GetAllRooms();
        public Task<RoomDTO> GetRoomById(string id);
        public Task<bool> AddNewRoom(RoomDTO roomDTO);
        public Task<bool> UpdateRoom(RoomDTO updateRoomDTO);
        public Task<bool> DeleteRoom(string id);
        public Task<bool> ChangeRoomStatus(string roomId, int newStatus);
        public Task<List<Room>> GetAvailableRooms(DateOnly date, TimeOnly startTime, TimeOnly endTime);
        }
    }
