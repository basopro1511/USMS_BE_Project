using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerService.Services.ExamScheduleServices;
using Services.RoomServices;

namespace SchedulerService.Controllers.ExamScheduleController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamScheduleController : ControllerBase
    {
        private readonly ExamScheduleService _examScheduleService;
        public ExamScheduleController(ExamScheduleService examScheduleService)
        {
            _examScheduleService = examScheduleService;
        }
        //POST: api/Rooms
        [HttpPost]
        public APIResponse AddNewExamSchedule(ExamScheduleDTO examScheduleDTO)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = _examScheduleService.AddNewExamSchedule(examScheduleDTO);
            return aPIResponse;
        }
    }
}
