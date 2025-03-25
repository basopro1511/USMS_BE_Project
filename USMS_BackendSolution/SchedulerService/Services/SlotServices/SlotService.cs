using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerService.Repository.SlotRepository;

namespace SchedulerService.Services.SlotServices
{
    public class SlotService
    {
        private readonly ISlotRepository _slotRepository;
        public SlotService()
        {
            _slotRepository = new SlotRepository();
        }

        #region Get All TimeSlot
        /// <summary>
        /// Retrive all Rooms in Database
        /// </summary>
        /// <returns>a list of all Rooms in DB</returns>
        public async Task<APIResponse> GetAllSlots()
        {
            APIResponse aPIResponse = new APIResponse();
            List<TimeSlotDTO> slots =await _slotRepository.getAllTimeSlot();
            if (slots == null)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Không tìm thấy buổi học khả dụng!";
            }
            aPIResponse.Result = slots;
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
            TimeSlotDTO slot = await _slotRepository.GetTimeSlotById(id);
            if (slot == null)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Không tìm thấy buổi học với ID = " + id ;
            }
            aPIResponse.Result = slot;
            return aPIResponse;
        }
        #endregion

        #region Add New TimeSlot
        /// <summary>
        /// Add New TimeSlot to databse
        /// </summary>
        /// <param name="room"></param>
        public async Task<APIResponse> AddNewSlot(TimeSlotDTO slot)
        {
            APIResponse aPIResponse = new APIResponse();
            TimeSlotDTO existingSlot = await _slotRepository.GetTimeSlotById(slot.SlotId);
            if (existingSlot != null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Mã buổi học được cung cấp đã tồn tại!"
                };
            }
          
            bool isAdded = await _slotRepository.AddNewTimeSlot(slot);
            if (isAdded)
            {
                return new APIResponse
                {
                    IsSuccess = true,
                    Message = "Thêm buổi học thành công!"
                };
            }
            return new APIResponse
            {
                IsSuccess = false,
                Message = "Thêm buổi học thất bại !"
            };
        }
        #endregion

        #region Update TimeSlot
        /// <summary>
        /// Udate TimeSlot in databse
        /// </summary>
        /// <param name="slot"></param>
        public async Task<APIResponse> UpdateTimeSlot(TimeSlotDTO slot)
        {
            APIResponse aPIResponse = new APIResponse();
            TimeSlotDTO existingSlot = await _slotRepository.GetTimeSlotById(slot.SlotId);
            if (existingSlot == null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Mã buổi học được cung cấp không tồn tại!"
                };
            }
            bool isUpdate =await _slotRepository.UpdateTimeSlot(slot);
            if (isUpdate)
            {
                return new APIResponse
                {
                    IsSuccess = true,
                    Message = "Cập nhật buổi học thành công!"
                };
            }
            return new APIResponse
            {
                IsSuccess = false,
                Message = "Cập nhật buổi học thất bại!"
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
            TimeSlotDTO existingSlot = await _slotRepository.GetTimeSlotById(id);
            if (existingSlot == null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Mã buổi học được cung cấp không tồn tại!"
                };
            }
            bool isSuccess = await _slotRepository.ChangeTimeSlotStatus(id, newStatus);
            if (isSuccess)
            {
                if (newStatus == 0)
                {
                    return new APIResponse
                    {
                        IsSuccess = true,
                        Message = $"Buổi học với mã: {id} đã được vô hiệu hóa."
                    };
                }
                else if (newStatus == 1)
                {
                    return new APIResponse
                    {
                        IsSuccess = true,
                        Message = $"Buổi học với mã: {id} đang khả dụng."
                    };
                }
            }
            // Trường hợp thay đổi trạng thái không thành công
            return new APIResponse
            {
                IsSuccess = false,
                Message = "Thay đổi trạng thái buổi học thất bại."
            };
        }
        #endregion
    }
}
