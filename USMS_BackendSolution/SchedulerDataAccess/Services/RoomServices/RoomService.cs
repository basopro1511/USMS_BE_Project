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

        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        // 1. Xem danh sách tất cả phòng học
        public async Task<IEnumerable<RoomDTO>> GetAllRoomsAsync()
        {
            return await _roomRepository.GetAllRoomsAsync();
        }

        // 2. Xem chi tiết phòng học theo RoomId
        public async Task<RoomDTO> GetRoomByIdAsync(string roomId)
        {
            return await _roomRepository.GetRoomByIdAsync(roomId);
        }

        // 3. Thêm phòng học mới
        public async Task AddRoomAsync(RoomDTO roomDTO)
        {
            // Kiểm tra logic nghiệp vụ trước khi thêm
            if (string.IsNullOrWhiteSpace(roomDTO.RoomId))
            {
                throw new ArgumentException("RoomId cannot be null or empty.");
            }

            if (roomDTO.Status < 0 || roomDTO.Status > 1) // Giả sử trạng thái chỉ là 0 (disabled) hoặc 1 (enabled)
            {
                throw new ArgumentException("Invalid Status value.");
            }

            await _roomRepository.AddRoomAsync(roomDTO);
        }

        // 4. Sửa thông tin phòng học
        public async Task UpdateRoomAsync(RoomDTO roomDTO)
        {
            // Kiểm tra logic nghiệp vụ trước khi cập nhật
            var existingRoom = await _roomRepository.GetRoomByIdAsync(roomDTO.RoomId);
            if (existingRoom == null)
            {
                throw new KeyNotFoundException("Room not found.");
            }

            await _roomRepository.UpdateRoomAsync(roomDTO);
        }

        // 5. Vô hiệu hóa phòng học (thay đổi trạng thái)
        public async Task DisableRoomAsync(string roomId)
        {
            // Kiểm tra logic nghiệp vụ trước khi vô hiệu hóa
            var existingRoom = await _roomRepository.GetRoomByIdAsync(roomId);
            if (existingRoom == null)
            {
                throw new KeyNotFoundException("Room not found.");
            }

            await _roomRepository.DisableRoomAsync(roomId);
        }
    }

}
