using ISUZU_NEXT.Server.Core.Extentions;
using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;
using SchedulerService.Repository.SlotRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulerService.Services.SlotServices
    {
    public class SlotService
        {
        private readonly ISlotRepository _slotRepository;
        public SlotService()
            {
            _slotRepository=new SlotRepository();
            }

        #region Get All TimeSlot
        /// <summary>
        /// Retrive all Rooms in Database
        /// </summary>
        /// <returns>a list of all Rooms in DB</returns>
        public async Task<APIResponse> GetAllSlots()
            {
            APIResponse aPIResponse = new APIResponse();
            // Lấy model thuần từ repository
            List<TimeSlot> slotModels = await _slotRepository.GetAllTimeSlot();
            List<TimeSlotDTO> slots = new List<TimeSlotDTO>();
            foreach (var model in slotModels)
                {
                TimeSlotDTO dto = new TimeSlotDTO();
                dto.CopyProperties(model);
                slots.Add(dto);
                }
            if (slots==null||slots.Count==0)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không tìm thấy buổi học khả dụng!";
                }
            aPIResponse.Result=slots;
            return aPIResponse;
            }
        #endregion

        #region Get Time Slot By Slot Id 
        /// <summary>
        /// Retrive a TimeSlot with Slot Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a TimeSlot by Id</returns>
        public async Task<APIResponse> GetSlotById(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            TimeSlot model = await _slotRepository.GetTimeSlotById(id);
            if (model==null)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không tìm thấy buổi học với ID = "+id;
                }
            else
                {
                TimeSlotDTO dto = new TimeSlotDTO();
                dto.CopyProperties(model);
                aPIResponse.Result=dto;
                }
            return aPIResponse;
            }
        #endregion

        #region Add New TimeSlot
        /// <summary>
        /// Add New TimeSlot to databse
        /// </summary>
        /// <param name="slot">TimeSlotDTO object</param>
        public async Task<APIResponse> AddNewSlot(TimeSlotDTO slot)
            {
            APIResponse aPIResponse = new APIResponse();
            if (slot.StartTime > slot.EndTime)
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Thời gian bắt đầu không thể diễn ra sau thời gian kết thúc!"
                    };
                }
            // Kiểm tra xem Slot với SlotId đã tồn tại hay chưa
            TimeSlot existingModel = await _slotRepository.GetTimeSlotById(slot.SlotId);
            if (existingModel!=null)
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Mã buổi học được cung cấp đã tồn tại!"
                    };
                }
            // Mapping từ DTO sang model
            TimeSlot model = new TimeSlot();
            model.CopyProperties(slot);
            bool isAdded = await _slotRepository.AddNewTimeSlot(model);
            if (isAdded)
                {
                return new APIResponse
                    {
                    IsSuccess=true,
                    Message="Thêm buổi học thành công!"
                    };
                }
            return new APIResponse
                {
                IsSuccess=false,
                Message="Thêm buổi học thất bại !"
                };
            }
        #endregion

        #region Update TimeSlot
        /// <summary>
        /// Update TimeSlot in databse
        /// </summary>
        /// <param name="slot">TimeSlotDTO object</param>
        public async Task<APIResponse> UpdateTimeSlot(TimeSlotDTO slot)
            {
            if (slot.StartTime>slot.EndTime)
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Thời gian bắt đầu không thể diễn ra sau thời gian kết thúc!"
                    };
                }
            APIResponse aPIResponse = new APIResponse();
            // Kiểm tra xem Slot cần cập nhật có tồn tại không
            TimeSlot existingModel = await _slotRepository.GetTimeSlotById(slot.SlotId);
            if (existingModel==null)
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Mã buổi học được cung cấp không tồn tại!"
                    };
                }
            // Mapping từ DTO sang model
            TimeSlot model = new TimeSlot();
            model.CopyProperties(slot);
            bool isUpdated = await _slotRepository.UpdateTimeSlot(model);
            if (isUpdated)
                {
                return new APIResponse
                    {
                    IsSuccess=true,
                    Message="Cập nhật buổi học thành công!"
                    };
                }
            return new APIResponse
                {
                IsSuccess=false,
                Message="Cập nhật buổi học thất bại!"
                };
            }
        #endregion

        #region Change TimeSlot Status
        /// <summary>
        /// Change the status of a TimeSlot
        /// </summary>
        /// <param name="id">TimeSlot ID</param>
        /// <param name="newStatus">New status to set ( 0 = Disable, 1 = Available)</param>
        /// <returns>APIResponse</returns>
        public async Task<APIResponse> ChangeSlotStatus(int id, int newStatus)
            {
            APIResponse aPIResponse = new APIResponse();
            TimeSlot existingModel = await _slotRepository.GetTimeSlotById(id);
            if (existingModel==null)
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Mã buổi học được cung cấp không tồn tại!"
                    };
                }
            bool isSuccess = await _slotRepository.ChangeTimeSlotStatus(id, newStatus);
            if (isSuccess)
                {
                if (newStatus==0)
                    {
                    return new APIResponse
                        {
                        IsSuccess=true,
                        Message=$"Buổi học với mã: {id} đã được vô hiệu hóa."
                        };
                    }
                else if (newStatus==1)
                    {
                    return new APIResponse
                        {
                        IsSuccess=true,
                        Message=$"Buổi học với mã: {id} đang khả dụng."
                        };
                    }
                }
            // Trường hợp thay đổi trạng thái không thành công
            return new APIResponse
                {
                IsSuccess=false,
                Message="Thay đổi trạng thái buổi học thất bại."
                };
            }
        #endregion
        }
    }
