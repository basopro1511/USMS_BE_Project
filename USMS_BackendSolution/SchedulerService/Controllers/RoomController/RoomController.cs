using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerService.Services.ExamScheduleServices;
using Services.RoomServices;

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
        public async Task<IActionResult> GetAllRoom()
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _roomService.GetAllRooms();
            return Ok(aPIResponse);
        }

        //GET: api/Rooms/{Id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById(string id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _roomService.GetRoomById(id);
            return Ok(aPIResponse);
        }

        //POST: api/Rooms
        [HttpPost]
        public async Task<IActionResult> AddNewRoom(RoomDTO roomDTO)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _roomService.AddNewRoom(roomDTO);
            return Ok(aPIResponse);
        }

        //PUT: api/Rooms
        [HttpPut]
        public async Task<IActionResult> UpdateRoom(RoomDTO room)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _roomService.UpdateRoom(room);
            return Ok(aPIResponse);
        }
        //PUT: api/Rooms
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(string id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _roomService.DeleteRoom(id);
            return Ok(aPIResponse);
        }

        [HttpGet("ChangeStatus/{id}/{status}")]
        public async Task<IActionResult> ChangeStatusRoomDisable(string id, int status)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _roomService.ChangeRoomStatus(id,status);
            return Ok(aPIResponse);
        }

        [HttpGet("AvailableRooms/{date}/{slotId}")]
        public async Task<IActionResult> GetAvailableRooms(DateOnly date,int slotId)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _roomService.GetAvailableRooms(date, slotId);
            return Ok(aPIResponse);
            }


        [HttpGet("AvailableRoomsToAddExamSchedule/{date}/{startTime}/{endTime}")]
        public async Task<IActionResult> AvailableRooms(DateOnly date, TimeOnly startTime, TimeOnly endTime)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _roomService.GetAvailableRooms(date, startTime, endTime);
            return Ok(aPIResponse);
            }
        [HttpPut("ChangeSelectStatus")]
        public async Task<IActionResult> ChangeStatusRoomSelected(List<string> ids, int status)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _roomService.ChangeRoomStatusSelected(ids, status);
            return Ok(aPIResponse);
            }
        }
}
