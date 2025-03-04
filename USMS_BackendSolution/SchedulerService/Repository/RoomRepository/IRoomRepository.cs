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
        public List<RoomDTO> GetAllRooms();
        public RoomDTO GetRoomById(string id);
        public bool AddNewRoom(RoomDTO roomDTO);
        public bool UpdateRoom(RoomDTO updateRoomDTO);
        public bool DeleteRoom(string id);
        public bool ChangeRoomStatus(string roomId, int newStatus);
        public Task<List<Room>> GetAvailableRooms(DateOnly date, TimeOnly startTime, TimeOnly endTime);
        }
    }
