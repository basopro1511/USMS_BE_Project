using Microsoft.AspNetCore.Mvc;
using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerService.Services.RequestServices;

namespace SchedulerService.Controllers.RequestController
{
	[Route("api/[controller]")]
	[ApiController]
	public class RequestController : ControllerBase
	{
		private readonly RequestService _requestService;
		public RequestController(RequestService requestService)
		{
			_requestService = requestService;
		}
		[HttpGet]
		public async Task<APIResponse> GetAllRequest()
		{
			APIResponse response = new APIResponse();
			response = await _requestService.GetAllRequests();
			return response;
		}
		[HttpGet("{id}")]
		public async Task<APIResponse> GetRequestById(int id)
		{
			APIResponse response = new APIResponse();
			response = await _requestService.GetRequestbyId(id);
			return response;
		}
		[HttpPost]
		public async Task<APIResponse> AddRequest(CreateRequestDTO request)
		{
			APIResponse response = new APIResponse();
			if (!ModelState.IsValid)
			{
				response.IsSuccess = false;
				response.Message = "Dữ liệu không hợp lệ!";
			}
			response = await _requestService.AddRequest(request);
			return response;
		}
		[HttpPut]
		public async Task<APIResponse> ChangeRequestStatus(UpdateStatusRequestDTO UpdateStatusRequest)
		{
			APIResponse response = new APIResponse();
			if (ModelState.IsValid)
			{
				response.IsSuccess = false;
				response.Message = "Dữ liệu không hợp lệ!";
			}
			response = await _requestService.ChangeStatusRequest(UpdateStatusRequest);
			return response;
		}
	}
}
