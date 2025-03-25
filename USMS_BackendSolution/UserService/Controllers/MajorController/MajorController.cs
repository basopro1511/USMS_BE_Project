using BusinessObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Services.MajorServices;

namespace UserService.Controllers.MajorController
{
    [Route("api/[controller]")]
    [ApiController]
    public class MajorController : ControllerBase
    {
        private readonly MajorService _service;
        public MajorController(MajorService Service)
        {
            _service = Service;
        }

        // GET: api/Rooms
        [HttpGet]
        public async Task<APIResponse> GetAllRoom()
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = await _service.GetAllMajor();
            return aPIResponse;
        }
    }
}
