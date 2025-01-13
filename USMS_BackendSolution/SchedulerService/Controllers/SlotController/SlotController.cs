using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerService.Services.SlotServices;
using Services.RoomServices;

namespace SchedulerService.Controllers.SlotController
{
    [Route("api/[controller]")]
    [ApiController]
    public class SlotController : ControllerBase
    {
        private readonly SlotService _slotService;
        public SlotController(SlotService slotService)
        {
            _slotService = slotService;
        }

        // GET: api/Rooms
        [HttpGet]
        public APIResponse GetAllSlots()
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _slotService.GetAllSlots();
            return aPIResponse;
        }

        //GET: api/Rooms/{Id}
        [HttpGet("{id}")]
        public APIResponse GeSlotById(int id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _slotService.GetSlotById(id);
            return aPIResponse;
        }

        //POST: api/Rooms
        [HttpPost]
        public APIResponse AddNewSlot(TimeSlotDTO slotDTO)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _slotService.AddNewSlot(slotDTO);
            return aPIResponse;
        }

        //PUT: api/Rooms
        [HttpPut]
        public APIResponse UpdateSlot(TimeSlotDTO slotDTO)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _slotService.UpdateTimeSlot(slotDTO);
            return aPIResponse;
        }

        [HttpGet("ChangeStatus/{id}/{status}")]
        public APIResponse ChangeSlotStatus(int id, int status)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _slotService.ChangeSlotStatus(id, status);
            return aPIResponse;
        }
    }
}
