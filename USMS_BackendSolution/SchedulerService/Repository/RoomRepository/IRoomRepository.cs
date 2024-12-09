using SchedulerBusinessObject.ModelDTOs;
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
        public bool ChangeStatusRoomDisable(string roomId);
        public bool ChangeStatusRoomAvailable(string roomId);
        public bool ChangeStatusRoomMaintenance(string roomId);
    }
}
