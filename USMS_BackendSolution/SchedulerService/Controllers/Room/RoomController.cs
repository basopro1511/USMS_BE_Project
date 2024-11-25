using Microsoft.AspNetCore.Mvc;
using SchedulerBusinessObject;
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
    }
}
