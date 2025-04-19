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
using OfficeOpenXml;

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
            room.RoomId=room.RoomId?.Trim();
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
            room.RoomId=room.RoomId?.Trim();
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

        #region Export Form Add Room
        /// <summary>
        /// Export empty form for add model
        /// </summary>
        /// <returns></returns>
        public Task<byte[]> ExportFormAddRoom()
            {
            ExcelPackage.LicenseContext=LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
                {
                var worksheet = package.Workbook.Worksheets.Add("Rooms");
                // Header
                worksheet.Cells[1, 1].Value="STT";
                worksheet.Cells[1, 2].Value="Mã phòng học";
                worksheet.Cells[1, 3].Value="Vị trí";
                // Gán công thức tự động tăng STT từ dòng 2 đến 1000
                for (int row = 2; row<=1000; row++)
                    {
                    worksheet.Cells[row, 1].Formula=$"=ROW()-1";
                    }
                worksheet.Cells.AutoFitColumns();
                return Task.FromResult(package.GetAsByteArray());
                }
            }
        #endregion

        #region Export Room Information
        /// <summary>
        /// Export Room Information
        /// </summary>
        /// <param name="majorId"></param>
        /// <returns></returns>
        public async Task<byte[]> ExportRoomsToExcel(int? status)
            {
            var models = await _roomRepository.GetAllRooms();
            if (status.HasValue)
                models=models.Where(s => s.Status==status.Value).ToList();
            ExcelPackage.LicenseContext=OfficeOpenXml.LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
                {
                var worksheet = package.Workbook.Worksheets.Add("Rooms");
                // Header
                worksheet.Cells[1, 1].Value="STT";
                worksheet.Cells[1, 2].Value="Mã phòng học";
                worksheet.Cells[1, 3].Value="Vị trí";
                worksheet.Cells[1, 4].Value="Trạng thái";
                worksheet.Cells[1, 5].Value="Thời gian tạo";
                worksheet.Cells[1, 6].Value="Thời gian cập nhật";
                int row = 2;
                int stt = 1;
                foreach (var s in models)
                    {
                    worksheet.Cells[row, 1].Value=stt++;
                    worksheet.Cells[row, 2].Value=s.RoomId;
                    worksheet.Cells[row, 3].Value=s.Location;
                    worksheet.Cells[row, 4].Value=s.Status==1 ? "Đang khả dụng" : s.Status==0 ? "Vô hiệu hóa" : "Đang bảo trì";
                    worksheet.Cells[row, 5].Value=s.CreateAt.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 6].Value=s.UpdateAt.ToString("dd/MM/yyyy");
                    row++;
                    }
                return package.GetAsByteArray();
                }
            }
        #endregion

        #region Import Rooms from Excels
        /// <summary>
        /// Import Rooms Form Excel
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<APIResponse> ImportRoomsFromExcel(IFormFile file)
            {
            try
                {
                if (file==null||file.Length==0)
                    {
                    return new APIResponse { IsSuccess=false, Message="File không hợp lệ." };
                    }
                var models = new List<Room>();
                ExcelPackage.LicenseContext=OfficeOpenXml.LicenseContext.NonCommercial;
                using (var stream = new MemoryStream())
                    {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                        {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row<=rowCount; row++)
                            {
                            if (string.IsNullOrWhiteSpace(worksheet.Cells[row, 2].Text))
                                {
                                break;
                                }
                            var model = new Room
                                {
                                RoomId=worksheet.Cells[row, 2].Text,
                                Location=worksheet.Cells[row, 3].Text,
                                Status=1,
                                CreateAt=DateTime.Now,
                                UpdateAt=DateTime.Now
                                };
                            #region 1. Validation       
                            string stt = worksheet.Cells[row, 1].Text;
                            model.RoomId=model.RoomId?.Trim();
                            var existingSubject = await _roomRepository.GetRoomById(model.RoomId);
                            List<(bool condition, string errorMessage)>? validations = new List<(bool condition, string errorMessage)>
                              {
                                 (existingSubject != null,"Mã phòng học đã tồn tại!"),
                                 (model.RoomId.Length>6,"Mã phòng học tại ô số "+ stt + " không thể dài hơn 6 ký tự!"),
                                 (model.Location.Length>300,"Vị trí tại ô số "+ stt + " không thể dài hơn 300 ký tự!"),
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
                            models.Add(model);
                            }
                        }
                    }
                bool isSuccess = await _roomRepository.AddRoomsAsyncs(models);
                if (isSuccess)
                    {
                    return new APIResponse { IsSuccess=true, Message="Import phòng học thành công." };
                    }
                return new APIResponse { IsSuccess=false, Message="Import phòng học thất bại." };
                }
            catch (Exception ex)
                {
                return new APIResponse { IsSuccess=false, Message=ex.Message };
                }
            }
        #endregion
        }
    }
