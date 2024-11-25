using Microsoft.EntityFrameworkCore;
using SchedulerBusinessObject.AppDBContext;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerDataAccess.Repositories.RoomRepository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly MyDbContext _context;

        public RoomRepository(MyDbContext context)
        {
            _context = context;
        }

        // 1. Xem danh sách tất cả phòng học
        public async Task<IEnumerable<RoomDTO>> GetAllRoomsAsync()
        {
            return await _context.Rooms
                .Select(room => new RoomDTO
                {
                    RoomId = room.RoomId,
                    Location = room.Location,
                    IsOnline = room.isOnline,
                    OnlineURL = room.OnlineURL,
                    Status = room.Status,
                    CreateAt = room.CreateAt,
                    UpdateAt = room.UpdateAt
                })
                .ToListAsync();
        }

        // 2. Xem chi tiết phòng học theo RoomId
        public async Task<RoomDTO> GetRoomByIdAsync(string roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);

            if (room == null) return null;

            return new RoomDTO
            {
                RoomId = room.RoomId,
                Location = room.Location,
                IsOnline = room.isOnline,
                OnlineURL = room.OnlineURL,
                Status = room.Status,
                CreateAt = room.CreateAt,
                UpdateAt = room.UpdateAt
            };
        }

        // 3. Thêm phòng học mới
        public async Task AddRoomAsync(RoomDTO roomDTO)
        {
            var room = new Rooms
            {
                RoomId = roomDTO.RoomId,
                Location = roomDTO.Location,
                isOnline = roomDTO.IsOnline,
                OnlineURL = roomDTO.OnlineURL,
                Status = roomDTO.Status,
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now
            };

            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
        }

        // 4. Sửa thông tin phòng học
        public async Task UpdateRoomAsync(RoomDTO roomDTO)
        {
            var room = await _context.Rooms.FindAsync(roomDTO.RoomId);
            if (room == null) return;

            room.Location = roomDTO.Location;
            room.isOnline = roomDTO.IsOnline;
            room.OnlineURL = roomDTO.OnlineURL;
            room.Status = roomDTO.Status;
            room.UpdateAt = DateTime.Now;

            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }

        // 5. Vô hiệu hóa phòng học (thay đổi trạng thái)
        public async Task DisableRoomAsync(string roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return;

            room.Status = 0;  // Giả định trạng thái 0 là "vô hiệu hóa"
            room.UpdateAt = DateTime.Now;

            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }
    }

}
