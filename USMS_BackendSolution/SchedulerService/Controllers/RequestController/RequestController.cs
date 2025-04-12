using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;
using SchedulerService.Services.RequestScheduleServices.cs;
using SchedulerService.Services.SlotServices;
using Services.RoomServices;

namespace SchedulerService.Controllers.RequestController
    {
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
        {
        private readonly RequestScheduleService _service;
        public RequestController(RequestScheduleService service)
            {
            _service=service;
            }
        [HttpGet]
        public async Task<IActionResult> GetAllRequest()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetAllRequest();
            return Ok(aPIResponse);
            }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRequestById(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetRequestById(id);
            return Ok(aPIResponse);
            }

        [HttpPost]
        public async Task<IActionResult> CreateRequest(RequestSchedule model)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.CreateRequest(model);
            return Ok(aPIResponse);
            }

        [HttpGet("ChangeStatus/{id}/{status}")]
        public async Task<IActionResult> ChangeRequestStatus(int id, int status)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.ChangeRequestStatus(id, status);
            return Ok(aPIResponse);
            }
        [HttpPut]
        public async Task<IActionResult> UpdateRequest(RequestSchedule request)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.UpdateRequest(request);
            return Ok(aPIResponse);
            }
        }
    }
