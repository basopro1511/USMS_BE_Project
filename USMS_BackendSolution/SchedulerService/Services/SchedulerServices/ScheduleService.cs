using ISUZU_NEXT.Server.Core.Extentions;
using Newtonsoft.Json;
using Repositories.ScheduleRepository;
using SchedulerBusinessObject;
using SchedulerBusinessObject.AppDBContext;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;
using Services.RoomServices;
using System.Text.Json;

namespace SchedulerDataAccess.Services.SchedulerServices
    {
    public class ScheduleService
        {
        private readonly RoomService _roomService;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly HttpClient _httpClient;
        public ScheduleService(HttpClient httpClient) {
            _scheduleRepository=new ScheduleRepository();
            _roomService=new RoomService();
            _httpClient=httpClient;
            }
        #region Get All Schedule
        public APIResponse GetAllSchedule() {
            APIResponse aPIResponse = new APIResponse();
            var schedule = _scheduleRepository.getAllSchedule();
            if(schedule==null) {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Don't have any schedule available!";
                }
            aPIResponse.Result=schedule;
            return aPIResponse;
            }
        #endregion

        //#region Post Schedule
        ///// <summary>
        ///// Post Schedule
        ///// </summary>
        ///// <param name="schedule"></param>
        ///// <returns></returns>
        //public async Task<APIResponse> PostSchedule(ClassScheduleDTO schedule)
        //{
        //	APIResponse aPIResponse = new APIResponse();
        //	try
        //	{
        //		var classSubjectList = await GetClassSubjects();
        //		if (classSubjectList == null)
        //		{
        //			aPIResponse.IsSuccess = false;
        //			aPIResponse.Message = "Không tìm thấy bất kỳ lớp nào!";
        //			return aPIResponse;
        //		}
        //		// Dựa theo ClassSubjectId tìm classSubject
        //		var classSubject = classSubjectList.FirstOrDefault(cs => cs.ClassSubjectId == schedule.ClassSubjectId);
        //		if (classSubject == null)
        //		{
        //			aPIResponse.IsSuccess = false;
        //			aPIResponse.Message = "Không tìm thấy lớp này!";
        //			return aPIResponse;
        //		}
        //		// Này lấy hết tất cả lớp của SE1702
        //		var classSubjectOfClass = classSubjectList.Where(x => x.ClassId == classSubject.ClassId).ToList();
        //		// Này lấy tất cả thời khóa biểu của ngày hôm đó ngay slot đó
        //		var existingSchedules =  _scheduleRepository.GetSchedulesByDateAndSlot(schedule.Date, schedule.SlotId);
        //		// Kiểm tra xung đột lịch học
        //		if (CheckSlotConflict(classSubjectOfClass, existingSchedules))
        //		{
        //			aPIResponse.IsSuccess = false;
        //			aPIResponse.Message = "Lớp này đã có tiết vào thời gian này!";
        //			return aPIResponse;
        //		}
        //		// Kiểm tra tính hợp lệ của phòng học
        //		var room = _roomService.GetRoomById(schedule.RoomId);
        //		if (!room.IsSuccess)
        //		{
        //			aPIResponse.IsSuccess = false;
        //			aPIResponse.Message = "Không tìm thấy phòng học!";
        //			return aPIResponse;
        //		}
        //		// Kiểm tra xung đột phòng học
        //		if (CheckRoomConflict(existingSchedules, schedule.RoomId))
        //		{
        //			aPIResponse.IsSuccess = false;
        //			aPIResponse.Message = "Phòng đã được sử dụng vào thời gian này!";
        //			return aPIResponse;
        //		}
        //		// Không có gì thay đổi thì thêm lịch mới
        //		var newSchedule = new Schedule();
        //		newSchedule.CopyProperties(schedule);
        //		newSchedule.Status = 1;
        //		await _scheduleRepository.AddSchedule(newSchedule);
        //		aPIResponse.IsSuccess = true;
        //		aPIResponse.Message = "Thêm thời khóa biểu thành công!";
        //	}
        //	catch (Exception ex)
        //	{
        //		aPIResponse.IsSuccess = false;
        //		aPIResponse.Message = ex.Message;
        //	}
        //	return aPIResponse;
        //}
        //      #endregion

        #region Post Schedule (kết hợp check conflict & SlotNoInSubject)
        /// <summary>
        /// Thêm mới 1 lịch học (schedule), có kiểm tra xung đột & đảm bảo SlotNoInSubject <= NumberOfSlot của môn
        /// </summary>
        public async Task<APIResponse> AddNewSchedule(ClassScheduleDTO schedule) {
            APIResponse aPIResponse = new APIResponse();
            try {
                // 1. Lấy danh sách tất cả ClassSubject (bạn đang gọi 1 hàm GetClassSubjects() ở đâu đó)
                var classSubjectList = await GetClassSubjects();
                if(classSubjectList==null||classSubjectList.Count==0) {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Không tìm thấy bất kỳ lớp nào!";
                    return aPIResponse;
                    }
                // 2. Tìm ClassSubject tương ứng với schedule.ClassSubjectId
                var classSubject = classSubjectList
                    .FirstOrDefault(cs => cs.ClassSubjectId==schedule.ClassSubjectId);
                if(classSubject==null) {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Không tìm thấy lớp này!";
                    return aPIResponse;
                    }
                // 3. Lấy môn học (Subject) tương ứng
                var subjectRes = await GetSubjectById(classSubject.SubjectId);
                if(subjectRes==null||!subjectRes.IsSuccess||subjectRes.Result==null) {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Không lấy được môn học tương ứng!";
                    return aPIResponse;
                    }
                //// Giả sử subjectRes.Result là SubjectDTO { NumberOfSlot = ??? }
                var subjectDto = JsonConvert.DeserializeObject<SubjectDTO>(
                     subjectRes.Result.ToString()
                );
                int maxSlot = subjectDto.NumberOfSlot; // Số tiết tối đa của môn
                // 4. Kiểm tra schedule.SlotNoInSubject với subjectDto.NumberOfSlot
                //    (SlotNoInSubject là trường mới bạn thêm trong ClassScheduleDTO)
                var schedulesOfThisClassSubject = _scheduleRepository.GetSchedulesByClassSubjectId(schedule.ClassSubjectId);
                int nextSlotNoInSubject = 1;
                if(schedulesOfThisClassSubject.Any()) {
                    // Lấy max
                    int currentMax = schedulesOfThisClassSubject.Max(x => x.SlotNoInSubject);
                    nextSlotNoInSubject=currentMax+1;
                    }
                // 5. Lấy danh sách tất cả ClassSubject cùng ClassId (ví dụ SE1702) => để check conflict
                var classSubjectOfClass = classSubjectList
                    .Where(x => x.ClassId==classSubject.ClassId)
                    .ToList();
                // 6. Lấy tất cả lịch học đã có ở cùng ngày + slotId => Check xung đột
                var existingSchedules = _scheduleRepository.GetSchedulesByDateAndSlot(schedule.Date,schedule.SlotId);
                // 7. Check xung đột lịch học (logic cũ)
                if(CheckSlotConflict(classSubjectOfClass,existingSchedules)) {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Lớp này đã có tiết vào thời gian này!";
                    return aPIResponse;
                    }
                // 8. Kiểm tra tính hợp lệ phòng học
                var room = _roomService.GetRoomById(schedule.RoomId);
                if(!room.IsSuccess||room.Result==null) {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Không tìm thấy phòng học!";
                    return aPIResponse;
                    }
                // 9. Check xung đột phòng
                if(CheckRoomConflict(existingSchedules,schedule.RoomId)) {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Phòng đã được sử dụng vào thời gian này!";
                    return aPIResponse;
                    }
                // 10. Không có xung đột, tiến hành thêm lịch mới
                var newSchedule = new Schedule();
                newSchedule.CopyProperties(schedule);
                newSchedule.Status=1;
                newSchedule.SlotNoInSubject=nextSlotNoInSubject;
                await _scheduleRepository.AddSchedule(newSchedule);
                aPIResponse.IsSuccess=true;
                aPIResponse.Message="Thêm thời khóa biểu thành công!";
                }
            catch(Exception ex) {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message=ex.Message;
                }
            return aPIResponse;
            }
        #endregion

        #region Get Subject By Id (gọi qua API Subject)
        /// <summary>
        /// Lấy thông tin Subject theo SubjectId, bằng cách gọi API bên SubjectController
        /// </summary>
        /// <param name="subjectId">Mã môn học</param>
        /// <returns>APIResponse chứa thông tin môn học (SubjectDTO) hoặc lỗi</returns>
        public async Task<APIResponse> GetSubjectById(string subjectId) {
            APIResponse response = new APIResponse();
            try {
                // Ví dụ gọi API: GET /api/Subject/{subjectId}
                // Tuỳ bạn định nghĩa route, có thể là /api/Subject?subjectId=...
                using(var client = new HttpClient()) {
                    // Đây là base URL cho service Subject, bạn thay đổi theo config của bạn
                    client.BaseAddress=new Uri("https://localhost:7067/");
                    // Gọi API GET
                    var result = await client.GetAsync($"api/Subjects/{subjectId}");
                    if(result.IsSuccessStatusCode) {
                        var jsonString = await result.Content.ReadAsStringAsync();
                        var apiRes = JsonConvert.DeserializeObject<APIResponse>(jsonString);
                        // Gán vào biến response để trả ra ngoài
                        response=apiRes;
                        }
                    else {
                        response.IsSuccess=false;
                        response.Message=$"Không tìm thấy subject với ID: {subjectId}";
                        }
                    }
                }
            catch(Exception ex) {
                response.IsSuccess=false;
                response.Message=$"Lỗi khi gọi API Subject: {ex.Message}";
                }
            return response;
            }
        #endregion

        #region Get Class Subjec ( Gọi qua API )
        /// <summary>
        /// Get ClassSubjectOfClass
        /// </summary>
        /// <returns></returns>
        private async Task<List<ClassSubjectDTO>?> GetClassSubjects() {
            // Lấy danh sách ClassSubject từ API
            var response = await _httpClient.GetAsync("https://localhost:7067/api/ClassSubject");
            var apiResponse = await response.Content.ReadFromJsonAsync<APIResponse>();
            if(apiResponse==null||!apiResponse.IsSuccess) {
                return null;
                }
            //Nhận dạng là một Json
            var classSubjectResponse = apiResponse.Result as JsonElement?;
            if(classSubjectResponse==null) {
                return null;
                }

            var options = new JsonSerializerOptions {
                PropertyNameCaseInsensitive=true
                };
            return classSubjectResponse.Value.Deserialize<List<ClassSubjectDTO>>(options);
            }
        #endregion

        #region Check Slot Conflict
        /// <summary>
        /// Check Slot Conflict
        /// </summary>
        /// <param name="classSubjectOfClass"></param>
        /// <param name="existingSchedules"></param>
        /// <returns></returns>
        private bool CheckSlotConflict(List<ClassSubjectDTO> classSubjectOfClass,List<Schedule>? existingSchedules) {
            if(existingSchedules==null||existingSchedules.Count==0) {
                return false;
                }
            return existingSchedules.Any(cs =>
                                            classSubjectOfClass.Any(csc =>
                                                                        csc.ClassSubjectId==cs.ClassSubjectId));
            }
        #endregion

        #region Check Room Conflict
        /// <summary>
        /// Check Room Conflict
        /// </summary>
        /// <param name="existingSchedules"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        private bool CheckRoomConflict(List<Schedule>? existingSchedules,string roomId) {
            return (existingSchedules!=null&&
                existingSchedules.Any(es =>
                                        es.RoomId==roomId));
            }
        #endregion

        #region Get Class SubjectIds by Student Id ( Gọi qua API Student In Class )
        private List<int> getClassSubjectIdsByStudentIds(string id) {
            try {
                var response = _httpClient.GetAsync($"https://localhost:7067/api/StudentInClass/ClassSubject/{id}").Result;
                var apiResponse = response.Content.ReadFromJsonAsync<APIResponse>().Result;
                if(apiResponse==null||!apiResponse.IsSuccess) {
                    return null;
                    }
                var dataResponse = apiResponse.Result as JsonElement?;
                if(dataResponse==null) {
                    return null;
                    }
                var options = new JsonSerializerOptions {
                    PropertyNameCaseInsensitive=true
                    };
                return dataResponse.Value.Deserialize<List<int>>(options);
                }
            catch(Exception ex) {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Class SubjectIds by MajorId, ClassId, SubjectId ( Gọi qua API Class Subject )
        private List<ClassSubjectDTO> getClassSubjectIdsByMajorIdClassIdSubjectId(string majorId,string classId,int term) {
            try {
                var response = _httpClient.GetAsync($"https://localhost:7286/api/ClassSubject/ClassSubject?majorId={majorId}&classId={classId}&term={term}").Result;
                var apiResponse = response.Content.ReadFromJsonAsync<APIResponse>().Result;
                if(apiResponse==null||!apiResponse.IsSuccess) {
                    return null;
                    }
                var dataResponse = apiResponse.Result as JsonElement?;
                if(dataResponse==null) {
                    return null;
                    }
                var options = new JsonSerializerOptions {
                    PropertyNameCaseInsensitive=true
                    };
                return dataResponse.Value.Deserialize<List<ClassSubjectDTO>>(options);
                }
            catch(Exception ex) {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Schedule for Student
        public APIResponse GetClassSchedulesByStudentIds(string studentId)  {
            APIResponse aPIResponse = new APIResponse();
            var classSubjectIds = getClassSubjectIdsByStudentIds(studentId);
            if(classSubjectIds==null) {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không tìm thấy lớp học của sinh viên có Id = "+studentId+"!";
                return aPIResponse;
                }
            aPIResponse.Result=_scheduleRepository.GetClassSchedulesByClassSubjectIds(classSubjectIds);
            return aPIResponse;
            }
        #endregion

        #region Get Schedule for Class
        public APIResponse GetClassSchedulesForClass(string majorId,string classId,int term,DateTime startDay,DateTime endDay) {
            APIResponse aPIResponse = new APIResponse();
            try {
                var classSubjects = getClassSubjectIdsByMajorIdClassIdSubjectId(majorId,classId,term);
                if(classSubjects==null) {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Không tìm thấy bất kỳ lớp nào!";
                    return aPIResponse;
                    };
                List<int> classSubjectIds = classSubjects.Select(s => s.ClassSubjectId).ToList();
                Dictionary<int,string> subjectMap = classSubjects.ToDictionary(s => s.ClassSubjectId,s => s.SubjectId);
                using(var _dbContext = new MyDbContext()) {
                    var schedules = _dbContext.Schedule
                        .Where(s => classSubjectIds.Contains(s.ClassSubjectId)
                                 &&s.Date>=DateOnly.FromDateTime(startDay)
                                 &&s.Date<=DateOnly.FromDateTime(endDay))
                        .Select(s => new ViewScheduleDTO {
                            ClassScheduleId=s.ScheduleId,
                            ClassSubjectId=s.ClassSubjectId,
                            ClassId=classId,
                            MajorId=majorId,
                            SubjectId=subjectMap.ContainsKey(s.ClassSubjectId) ? subjectMap[s.ClassSubjectId] : null, // Lấy SubjectId theo ClassSubjectId
                            SlotId=s.SlotId,
                            TeacherId=s.TeacherId,
                            Date=s.Date,
                            Status=s.Status,
                            RoomId=s.RoomId,
                            SlotNoInSubject=s.SlotNoInSubject
                            })
                        .ToList();
                    aPIResponse.Result=schedules;

                    }
                return aPIResponse;
                }
            catch(Exception ex) {
                throw new Exception("An error occurred while fetching class schedules.",ex);
                }
            }
        #endregion

        #region Put Schedule - Update Schedule
        /// <summary>
        /// Cập nhật 1 lịch học (schedule), có kiểm tra xung đột
        /// </summary>
        public async Task<APIResponse> UpdateSchedule(ScheduleDTO scheduleDto) {
            APIResponse aPIResponse = new APIResponse();
            try {
                // 1. Kiểm tra xem lịch học cần cập nhật có tồn tại không
                var existingSchedule = _scheduleRepository.GetScheduleById(scheduleDto.ClassScheduleId);
                if(existingSchedule==null) {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Không tìm thấy lịch học cần cập nhật!";
                    return aPIResponse;
                    }
                // 2. Lấy danh sách ClassSubject (các lớp - môn học)
                var classSubjectList = await GetClassSubjects();
                if(classSubjectList==null||classSubjectList.Count==0) {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Không tìm thấy bất kỳ lớp nào!";
                    return aPIResponse;
                    }
                // 3. Tìm ClassSubject tương ứng với scheduleDto.ClassSubjectId
                var classSubject = classSubjectList.FirstOrDefault(cs => cs.ClassSubjectId==scheduleDto.ClassSubjectId);
                if(classSubject==null) {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Không tìm thấy lớp này!";
                    return aPIResponse;
                    }
                // 4. Lấy môn học (Subject) tương ứng
                var subjectRes = await GetSubjectById(classSubject.SubjectId);
                if(subjectRes==null||!subjectRes.IsSuccess||subjectRes.Result==null) {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Không lấy được môn học tương ứng!";
                    return aPIResponse;
                    }
                // 5. Lấy danh sách tất cả ClassSubject cùng ClassId (cùng lớp)
                var classSubjectOfClass = classSubjectList
                    .Where(x => x.ClassId==classSubject.ClassId)
                    .ToList();
                // 6. Kiểm tra xung đột lịch học theo ngày & slot (Không được trùng lịch của chính lớp này)
                var existingSchedules = _scheduleRepository.GetSchedulesByDateAndSlot(scheduleDto.Date,scheduleDto.SlotId)
                    .Where(sch => sch.ClassSubjectId!=scheduleDto.ClassSubjectId) // Không kiểm tra chính nó
                    .ToList();
                if(existingSchedules.Any()) {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Có lớp khác có tiết vào thời gian này!";
                    return aPIResponse;
                    }
                // 7. Kiểm tra xung đột phòng học
                var room = _roomService.GetRoomById(scheduleDto.RoomId);
                if(!room.IsSuccess||room.Result==null) {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Không tìm thấy phòng học!";
                    return aPIResponse;
                    }
                // Kiểm tra xem phòng đã có lịch trong cùng ngày + slot chưa
                var roomConflict = _scheduleRepository.GetSchedulesByDateAndSlot(scheduleDto.Date,scheduleDto.SlotId)
                    .Any(sch => sch.RoomId==scheduleDto.RoomId);
                if(roomConflict) {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Nếu không có bất kì thay đổi nào hãy nhấn nút 'Hủy' để giữ nguyên lịch học";
                    return aPIResponse;
                    }
                // 8. Cập nhật thông tin mới cho lịch học
                existingSchedule.CopyProperties(scheduleDto);
                existingSchedule.Status=scheduleDto.Status;

                _scheduleRepository.UpdateSchedule(existingSchedule);

                aPIResponse.IsSuccess=true;
                aPIResponse.Message="Cập nhật thời khóa biểu thành công!";
                }
            catch(Exception ex) {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message=ex.Message;
                }
            return aPIResponse;
            }
        #endregion

        #region Delete Schedule
        public APIResponse DeleteSchedule(int scheduleId) {
            APIResponse response = new APIResponse();
            try {
                var result = _scheduleRepository.DeleteScheduleById(scheduleId);
                if(!result) {
                    response.IsSuccess=false;
                    response.Message="Không tìm thấy lịch học để xóa!";
                    return response;
                    }
                response.IsSuccess=true;
                response.Message="Xóa lịch học thành công!";
                }
            catch(Exception ex) {
                response.IsSuccess=false;
                response.Message=$"Lỗi khi xóa lịch học: {ex.Message}";
                }
            return response;
            }
        #endregion

        }
    }
