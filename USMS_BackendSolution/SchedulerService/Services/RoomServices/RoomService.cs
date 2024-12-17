using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using Repositories.RoomRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.RoomServices
{
    public class RoomService
    {
        private readonly IRoomRepository _roomRepository;
        public RoomService()
        {
            _roomRepository = new RoomRepository();
        }

        #region Get All Room
        /// <summary>
        /// Retrive all Rooms in Database
        /// </summary>
        /// <returns>a list of all Rooms in DB</returns>
        public APIResponse GetAllRooms()
        {
            APIResponse aPIResponse = new APIResponse();
            List<RoomDTO> rooms = _roomRepository.GetAllRooms();
            if (rooms == null)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Don't have any Room available!";
            }
            aPIResponse.Result = rooms;
            return aPIResponse;
        }
        #endregion

        #region Get Room By RoomId 
        /// <summary>
        /// Retrive a Room with RoomId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a Room by Id</returns>
        public APIResponse GetRoomById(string id)
        {
            APIResponse aPIResponse = new APIResponse();
            RoomDTO room = _roomRepository.GetRoomById(id);
            if (room == null)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Room with RoomId: " + id + " is not found";
            }
            aPIResponse.Result = room;
            return aPIResponse;
        }
        #endregion

        #region Add New Room
        /// <summary>
        /// Add New Room to databse
        /// </summary>
        /// <param name="room"></param>
        public APIResponse AddNewRoom(RoomDTO room)
        {
            APIResponse aPIResponse = new APIResponse();
            RoomDTO existingRoom = _roomRepository.GetRoomById(room.RoomId);
            if (existingRoom != null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Mã phòng học được cung cấp đã tồn tại!"
                };
            }
            if (room.isOnline == false) { // Check If isOnlnie is false the Online URL will be null 
                room.OnlineURL = null;
            }
            if (room.RoomId.Length > 6 )
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Mã phòng học không thể dài hơn 6 ký tự"
                };
            }
            bool isAdded = _roomRepository.AddNewRoom(room);
            if (isAdded)
            {
                return new APIResponse
                {
                    IsSuccess = true,
                    Message = "Thêm phòng học thành công!"
                };
            }
            return new APIResponse
            {
                IsSuccess = false,
                Message = "Thêm phòng học thất bại !"
            };
        }
        #endregion

        #region Update Room
        /// <summary>
        /// Udate Room in databse
        /// </summary>
        /// <param name="room"></param>
        public APIResponse UpdateRoom(RoomDTO room)
        {
            APIResponse aPIResponse = new APIResponse();
            RoomDTO existingRoom = _roomRepository.GetRoomById(room.RoomId);
            if (existingRoom == null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Mã phòng học được cung cấp đã tồn tại!"
                };
            }
            if (room.isOnline == false)
            { // Check If isOnlnie is false the Online URL will be null 
                room.OnlineURL = null;
            }
            bool isUpdate = _roomRepository.UpdateRoom(room);
            if (isUpdate)
            {
                return new APIResponse
                {
                    IsSuccess = true,
                    Message = "Cập nhật phòng học thành công!"
                };
            }
            return new APIResponse
            {
                IsSuccess = false,
                Message = "Cập nhật phòng học thất bại!"
            };
        }
        #endregion

        #region Delete Room
        /// <summary>
        /// Delete Room from database
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns>APIResponse</returns>
        public APIResponse DeleteRoom(string roomId)
        {
            APIResponse aPIResponse = new APIResponse();
            RoomDTO existingRoom = _roomRepository.GetRoomById(roomId);
            if (existingRoom == null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Mã phòng học được cung cấp đã tồn tại!"
                };
            }
            bool isDeleted = _roomRepository.DeleteRoom(roomId);
            if (isDeleted)
            {
                return new APIResponse
                {
                    IsSuccess = true,
                    Message = "Xóa phòng học thành công !"
                };
            }
            return new APIResponse
            {
                IsSuccess = false,
                Message = "Xóa phòng học thất bại !"
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
        public APIResponse ChangeRoomStatus(string id, int newStatus)
        {
            APIResponse aPIResponse = new APIResponse();
            RoomDTO existingRoom = _roomRepository.GetRoomById(id);
            if (existingRoom == null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Mã phòng học được cung cấp không tồn tại."
                };
            }
            bool isSuccess = _roomRepository.ChangeRoomStatus(id, newStatus);
            if (isSuccess)
            {
                if (newStatus == 0)
                {
                    return new APIResponse
                    {
                        IsSuccess = true,
                        Message = $"Phòng học với mã: {id} đã được vô hiệu hóa."
                    };
                }
                else if (newStatus == 1)
                {
                    return new APIResponse
                    {
                        IsSuccess = true,
                        Message = $"Phòng học với mã: {id} đang khả dụng."
                    };
                }
                else if (newStatus == 2)
                {
                    return new APIResponse
                    {
                        IsSuccess = true,
                        Message = $"Phòng học với mã: {id} đang được bảo trì."
                    };
                }
            }
            // Trường hợp thay đổi trạng thái không thành công
            return new APIResponse
            {
                IsSuccess = false,
                Message = "Thay đổi trạng thái phòng thất bại."
            };
        }
        #endregion

    }
}
