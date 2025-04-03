using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using Repositories.RoomRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.ScheduleRepository;
using SchedulerBusinessObject.SchedulerModels;
using ISUZU_NEXT.Server.Core.Extentions;

namespace Services.RoomServices
    {
    public class RoomService
        {
        private readonly IRoomRepository _roomRepository;
        private readonly IScheduleRepository _scheduleRepository;
        public RoomService()
            {
            _roomRepository=new RoomRepository();
            _scheduleRepository=new ScheduleRepository();
            }

        #region Get All Room
        /// <summary>
        /// Retrive all Rooms in Database
        /// </summary>
        /// <returns>a list of all Rooms in DB</returns>
        public async Task<APIResponse> GetAllRooms()
            {
            APIResponse aPIResponse = new APIResponse();
            List<Room> rooms = await _roomRepository.GetAllRooms();
            if (rooms==null)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Don't have any Room available!";
                }
            else
                {
                List<RoomDTO> roomDTOs = new List<RoomDTO>();
                foreach (var room in rooms)
                    {
                    RoomDTO dto = new RoomDTO();
                    dto.CopyProperties(room);
                    roomDTOs.Add(dto);
                    }
                aPIResponse.Result=roomDTOs;
                }
            return aPIResponse;
            }
        #endregion

        #region Get Room By RoomId 
        /// <summary>
        /// Retrive a Room with RoomId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a Room by Id</returns>
        public async Task<APIResponse> GetRoomById(string id)
            {
            APIResponse aPIResponse = new APIResponse();
            Room room = await _roomRepository.GetRoomById(id);
            if (room==null)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Phòng học với mã: "+id+" không tìm thấy";
                }
            else
                {
                RoomDTO dto = new RoomDTO();
                dto.CopyProperties(room);
                aPIResponse.Result=dto;
                }
            return aPIResponse;
            }
        #endregion

        #region Add New Room
        /// <summary>
        /// Add New Room to databse
        /// </summary>
        /// <param name="room"></param>
        public async Task<APIResponse> AddNewRoom(RoomDTO room)
            {
            APIResponse aPIResponse = new APIResponse();
            Room existingRoom = await _roomRepository.GetRoomById(room.RoomId);
            if (existingRoom!=null)
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Mã phòng học được cung cấp đã tồn tại!"
                    };
                }
            if (room.isOnline==false)
                {
                room.OnlineURL=null;
                }
            if (room.RoomId.Length>6)
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Mã phòng học không thể dài hơn 6 ký tự"
                    };
                }
            Room model = new Room();
            model.CopyProperties(room);
            bool isAdded = await _roomRepository.AddNewRoom(model);
            if (isAdded)
                {
                return new APIResponse
                    {
                    IsSuccess=true,
                    Message="Thêm phòng học thành công!"
                    };
                }
            return new APIResponse
                {
                IsSuccess=false,
                Message="Thêm phòng học thất bại !"
                };
            }
        #endregion

        #region Update Room
        /// <summary>
        /// Udate Room in databse
        /// </summary>
        /// <param name="room"></param>
        public async Task<APIResponse> UpdateRoom(RoomDTO room)
            {
            APIResponse aPIResponse = new APIResponse();
            Room existingRoom = await _roomRepository.GetRoomById(room.RoomId);
            if (existingRoom==null)
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Mã phòng học được cung cấp không tồn tại!"
                    };
                }
            if (room.isOnline==false)
                {
                room.OnlineURL=null;
                }
            Room model = new Room();
            model.CopyProperties(room);
            bool isUpdate = await _roomRepository.UpdateRoom(model);
            if (isUpdate)
                {
                return new APIResponse
                    {
                    IsSuccess=true,
                    Message="Cập nhật phòng học thành công!"
                    };
                }
            return new APIResponse
                {
                IsSuccess=false,
                Message="Cập nhật phòng học thất bại!"
                };
            }
        #endregion

        #region Delete Room
        /// <summary>
        /// Delete Room from database
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns>APIResponse</returns>
        public async Task<APIResponse> DeleteRoom(string roomId)
            {
            APIResponse aPIResponse = new APIResponse();
            Room existingRoom = await _roomRepository.GetRoomById(roomId);
            if (existingRoom==null)
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Mã phòng học được cung cấp không tồn tại!"
                    };
                }
            bool isDeleted = await _roomRepository.DeleteRoom(roomId);
            if (isDeleted)
                {
                return new APIResponse
                    {
                    IsSuccess=true,
                    Message="Xóa phòng học thành công !"
                    };
                }
            return new APIResponse
                {
                IsSuccess=false,
                Message="Xóa phòng học thất bại !"
                };
            }
        #endregion

        #region Change Room Status
        /// <summary>
        /// Change the status of a room
        /// </summary>
        /// <param name="id">Room ID</param>
        /// <param name="newStatus">New status to set ( 0 = Disable, 1 = Available, 2 = Maintenance)</param>
        /// <returns>APIResponse</returns>
        public async Task<APIResponse> ChangeRoomStatus(string id, int newStatus)
            {
            APIResponse aPIResponse = new APIResponse();
            Room existingRoom = await _roomRepository.GetRoomById(id);
            if (existingRoom==null)
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Mã phòng học được cung cấp không tồn tại."
                    };
                }
            bool isSuccess = await _roomRepository.ChangeRoomStatus(id, newStatus);
            if (isSuccess)
                {
                if (newStatus==0)
                    {
                    return new APIResponse
                        {
                        IsSuccess=true,
                        Message=$"Phòng học với mã: {id} đã được vô hiệu hóa."
                        };
                    }
                else if (newStatus==1)
                    {
                    return new APIResponse
                        {
                        IsSuccess=true,
                        Message=$"Phòng học với mã: {id} đang khả dụng."
                        };
                    }
                else if (newStatus==2)
                    {
                    return new APIResponse
                        {
                        IsSuccess=true,
                        Message=$"Phòng học với mã: {id} đang được bảo trì."
                        };
                    }
                }
            return new APIResponse
                {
                IsSuccess=false,
                Message="Thay đổi trạng thái phòng thất bại."
                };
            }
        #endregion

        #region Get Available Room for Add Schedule
        /// <summary>
        /// Lấy danh sách phòng trống trong ngày date, slotId
        /// </summary>
        /// <param name="date">Ngày</param>
        /// <param name="slotId">Slot (tiết)</param>
        /// <returns>APIResponse với Result = danh sách phòng (List<Room>)</returns>
        public async Task<APIResponse> GetAvailableRooms(DateOnly date, int slotId)
            {
            APIResponse response = new APIResponse();
            try
                {
                var allRooms = await _roomRepository.GetAllRooms();
                if (allRooms==null||allRooms.Count==0)
                    {
                    response.IsSuccess=false;
                    response.Message="Không có phòng nào trong hệ thống!";
                    return response;
                    }
                // 2. Lấy các schedule có date & slotId = ...
                var schedules = _scheduleRepository.GetSchedulesByDateAndSlot(date, slotId);
                // 3. Lấy danh sách roomId đã bị chiếm
                var usedRoomIds = schedules.Select(sch => sch.RoomId).Distinct().ToHashSet();
                // 4. Lọc ra các phòng còn trống
                var availableRooms = allRooms
                    .Where(r => !usedRoomIds.Contains(r.RoomId))
                    .ToList();
                // 5. Gói vào APIResponse
                response.IsSuccess=true;
                response.Message="Lấy danh sách phòng trống thành công.";
                response.Result=availableRooms; // hoặc map sang DTO RoomDTO
                }
            catch (Exception ex)
                {
                response.IsSuccess=false;
                response.Message="Lỗi: "+ex.Message;
                }
            return response;
            }
        #endregion

        #region Get Available Rooms to Add Exam Schedule
        /// <summary>
        /// Retrive all Rooms in Database
        /// </summary>
        /// <returns>a list of all Rooms in DB</returns>
        public async Task<APIResponse> GetAvailableRooms(DateOnly date, TimeOnly startTime, TimeOnly endTime)
            {
            APIResponse aPIResponse = new APIResponse();
            var availableRooms = await _roomRepository.GetAvailableRooms(date, startTime, endTime);
            if (availableRooms==null||availableRooms.Count==0)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không có phòng trống nào khả dụng trong thời gian đã chọn !";
                }
            else
                {
                aPIResponse.Result=availableRooms;
                }
            return aPIResponse;
            }
        #endregion

        #region Change Exam SCHEDULE Status Selected 
        /// <summary>
        /// Change Exam SCHEDULE status
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<APIResponse> ChangeRoomStatusSelected(List<string> Ids, int status)
            {
            APIResponse aPIResponse = new APIResponse();
            if (Ids==null||!Ids.Any())
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Danh sách phòng học không hợp lệ.";
                return aPIResponse;
                }
            bool isSuccess = await _roomRepository.ChangeRoomStatusSelected(Ids, status);
            if (isSuccess)
                {
                aPIResponse.IsSuccess=true;
                switch (status)
                    {
                    case 0:
                        aPIResponse.Message="Đã thay đổi trạng thái các phòng học đã chọn thành 'Vô hiệu hóa'.";
                        break;
                    case 1:
                        aPIResponse.Message="Đã thay đổi trạng thái các phòng học đã chọn thành 'Đang khả dụng'.";
                        break;
                    case 2:
                        aPIResponse.Message="Đã thay đổi trạng thái các phòng học đã chọn thành 'Đang bảo trì'.";
                        break;
                    default:
                        aPIResponse.Message="Trạng thái không hợp lệ.";
                        break;
                    }
                }
            return aPIResponse;
            }
        #endregion
        }
    }
