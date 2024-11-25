using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerDataAccess.Repositories.RoomRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerDataAccess.Services.RoomServices
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
        public APIResponse AddNewRoom(AddUpdateRoomDTO room)
        {
            APIResponse aPIResponse = new APIResponse();
            RoomDTO existingRoom = _roomRepository.GetExistingRoom(room.RoomId);
            if (existingRoom != null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Room with the given Room Id already exists."
                };
            }
            bool isAdded = _roomRepository.AddNewRoom(room);
            if (isAdded)
            {
                return new APIResponse
                {
                    IsSuccess = true,
                    Message = "Room added successfully."
                };
            }
            return new APIResponse
            {
                IsSuccess = false,
                Message = "Failed to add Room."
            };
        }
        #endregion

        #region Update Room
        /// <summary>
        /// Udate Room in databse
        /// </summary>
        /// <param name="room"></param>
        public APIResponse UpdateRoom(AddUpdateRoomDTO room)
        {
            APIResponse aPIResponse = new APIResponse();
            RoomDTO existingRoom = _roomRepository.GetRoomById(room.RoomId);
            if (existingRoom == null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Room with the given ID is not exists."
                };
            }
            bool isAdded = _roomRepository.UpdateRoom(room);
            if (isAdded)
            {
                return new APIResponse
                {
                    IsSuccess = true,
                    Message = "Room updated successfully."
                };
            }
            return new APIResponse
            {
                IsSuccess = false,
                Message = "Failed to updated Room."
            };
        }
        #endregion

        #region Change Room Status
        /// <summary>
        /// Change Status of Room
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public APIResponse ChangeStatusRoom(string id)
        {
            var response = new APIResponse();
            RoomDTO existingRoom = _roomRepository.GetRoomById(id);
            if (existingRoom == null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Room with the given ID is not exists."
                };
            }
            bool isSuccess = _roomRepository.ChangeStatusRoom(id);
            if (isSuccess)
            {
                return new APIResponse
                {
                    IsSuccess = true,
                    Message = "Room change status successfully."
                };
            }
            return new APIResponse
            {
                IsSuccess = false,
                Message = "Failed to change status of Room."
            };
        }
        #endregion
    }
}