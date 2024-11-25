using Microsoft.AspNetCore.Mvc;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerDataAccess.Services.RoomServices;

namespace SchedulerService.Controllers.Room
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;

        public RoomController(RoomService roomService)
        {
            _roomService = roomService;
        }

        // 1. GET: api/room
        // Lấy danh sách tất cả phòng học
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoomDTO>>> GetAllRooms()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            return Ok(rooms);  // Trả về danh sách phòng học dưới dạng JSON
        }

        // 2. GET: api/room/{id}
        // Lấy thông tin chi tiết phòng học theo RoomId
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDTO>> GetRoomById(string id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null)
            {
                return NotFound(new { Message = "Room not found." });
            }
            return Ok(room);  // Trả về thông tin phòng học
        }

        // 3. POST: api/room
        // Thêm mới phòng học
        [HttpPost]
        public async Task<ActionResult> AddRoom([FromBody] RoomDTO roomDTO)
        {
            try
            {
                await _roomService.AddRoomAsync(roomDTO);
                return CreatedAtAction(nameof(GetRoomById), new { id = roomDTO.RoomId }, roomDTO); // Trả về thông tin phòng học mới được tạo
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // 4. PUT: api/room/{id}
        // Cập nhật thông tin phòng học
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateRoom(string id, [FromBody] RoomDTO roomDTO)
        {
            if (id != roomDTO.RoomId)
            {
                return BadRequest(new { Message = "Room ID mismatch." });
            }

            try
            {
                await _roomService.UpdateRoomAsync(roomDTO);
                return NoContent();  // Trả về mã trạng thái 204 nếu cập nhật thành công
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Room not found." });
            }
        }

        // 5. PATCH: api/room/{id}/disable
        // Vô hiệu hóa phòng học
        [HttpPatch("{id}/disable")]
        public async Task<ActionResult> DisableRoom(string id)
        {
            try
            {
                await _roomService.DisableRoomAsync(id);
                return NoContent();  // Trả về mã trạng thái 204 nếu vô hiệu hóa thành công
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "Room not found." });
            }
        }
    }

}
