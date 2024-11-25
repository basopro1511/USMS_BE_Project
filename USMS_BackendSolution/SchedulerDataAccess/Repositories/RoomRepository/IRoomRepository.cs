using SchedulerBusinessObject.ModelDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerDataAccess.Repositories.RoomRepository
{
    public interface IRoomRepository
    {
        Task<IEnumerable<RoomDTO>> GetAllRoomsAsync();         
        Task<RoomDTO> GetRoomByIdAsync(string roomId);          
        Task AddRoomAsync(RoomDTO roomDTO);                    
        Task UpdateRoomAsync(RoomDTO roomDTO);                   
        Task DisableRoomAsync(string roomId);                    
    }

}
