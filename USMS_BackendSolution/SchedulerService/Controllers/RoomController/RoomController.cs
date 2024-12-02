using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerDataAccess.Services.RoomServices;

namespace SchedulerService.Controllers.RoomController
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;
        public RoomController(RoomService roomService)
        {
            _roomService = roomService;
        }

        // GET: api/Rooms
        [HttpGet]
        public APIResponse GetAllRoom()
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _roomService.GetAllRooms();
            return aPIResponse;
        }

        //GET: api/Rooms/{Id}
        [HttpGet("{id}")]
        public APIResponse GetRoomById(string id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _roomService.GetRoomById(id);
            return aPIResponse;
        }

        //POST: api/Rooms
        [HttpPost]
        public APIResponse AddNewRoom(RoomDTO roomDTO)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _roomService.AddNewRoom(roomDTO);
            return aPIResponse;
        }

        //PUT: api/Rooms
        [HttpPut]
        public APIResponse UpdateRoom(RoomDTO room)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _roomService.UpdateRoom(room);
            return aPIResponse;
        }

        //Get: api/Rooms/Disable/{Id}
        [HttpGet("Disable/{id}")]
        public APIResponse ChangeStatusRoomDisable(string id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _roomService.ChangeStatusRoomDisable(id);
            return aPIResponse;
        }

        //Get: api/Rooms/Available/{Id}
        [HttpGet("Available/{id}")]
        public APIResponse ChangeStatusRoomAvailable(string id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _roomService.ChangeStatusRoomAvailable(id);
            return aPIResponse;
        }

        //Get: api/Rooms/Maintenance/{Id}
        [HttpGet("Maintenance/{id}")]
        public APIResponse ChangeStatusRoomMaintenance(string id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _roomService.ChangeStatusRoomMaintenance(id);
            return aPIResponse;
        }
    }
}
