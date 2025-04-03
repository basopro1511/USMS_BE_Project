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
        public async Task<IActionResult> GetAllSlots()
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _slotService.GetAllSlots();
            return Ok(aPIResponse);
        }

        //GET: api/Rooms/{Id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GeSlotById(int id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _slotService.GetSlotById(id);
            return Ok(aPIResponse);
        }

        //POST: api/Rooms
        [HttpPost]
        public async Task<IActionResult> AddNewSlot(TimeSlotDTO slotDTO)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _slotService.AddNewSlot(slotDTO);
            return Ok(aPIResponse);
        }

        //PUT: api/Rooms
        [HttpPut]
        public async Task<IActionResult> UpdateSlot(TimeSlotDTO slotDTO)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _slotService.UpdateTimeSlot(slotDTO);
            return Ok(aPIResponse);
        }

        [HttpGet("ChangeStatus/{id}/{status}")]
        public async Task<IActionResult> ChangeSlotStatus(int id, int status)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _slotService.ChangeSlotStatus(id, status);
            return Ok(aPIResponse);
        }
    }
}
