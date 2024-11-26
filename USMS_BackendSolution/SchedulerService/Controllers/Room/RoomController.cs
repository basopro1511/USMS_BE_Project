using Microsoft.AspNetCore.Mvc;
using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerDataAccess.Services.RoomServices;

namespace SchedulerService.Controllers.Room
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
        [HttpGet("{Id}")]
        public APIResponse GetRoomById(string Id)
        { 
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _roomService.GetRoomById(Id);
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
        }//PUT: api/Rooms/{Id}
        [HttpGet("/{Id}")]
        public APIResponse ChangeStatusRoom(string Id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _roomService.ChangeStatusRoom(Id);
            return aPIResponse;
        }
    }
}
