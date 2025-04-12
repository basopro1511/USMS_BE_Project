
using Azure.Core;
using Repositories.RequestScheduleRepository;
using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;

namespace SchedulerService.Services.RequestScheduleServices.cs
    {
    public class RequestScheduleService
        {
        private readonly IRequestRepository _requestRepository;

        public RequestScheduleService()
            {
            _requestRepository=new RequestRepository();
            }

        #region Get All Request
        public async Task<APIResponse> GetAllRequest()
            {
            APIResponse aPIResponse = new APIResponse();
            var requests = await _requestRepository.GetRequestSchedules();
            if (requests==null)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không tìm thấy yêu cầu!";
                }
            else
                {
                aPIResponse.IsSuccess=true;
                aPIResponse.Result=requests;
                }
            return aPIResponse;
            }
        #endregion

        #region Get Request By Id
        public async Task<APIResponse> GetRequestById(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            var request = await _requestRepository.GetRequestScheduleById(id);
            if (request==null)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không tìm thấy yêu cầu!";
                }
            else
                {
                aPIResponse.IsSuccess=true;
                aPIResponse.Result=request;
                }
            return aPIResponse;
            }
        #endregion

        #region Teacher Create Request
        public async Task<APIResponse> CreateRequest(RequestSchedule model)
            {
            APIResponse aPIResponse = new APIResponse();
            #region validation 
            List<(bool condition, string errorMessage)>? validations = new List<(bool condition, string errorMessage)>
            {
                  (model.UserId== null, "Mã giáo viên không thể để trống!"),
                  (model.RequestType == null , "Mã giáo viên không thể để trống!"),

            };
            foreach (var validation in validations)
                {
                if (validation.condition)
                    {
                    return new APIResponse
                        {
                        IsSuccess=false,
                        Message=validation.errorMessage
                        };
                    }
                }
            #endregion
            bool isAdded = await _requestRepository.CreateRequest(model);
            if (isAdded)
                {
                return new APIResponse
                    {
                    IsSuccess=true,
                    Message="Gửi đơn thành công!"
                    };
                }
            return new APIResponse
                {
                IsSuccess=false,
                Message="Gửi đơn thất bại !"
                };
            }
        #endregion

        #region Change Request Status
        /// <summary>
        /// Change the Request of a room
        /// </summary>
        /// <param name="id">Room ID</param>
        /// <param name="newStatus">New status to set ( 0 = Disable, 1 = Available, 2 = Maintenance)</param>
        /// <returns>APIResponse</returns>
        public async Task<APIResponse> ChangeRequestStatus(int id, int newStatus)
            {
            APIResponse aPIResponse = new APIResponse();
            RequestSchedule existing = await _requestRepository.GetRequestScheduleById(id);
            if (existing==null)
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Mã yêu cầu được cung cấp không tồn tại."
                    };
                }
            bool isSuccess = await _requestRepository.ChangeRequestStatus(id, newStatus);
            if (isSuccess)
                {
                if (newStatus==0)
                    {
                    return new APIResponse
                        {
                        IsSuccess=true,
                        Message=$"Mã yêu cầu: {id} đã được đổi thành 'Chưa xử lý'."
                        };
                    }
                else if (newStatus==1)
                    {
                    return new APIResponse
                        {
                        IsSuccess=true,
                        Message=$"Mã yêu cầu: {id} đã được đổi thành 'Đã xử lý'."
                        };
                    }
                }
            return new APIResponse
                {
                IsSuccess=false,
                Message="Thay đổi trạng thái yêu cầu thất bại."
                };
            }
        #endregion

        #region Update Request
        /// <summary>
        /// Udate Request in databse
        /// </summary>
        /// <param name="room"></param>
        public async Task<APIResponse> UpdateRequest(RequestSchedule model)
            {
            APIResponse aPIResponse = new APIResponse();
            RequestSchedule existing = await _requestRepository.GetRequestScheduleById(model.RequestId);
            if (existing==null)
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Mã yêu cầu được cung cấp không tồn tại!"
                    };
                }
            bool isUpdate = await _requestRepository.UpdateRequest(model);
            if (isUpdate)
                {
                return new APIResponse
                    {
                    IsSuccess=true,
                    Message="Cập nhật yêu cầu thành công!"
                    };
                }
            return new APIResponse
                {
                IsSuccess=false,
                Message="Cập nhật yêu cầu thất bại!"
                };
            }
        #endregion

        }
    }
