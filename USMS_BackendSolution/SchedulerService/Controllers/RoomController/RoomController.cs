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
        public async Task<APIResponse> GetAllRoom()
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _roomService.GetAllRooms();
            return aPIResponse;
        }

        //GET: api/Rooms/{Id}
        [HttpGet("{id}")]
        public async Task<APIResponse> GetRoomById(string id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _roomService.GetRoomById(id);
            return aPIResponse;
        }

        //POST: api/Rooms
        [HttpPost]
        public async Task<APIResponse> AddNewRoom(RoomDTO roomDTO)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _roomService.AddNewRoom(roomDTO);
            return aPIResponse;
        }

        //PUT: api/Rooms
        [HttpPut]
        public async Task<APIResponse> UpdateRoom(RoomDTO room)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _roomService.UpdateRoom(room);
            return aPIResponse;
        }
        //PUT: api/Rooms
        [HttpDelete("{id}")]
        public async Task<APIResponse> DeleteRoom(string id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _roomService.DeleteRoom(id);
            return aPIResponse;
        }

        [HttpGet("ChangeStatus/{id}/{status}")]
        public async Task<APIResponse> ChangeStatusRoomDisable(string id, int status)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _roomService.ChangeRoomStatus(id,status);
            return aPIResponse;
        }

        [HttpGet("AvailableRooms/{date}/{slotId}")]
        public async Task<APIResponse> GetAvailableRooms(DateOnly date,int slotId)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _roomService.GetAvailableRooms(date, slotId);
            return aPIResponse;
            }


        [HttpGet("AvailableRoomsToAddExamSchedule/{date}/{startTime}/{endTime}")]
        public async Task<APIResponse> AvailableRooms(DateOnly date, TimeOnly startTime, TimeOnly endTime)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _roomService.GetAvailableRooms(date, startTime, endTime);
            return aPIResponse;
            }
        }


}
