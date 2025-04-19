using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using Services.RoomServices;

namespace SchedulerService.Controllers.RoomController
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly RoomService _roomService;
        public RoomController(RoomService roomService)
        {
            _roomService = roomService;
        }

        // GET: api/Rooms
        [HttpGet]
        public async Task<IActionResult> GetAllRoom()
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _roomService.GetAllRooms();
            return Ok(aPIResponse);
        }

        //GET: api/Rooms/{Id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoomById(string id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _roomService.GetRoomById(id);
            return Ok(aPIResponse);
        }

        //POST: api/Rooms
        [HttpPost]
        public async Task<IActionResult> AddNewRoom(RoomDTO roomDTO)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _roomService.AddNewRoom(roomDTO);
            return Ok(aPIResponse);
        }

        //PUT: api/Rooms
        [HttpPut]
        public async Task<IActionResult> UpdateRoom(RoomDTO room)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _roomService.UpdateRoom(room);
            return Ok(aPIResponse);
        }
        //PUT: api/Rooms
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(string id)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _roomService.DeleteRoom(id);
            return Ok(aPIResponse);
        }

        [HttpGet("ChangeStatus/{id}/{status}")]
        public async Task<IActionResult> ChangeStatusRoomDisable(string id, int status)
        {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _roomService.ChangeRoomStatus(id,status);
            return Ok(aPIResponse);
        }

        [HttpGet("AvailableRooms/{date}/{slotId}")]
        public async Task<IActionResult> GetAvailableRooms(DateOnly date,int slotId)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _roomService.GetAvailableRooms(date, slotId);
            return Ok(aPIResponse);
            }

        [HttpPut("ChangeSelectStatus")]
        public async Task<IActionResult> ChangeStatusRoomSelected(List<string> ids, int status)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _roomService.ChangeRoomStatusSelected(ids, status);
            return Ok(aPIResponse);
            }
        #region

        [HttpGet("export")]
        public async Task<IActionResult> ExportRoomsToExcel(int? status)
            {
            APIResponse aPIResponse = new APIResponse();
            var export = await _roomService.ExportRoomsToExcel(status);
            if (export==null)
                {
                aPIResponse.Message="Không có dữ liệu để xuất.";
                return BadRequest(aPIResponse);
                }
            aPIResponse.Result="File đã được tạo và sẵn sàng để tải về.";
            aPIResponse.Message="Export Thành công";
            // Trả về tệp Excel trực tiếp
            var fileBytes = export as byte[];
            if (fileBytes==null)
                {
                aPIResponse.Message="Đã xảy ra lỗi khi tạo tệp Excel.";
                return StatusCode(500, aPIResponse);
                }
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachPhongHoc.xlsx");
            }
        #endregion

        #region
        /// <summary>
        /// Export Empty form to add room
        /// </summary>
        /// <returns></returns>
        [HttpGet("exportEmpty")]
        public async Task<IActionResult> ExportFormAddRoom()
            {
            APIResponse aPIResponse = new APIResponse();
            var export = await _roomService.ExportFormAddRoom();
            if (export==null)
                {
                aPIResponse.Message="Không có dữ liệu để xuất.";
                return BadRequest(aPIResponse);
                }
            aPIResponse.Result="File đã được tạo và sẵn sàng để tải về.";
            aPIResponse.Message="Export Thành công";
            // Trả về tệp Excel trực tiếp
            var fileBytes = export as byte[];
            if (fileBytes==null)
                {
                aPIResponse.Message="Đã xảy ra lỗi khi tạo tệp Excel.";
                return StatusCode(500, aPIResponse);
                }
            return File(fileBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "DanhSachPhongHoc.xlsx");
            }
        #endregion

        #region Import From Excel
        [HttpPost("import")]
        public async Task<IActionResult> ImportRooms(IFormFile file)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _roomService.ImportRoomsFromExcel(file);
            return Ok(aPIResponse);
            }
        #endregion
        }
    }
