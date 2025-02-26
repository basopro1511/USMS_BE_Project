using ISUZU_NEXT.Server.Core.Extentions;
using SchedulerBusinessObject;
using SchedulerBusinessObject.Enums;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;
using SchedulerService.Repository.RequestRepository;

namespace SchedulerService.Services.RequestServices
{
	public class RequestService
	{
		private readonly IRequestRepository _requestRepository;
		public RequestService(IRequestRepository requestRepository)
		{
			_requestRepository = requestRepository;
		}
		#region Get All Requests
		public async Task<APIResponse> GetAllRequests()
		{
			APIResponse response = new APIResponse();
			var requests = await _requestRepository.GetAllRequest();
			// Không có trường nào là false
			response.Result = requests;
			return response;
		}
		#endregion
		#region Get Request By Id
		public async Task<APIResponse> GetRequestbyId(int requestId)
		{
			APIResponse response = new APIResponse();
			var requests = await _requestRepository.GetRequestById(requestId);
			if (requests == null)
			{
				response.IsSuccess = false;
				response.Message = "Không tìm thấy yêu cầu này!";
			}
			else
			{
				response.Result = requests;
			}
			return response;
		}
		#endregion
		#region Add Request
		public async Task<APIResponse> AddRequest(CreateRequestDTO request)
		{
			APIResponse response = new APIResponse();
			if (request == null)
			{
				response.IsSuccess = false;
				response.Message = "Dữ liệu không hợp lệ!";
				return response;
			}
			var create = new RequestSchedule();
			create.CopyProperties(request);
			create.Status = (int)RequestStatus.Pending;
			create.CreatedAt = DateTime.Now;
			create.UpdatedAt = DateTime.Now;
			await _requestRepository.AddRequest(create);
			response.Message = "Thêm yêu cầu thành công!";
			return response;
		}
		#endregion
		#region Change Request Status
		public async Task<APIResponse> ChangeStatusRequest(UpdateStatusRequestDTO updateStatusRequest)
		{
			APIResponse aPIResponse = new APIResponse();
			var request = await _requestRepository.GetRequestById(updateStatusRequest.RequestId);
			if (request == null)
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "Không tìm thấy yêu cầu này!";
				return aPIResponse;
			}
			if (request.Status == updateStatusRequest.Status)
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "Trạng thái yêu cầu không thay đổi!";
				return aPIResponse;
			}
			request.Status = updateStatusRequest.Status;
			request.UpdatedAt = DateTime.Now;
			await _requestRepository.ChangeRequestStatus(request);
			return aPIResponse;
		}
		#endregion
	}
}
