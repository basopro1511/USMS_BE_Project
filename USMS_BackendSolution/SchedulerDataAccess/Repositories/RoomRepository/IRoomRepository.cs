using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerDataAccess.Repositories.RoomRepository
{
    public interface IRoomRepository
    {
        public List<RoomDTO> GetAllRooms();
        public RoomDTO GetRoomById(string id);
        public RoomDTO GetExistingRoom(string roomId);

        public bool AddNewRoom(RoomDTO roomDTO);
        public bool UpdateRoom(RoomDTO updateRoomDTO);
        public bool ChangeStatusRoom(string id);
    }
}
