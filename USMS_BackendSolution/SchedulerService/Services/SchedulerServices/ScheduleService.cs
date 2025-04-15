using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;
using Repositories.ScheduleRepository;
using SchedulerBusinessObject;
using SchedulerBusinessObject.AppDBContext;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;
using SchedulerService.Services.SlotServices;
using Services.RoomServices;
using System.Text.Json;

namespace SchedulerDataAccess.Services.SchedulerServices
    {
    public class ScheduleService
        {
        private readonly RoomService _roomService;
        private readonly SlotService _slotService;
        private readonly IScheduleRepository _scheduleRepository;
        private readonly HttpClient _httpClient;
        public ScheduleService(HttpClient httpClient)
            {
            _scheduleRepository=new ScheduleRepository();
            _roomService=new RoomService();
            _slotService=new SlotService();
            _httpClient=httpClient;
            }

        #region Get All Schedule
        /// <summary>
        /// Get All Schedule in database
        /// </summary>
        /// <returns></returns>
        public async Task<APIResponse> GetAllSchedule()
            {
            APIResponse aPIResponse = new APIResponse();
            var schedule = await _scheduleRepository.getAllSchedule();
            if (schedule==null)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không có bất kì lịch học nào tồn tại!";
                }
            aPIResponse.Result=schedule;
            return aPIResponse;
            }
        #endregion       

        //#region Get All Schedule
        ///// <summary>
        ///// Get All Schedule in database
        ///// </summary>
        ///// <returns></returns>
        //public async Task<APIResponse> GetAllSchedule()
        //    {
        //    APIResponse aPIResponse = new APIResponse();
        //    var schedule = await _scheduleRepository.getAllSchedule();
        //    if (schedule==null)
        //        {
        //        aPIResponse.IsSuccess=false;
        //        aPIResponse.Message="Không có bất kì lịch học nào tồn tại!";
        //        }
        //    aPIResponse.Result=schedule;
        //    return aPIResponse;
        //    }
        //#endregion

        #region Get Schedule by Id  
        /// <summary>
        /// Get Schedule By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<APIResponse> GetScheduleById(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            var schedule = await _scheduleRepository.GetScheduleById(id);
            if (schedule==null)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không có bất kì lịch học nào tồn tại!";
                }
            aPIResponse.Result=schedule;
            return aPIResponse;
            }
        #endregion

        #region CRUD Schedule

        #region Check Slot Conflict
        /// <summary>
        /// Check Slot Conflict
        /// </summary>
        /// <param name="classSubjectOfClass"></param>
        /// <param name="existingSchedules"></param>
        /// <returns></returns>
        private bool CheckSlotConflict(List<ClassSubjectDTO> classSubjectOfClass, List<Schedule>? existingSchedules)
            {
            if (existingSchedules==null||existingSchedules.Count==0)
                {
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
        private bool CheckRoomConflict(List<Schedule>? existingSchedules, string roomId)
            {
            return (existingSchedules!=null&&
                existingSchedules.Any(es =>
                                        es.RoomId==roomId));
            }
        #endregion

        #region Get Class Subject ( Gọi qua API )
        /// <summary>
        /// Get ClassSubjectOfClass
        /// </summary>
        /// <returns></returns>
        private async Task<List<ClassSubjectDTO>?> GetClassSubjects()
            {
            // Lấy danh sách ClassSubject từ API
            var response = await _httpClient.GetAsync("https://localhost:7067/api/ClassSubject");
            var apiResponse = await response.Content.ReadFromJsonAsync<APIResponse>();
            if (apiResponse==null||!apiResponse.IsSuccess)
                {
                return null;
                }
            //Nhận dạng là một Json
            var classSubjectResponse = apiResponse.Result as JsonElement?;
            if (classSubjectResponse==null)
                {
                return null;
                }
            var options = new JsonSerializerOptions
                {
                PropertyNameCaseInsensitive=true
                };
            return classSubjectResponse.Value.Deserialize<List<ClassSubjectDTO>>(options);
            }
        #endregion

        #region Get Subject By Id (gọi qua API Subject)
        /// <summary>
        /// Lấy thông tin Subject theo SubjectId, bằng cách gọi API bên SubjectController
        /// </summary>
        /// <param name="subjectId">Mã môn học</param>
        /// <returns>APIResponse chứa thông tin môn học (SubjectDTO) hoặc lỗi</returns>
        public async Task<APIResponse> GetSubjectById(string subjectId)
            {
            APIResponse response = new APIResponse();
            try
                {
                using (var client = new HttpClient())
                    {
                    client.BaseAddress=new Uri("https://localhost:7067/");
                    // Gọi API GET
                    var result = await client.GetAsync($"api/Subjects/{subjectId}");
                    if (result.IsSuccessStatusCode)
                        {
                        var jsonString = await result.Content.ReadAsStringAsync();
                        var apiRes = JsonConvert.DeserializeObject<APIResponse>(jsonString);
                        response=apiRes;
                        }
                    else
                        {
                        response.IsSuccess=false;
                        response.Message=$"Không tìm thấy subject với ID: {subjectId}";
                        }
                    }
                }
            catch (Exception ex)
                {
                response.IsSuccess=false;
                response.Message=$"Lỗi khi gọi API Subject: {ex.Message}";
                }
            return response;
            }
        #endregion                      

        #region Post Schedule (kết hợp check conflict & SlotNoInSubject)
        /// <summary>
        ///  Thêm mới 1 lịch học (schedule), có kiểm tra xung đột & đảm bảo SlotNoInSubject <= NumberOfSlot của môn
        /// </summary>
        /// <param name="schedule"></param>
        /// <returns></returns>
        public async Task<APIResponse> AddNewSchedule(ClassScheduleDTO schedule)
            {
            APIResponse aPIResponse = new APIResponse();
            try
                {
                // 1. Lấy danh sách tất cả ClassSubject (bạn đang gọi 1 hàm GetClassSubjects() ở đâu đó)
                var classSubjectList = await GetClassSubjects();
                if (classSubjectList==null||classSubjectList.Count==0)
                    {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Không tìm thấy bất kỳ lớp nào!";
                    return aPIResponse;
                    }
                // 2. Tìm ClassSubject tương ứng với schedule.ClassSubjectId
                var classSubject = classSubjectList
                    .FirstOrDefault(cs => cs.ClassSubjectId==schedule.ClassSubjectId);
                if (classSubject==null)
                    {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Không tìm thấy lớp này!";
                    return aPIResponse;
                    }
                // 3. Lấy môn học (Subject) tương ứng
                var subjectRes = await GetSubjectById(classSubject.SubjectId);
                if (subjectRes==null||!subjectRes.IsSuccess||subjectRes.Result==null)
                    {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Không lấy được môn học tương ứng!";
                    return aPIResponse;
                    }
                //// Giả sử subjectRes.Result là SubjectDTO { NumberOfSlot = ??? }
                var subjectDto = JsonConvert.DeserializeObject<SubjectDTO>(
                     subjectRes.Result.ToString()
                );
                int maxSlot = subjectDto.NumberOfSlot; // Số tiết tối đa của môn
                                                       // 4. Lấy danh sách lịch học của ClassSubject
                var schedulesOfThisClassSubject = await _scheduleRepository.GetSchedulesByClassSubjectId(schedule.ClassSubjectId);
                // Tìm slot trống nhỏ nhất từ 1 đến maxSlot
                var usedSlots = schedulesOfThisClassSubject.Select(x => x.SlotNoInSubject).ToList();
                int nextSlotNoInSubject = Enumerable.Range(1, subjectDto.NumberOfSlot)
                    .FirstOrDefault(slot => !usedSlots.Contains(slot));

                if (nextSlotNoInSubject==0)
                    {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message=$"Môn học này chỉ có tối đa {subjectDto.NumberOfSlot} slot học. Không thể thêm lịch mới.";
                    return aPIResponse;
                    }

                // 5. Lấy danh sách tất cả ClassSubject cùng ClassId (ví dụ SE1702) => để check conflict
                var classSubjectOfClass = classSubjectList
                    .Where(x => x.ClassId==classSubject.ClassId)
                    .ToList();
                // 6. Lấy tất cả lịch học đã có ở cùng ngày + slotId => Check xung đột
                var existingSchedules = _scheduleRepository.GetSchedulesByDateAndSlot(schedule.Date, schedule.SlotId);
                // 7. Check xung đột lịch học (logic cũ)
                if (CheckSlotConflict(classSubjectOfClass, existingSchedules))
                    {
                    var getClassName = getClassSubjectByClassSubjectId(existingSchedules[0].ClassSubjectId);
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Lớp "+getClassName.ClassId+" đã có tiết vào thời gian này!";
                    return aPIResponse;
                    }
                // 8. Kiểm tra tính hợp lệ phòng học
                var room = await _roomService.GetRoomById(schedule.RoomId);
                if (!room.IsSuccess||room.Result==null)
                    {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Không tìm thấy phòng học!";
                    return aPIResponse;
                    }
                // 9. Check xung đột phòng
                if (CheckRoomConflict(existingSchedules, schedule.RoomId))
                    {
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
            catch (Exception ex)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message=ex.Message;
                }
            return aPIResponse;
            }
        #endregion

        #region Put Schedule - Update Schedule
        /// <summary>
        /// Update Schedule
        /// </summary>
        /// <param name="scheduleDto"></param>
        /// <returns></returns>
        public async Task<APIResponse> UpdateSchedule(ScheduleDTO scheduleDto)
            {
            APIResponse aPIResponse = new APIResponse();
            try
                {
                #region Validation
                // 1. Kiểm tra xem lịch học cần cập nhật có tồn tại không
                var existingSchedule = await _scheduleRepository.GetScheduleById(scheduleDto.ScheduleId);
                if (existingSchedule==null)
                    {
                    return new APIResponse
                        {
                        IsSuccess=false,
                        Message="Không tìm thấy lịch học cần cập nhật!"
                        };
                    }
                // 2. Lấy danh sách ClassSubject (các lớp - môn học)
                var classSubjectList = await GetClassSubjects();
                if (classSubjectList==null||classSubjectList.Count==0)
                    {
                    return new APIResponse
                        {
                        IsSuccess=false,
                        Message="Không tìm thấy bất kỳ lớp nào!"
                        };
                    }
                // 3. Tìm ClassSubject tương ứng với scheduleDto.ClassSubjectId
                var classSubject = classSubjectList.FirstOrDefault(cs => cs.ClassSubjectId==scheduleDto.ClassSubjectId);
                if (classSubject==null)
                    {
                    return new APIResponse
                        {
                        IsSuccess=false,
                        Message="Không tìm thấy lớp này!"
                        };
                    }
                // 4. Lấy danh sách tất cả ClassSubject cùng ClassId (cùng lớp)
                var classSubjectOfClass = classSubjectList.Where(x => x.ClassId==classSubject.ClassId).ToList();
                // 5. Lấy danh sách lịch đã có theo ngày và slot, loại trừ lịch đang cập nhật
                var existingSchedules = _scheduleRepository.GetSchedulesByDateAndSlot(scheduleDto.Date, scheduleDto.SlotId)
                    .Where(sch => sch.ScheduleId!=scheduleDto.ScheduleId)
                    .ToList();
                // 6. Kiểm tra xung đột lịch học theo ngày & slot (không được trùng lịch của lớp khác)
                bool slotConflict = CheckSlotConflict(classSubjectOfClass, existingSchedules);
                // 7. Kiểm tra tính hợp lệ phòng học
                var room = await _roomService.GetRoomById(scheduleDto.RoomId);
                if (!room.IsSuccess||room.Result==null)
                    {
                    return new APIResponse
                        {
                        IsSuccess=false,
                        Message="Không tìm thấy phòng học!"
                        };
                    }
                // 8. Kiểm tra xem phòng có đang sử dụng hay không, loại trừ lịch hiện tại
                bool roomConflict = _scheduleRepository.GetSchedulesByDateAndSlot(scheduleDto.Date, scheduleDto.SlotId)
                    .Where(sch => sch.ScheduleId!=scheduleDto.ScheduleId)
                    .Any(sch => sch.RoomId==scheduleDto.RoomId);

                // 9. Nếu phòng bị xung đột, hoặc lịch học trùng (xung đột slot) báo lỗi
                if (slotConflict)
                    {
                    return new APIResponse
                        {
                        IsSuccess=false,
                        Message="Lớp học khác đã có tiết vào thời gian này!"
                        };
                    }
                if (roomConflict)
                    {
                    return new APIResponse
                        {
                        IsSuccess=false,
                        Message="Phòng đã được sử dụng vào thời gian này!"
                        };
                    }
                // 10. Nếu không có bất kỳ thay đổi nào về thời gian, có thể báo lỗi và nhắc nhở người dùng nhấn "Hủy" nếu muốn giữ nguyên lịch học.
                bool noChange = existingSchedule.Date==scheduleDto.Date&&
                                existingSchedule.SlotId==scheduleDto.SlotId&&
                                existingSchedule.RoomId==scheduleDto.RoomId&&
                                existingSchedule.TeacherId==scheduleDto.TeacherId;
                if (noChange)
                    {
                    return new APIResponse
                        {
                        IsSuccess=false,
                        Message="Nếu không có bất kì thay đổi nào hãy nhấn nút 'Hủy' để giữ nguyên lịch học."
                        };
                    }
                #endregion
                // 11. Cập nhật thông tin mới cho lịch học
                existingSchedule.CopyProperties(scheduleDto);
                existingSchedule.Status=scheduleDto.Status;
                await _scheduleRepository.UpdateSchedule(existingSchedule);
                aPIResponse.IsSuccess=true;
                aPIResponse.Message="Cập nhật thời khóa biểu thành công!";
                }
            catch (Exception ex)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message=ex.Message;
                }
            return aPIResponse;
            }
        #endregion

        #region Delete Schedule
        public async Task<APIResponse> DeleteSchedule(int scheduleId)
            {
            APIResponse response = new APIResponse();
            try
                {
                var result = await _scheduleRepository.DeleteScheduleById(scheduleId);
                if (!result)
                    {
                    response.IsSuccess=false;
                    response.Message="Không tìm thấy lịch học để xóa!";
                    return response;
                    }
                response.IsSuccess=true;
                response.Message="Xóa lịch học thành công!";
                }
            catch (Exception ex)
                {
                response.IsSuccess=false;
                response.Message=$"Lỗi khi xóa lịch học: {ex.Message}";
                }
            return response;
            }
        #endregion

        #endregion

        #region Get Class SubjectIds by StudentId ( Gọi qua API Student In Class )
        private List<int> getClassSubjectIdsByStudentIds(string id)
            {
            try
                {
                var response = _httpClient.GetAsync($"https://localhost:7067/api/StudentInClass/ClassSubject/{id}").Result;
                var apiResponse = response.Content.ReadFromJsonAsync<APIResponse>().Result;
                if (apiResponse==null||!apiResponse.IsSuccess)
                    {
                    return null;
                    }
                var dataResponse = apiResponse.Result as JsonElement?;
                if (dataResponse==null)
                    {
                    return null;
                    }
                var options = new JsonSerializerOptions
                    {
                    PropertyNameCaseInsensitive=true
                    };
                return dataResponse.Value.Deserialize<List<int>>(options);
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Class SubjectIds by MajorId, ClassId, Term ( Gọi qua API Class Subject )
        private List<ClassSubjectDTO> getClassSubjectIdsByMajorIdClassIdAndTerm(string majorId, string classId, int term)
            {
            try
                {
                var response = _httpClient.GetAsync($"https://localhost:7286/api/ClassSubject/ClassSubject?majorId={majorId}&classId={classId}&term={term}").Result;
                var apiResponse = response.Content.ReadFromJsonAsync<APIResponse>().Result;
                if (apiResponse==null||!apiResponse.IsSuccess)
                    {
                    return null;
                    }
                var dataResponse = apiResponse.Result as JsonElement?;
                if (dataResponse==null)
                    {
                    return null;
                    }
                var options = new JsonSerializerOptions
                    {
                    PropertyNameCaseInsensitive=true
                    };
                return dataResponse.Value.Deserialize<List<ClassSubjectDTO>>(options);
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Class SubjectIds by MajorId, ClassId, SemesterId, Term ( Gọi qua API Class Subject )
        private List<ClassSubjectDTO> getClassSubjectIdsByMajorIdClassIdSemesterIdAndTerm(string majorId, string classId, string semesterId, int term)
            {
            try
                {
                var response = _httpClient.GetAsync($"https://localhost:7286/api/ClassSubject/ClassSubject?majorId={majorId}&classId={classId}&classId={semesterId}&term={term}").Result;
                var apiResponse = response.Content.ReadFromJsonAsync<APIResponse>().Result;
                if (apiResponse==null||!apiResponse.IsSuccess)
                    {
                    return null;
                    }
                var dataResponse = apiResponse.Result as JsonElement?;
                if (dataResponse==null)
                    {
                    return null;
                    }
                var options = new JsonSerializerOptions
                    {
                    PropertyNameCaseInsensitive=true
                    };
                return dataResponse.Value.Deserialize<List<ClassSubjectDTO>>(options);
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Semester Data ( Gọi qua API Class Subject )
        private SemesterDTO getSemesterBySemesterId(string semesterId)
            {
            try
                {
                var response = _httpClient.GetAsync($"https://localhost:7286/api/Semester/{semesterId}").Result;
                var apiResponse = response.Content.ReadFromJsonAsync<APIResponse>().Result;
                if (apiResponse==null||!apiResponse.IsSuccess)
                    {
                    return null;
                    }
                var dataResponse = apiResponse.Result as JsonElement?;
                if (dataResponse==null)
                    {
                    return null;
                    }
                var options = new JsonSerializerOptions
                    {
                    PropertyNameCaseInsensitive=true
                    };
                return dataResponse.Value.Deserialize<SemesterDTO>(options);
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get ClassSubject by ClassSubjectId
        /// <summary>
        /// This method will call API from ClassSubjectController to get Data by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private ClassSubjectDTO getClassSubjectByClassSubjectId(int id)
            {
            try
                {
                var response = _httpClient.GetAsync($"https://localhost:7286/api/ClassSubject/{id}").Result;
                var apiResponse = response.Content.ReadFromJsonAsync<APIResponse>().Result;
                if (apiResponse==null||!apiResponse.IsSuccess)
                    {
                    return null;
                    }
                var dataResponse = apiResponse.Result as JsonElement?;
                if (dataResponse==null)
                    {
                    return null;
                    }
                var options = new JsonSerializerOptions
                    {
                    PropertyNameCaseInsensitive=true
                    };
                return dataResponse.Value.Deserialize<ClassSubjectDTO>(options);
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Schedule for Student
        /// <summary>
        /// Get Schedule for Student
        /// </summary>
        /// <param name="studentId"></param>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        /// <returns></returns>
        public async Task<APIResponse> GetClassScheduleForStudent(string studentId, DateTime startDay, DateTime endDay)
            {
            APIResponse aPIResponse = new APIResponse();
            var classSubjectIds = getClassSubjectIdsByStudentIds(studentId);
            if (classSubjectIds==null)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không tìm thấy lớp học của sinh viên có Id = "+studentId+"!";
                return aPIResponse;
                }
            List<Schedule>? schedules = await _scheduleRepository.GetClassSchedulesForStaff(classSubjectIds, startDay, endDay);
            List<ViewScheduleDTO> viewScheduleDTOs = new List<ViewScheduleDTO>();
            foreach (var item in schedules)
                {
                var dto = new ViewScheduleDTO();
                dto.ScheduleId=item.ScheduleId;
                dto.CopyProperties(item);
                var classSubject = getClassSubjectByClassSubjectId(item.ClassSubjectId);
                dto.ClassId=classSubject.ClassId;
                dto.SubjectId=classSubject.SubjectId;
                dto.MajorId=classSubject.MajorId;
                viewScheduleDTOs.Add(dto);
                }
            aPIResponse.Result=viewScheduleDTOs;
            return aPIResponse;
            }
        #endregion

        #region Get Schedule for Class (Academic Staff)
        /// <summary>
        /// Get ScheduleForStaff
        /// </summary>
        /// <param name="majorId"></param>
        /// <param name="classId"></param>
        /// <param name="term"></param>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<APIResponse> GetClassSchedulesForStaff(string majorId, string classId, int term, DateTime startDay, DateTime endDay)
            {
            APIResponse aPIResponse = new APIResponse();
            try
                {
                List<ClassSubjectDTO>? classSubjects = getClassSubjectIdsByMajorIdClassIdAndTerm(majorId, classId, term);
                if (classSubjects==null)
                    {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Không tìm thấy bất kỳ lớp nào!";
                    return aPIResponse;
                    };
                List<int> classSubjectIds = classSubjects.Select(s => s.ClassSubjectId).ToList();
                Dictionary<int, string> subjectMap = classSubjects.ToDictionary(s => s.ClassSubjectId, s => s.SubjectId);
                List<Schedule>? schedules = await _scheduleRepository.GetClassSchedulesForStaff(classSubjectIds, startDay, endDay);
                List<ViewScheduleDTO> viewScheduleDTOs = new List<ViewScheduleDTO>();
                foreach (var schedule in schedules)
                    {
                    var dto = new ViewScheduleDTO();
                    dto.CopyProperties(schedule);
                    dto.MajorId=majorId;
                    dto.ClassId=classId;
                    dto.SubjectId=subjectMap.ContainsKey(schedule.ClassSubjectId) ? subjectMap[schedule.ClassSubjectId] : null; // Lấy SubjectId theo ClassSubjectId
                    viewScheduleDTOs.Add(dto);
                    }
                aPIResponse.Result=viewScheduleDTOs;
                return aPIResponse;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Schedule for Teacher
        /// <summary>
        /// Get Schedule for Teacher 
        /// </summary>
        /// <param name="teacherId"></param>
        /// <param name="startDay"></param>
        /// <param name="endDay"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<APIResponse> GetClassSchedulesForTeacher(string teacherId, DateTime startDay, DateTime endDay)
            {
            APIResponse aPIResponse = new APIResponse();
            try
                {
                var schedules = await _scheduleRepository.GetScheduleForTeacher(teacherId, startDay, endDay);
                List<ViewScheduleDTO> viewScheduleDTOs = new List<ViewScheduleDTO>();
                foreach (var item in schedules)
                    {
                    var dto = new ViewScheduleDTO();
                    dto.CopyProperties(item);
                    dto.ScheduleId=item.ScheduleId;
                    var classSubject = getClassSubjectByClassSubjectId(item.ClassSubjectId);
                    dto.ClassId=classSubject.ClassId;
                    dto.SubjectId=classSubject.SubjectId;
                    dto.MajorId=classSubject.MajorId;
                    viewScheduleDTOs.Add(dto);
                    }
                aPIResponse.Result=viewScheduleDTOs;
                return aPIResponse;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Schedule By Date And Slot       
        /// <summary>
        /// Get Schedule By Date and Slot to check conflict
        /// </summary>
        /// <param name="date"></param>
        /// <param name="slot"></param>
        /// <returns></returns>
        public APIResponse GetSchedulesByDateAndSlot(DateOnly date, int slot)
            {
            APIResponse aPIResponse = new APIResponse();
            var schedule = _scheduleRepository.GetSchedulesByDateAndSlot(date, slot);
            if (schedule==null)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không tìm thấy lịch học khả dụng vào buổi "+slot+" ngày: "+date;
                }
            aPIResponse.Result=schedule;
            return aPIResponse;
            }
        #endregion

        #region Get Available Teacher
        private List<TeacherDTO> GetAvailableTeachersByMajorId(string majorId)
            {
            try
                {
                var response = _httpClient.GetAsync($"https://localhost:7067/api/Teacher/Available/{majorId}").Result;
                var apiResponse = response.Content.ReadFromJsonAsync<APIResponse>().Result;
                if (apiResponse==null||!apiResponse.IsSuccess)
                    {
                    return null;
                    }
                var dataResponse = apiResponse.Result as JsonElement?;
                if (dataResponse==null)
                    {
                    return null;
                    }
                var options = new JsonSerializerOptions
                    {
                    PropertyNameCaseInsensitive=true
                    };
                return dataResponse.Value.Deserialize<List<TeacherDTO>>(options);
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get All Teachers Available
        /// <summary>
        /// Get All Teacher Available from Database
        /// </summary>
        /// <param name="teacherDto"></param>
        /// <returns></returns>
        public async Task<APIResponse> GetAllTeacherAvailableForAddSchedule(string majorId, DateOnly date, int slot)
            {
            APIResponse aPIResponse = new APIResponse();
            List<TeacherDTO> teachers = GetAvailableTeachersByMajorId(majorId);
            #region validation 
            List<(bool condition, string errorMessage)>? validations = new List<(bool condition, string errorMessage)>
            {
                  (teachers == null, "Không tìm thấy giáo viên!"),
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
            // 2. Lấy các schedule có date & slotId
            var schedules = _scheduleRepository.GetSchedulesByDateAndSlot(date, slot);
            // 3. Lấy danh sách teacherId đã bị chiếm
            var usedTeacherIds = schedules.Select(sch => sch.TeacherId).Distinct().ToHashSet();
            // 4. Lọc ra các giáo viên còn trống
            var availableTeacher = teachers
                .Where(r => !usedTeacherIds.Contains(r.UserId))
                .ToList();
            aPIResponse.IsSuccess=true;
            aPIResponse.Result=availableTeacher;
            return aPIResponse;
            }
        #endregion

        #region Change Schedule Status Selected 
        /// <summary>
        /// Change Schedule status
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<APIResponse> ChangeScheduleStatus(string majorId, string classId, int term, int status)
            {
            APIResponse aPIResponse = new APIResponse();
            List<ClassSubjectDTO>? classSubjects = getClassSubjectIdsByMajorIdClassIdAndTerm(majorId, classId, term);
            List<int> classSubjectIds = classSubjects.Select(s => s.ClassSubjectId).ToList();
            if (classSubjectIds==null||!classSubjectIds.Any())
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Danh sách lịch học không hợp lệ.";
                return aPIResponse;
                }
            bool isSuccess = await _scheduleRepository.ChangeScheduleStatus(classSubjectIds, status);
            if (isSuccess)
                {
                aPIResponse.IsSuccess=true;
                switch (status)
                    {
                    case 0:
                        aPIResponse.Message="Đã thay đổi trạng thái lịch học đã chọn thành 'Vô hiệu hóa'.";
                        break;
                    case 1:
                        aPIResponse.Message="Đã thay đổi trạng thái lịch học đã chọn thành 'Đang khả dụng'.";
                        break;
                    default:
                        aPIResponse.Message="Trạng thái không hợp lệ.";
                        break;
                    }
                }
            return aPIResponse;
            }
        #endregion

        //#region Sắp lịch học tự động
        //public async Task<APIResponse> AutoScheduleClasses(string majorId, string classId,string semesterId, int term, List<DayOfWeek> scheduledDays)
        //    {
        //    APIResponse response = new APIResponse();
        //    try
        //        {
        //        // Lấy tất cả ClassSubject từ API ( lớp nào cần sắp lịch mới lấy )
        //        var classSubjects = getClassSubjectIdsByMajorIdClassIdSemesterIdAndTerm(majorId,classId,semesterId,term);
        //        if (classSubjects==null||classSubjects.Count==0)
        //            {
        //            response.IsSuccess=false;
        //            response.Message="Không có lớp nào để sắp lịch.";
        //            return response;
        //            }
        //        var semesterDate = getSemesterBySemesterId(semesterId);
        //        using (var dbContext = new MyDbContext())
        //            {
        //            var timeSlots = await dbContext.TimeSlot.Where(s=> s.Status == 1).ToListAsync();
        //            var rooms = await dbContext.Room.Where(s => s.Status==1).ToListAsync();
        //            // Giả sử, với mỗi lớp, các ClassSubject có cùng ClassId; nhóm các môn theo ClassId
        //            // vd lớp SEC2501 có 3 môn tuy classID chung là SEC2501 tuy nhiên ClassSubjectId nó lại khác id
        //            var classGroups = classSubjects.GroupBy(cs => cs.ClassId);
        //            // Duyệt từng nhóm lớp để thêm từng lớp có môn riêng biệt
        //            foreach (var group in classGroups)
        //                {
        //                // Các môn học của lớp này
        //                var subjectsInClass = group.ToList();
        //                // Lưu số tiết đã sắp cho từng ClassSubject
        //                Dictionary<int, int> scheduledCount = new Dictionary<int, int>();
        //                foreach (var cs in subjectsInClass)
        //                    {
        //                    var listScheduled = await _scheduleRepository.GetSchedulesByClassSubjectId(cs.ClassSubjectId);
        //                    scheduledCount[cs.ClassSubjectId]=listScheduled!=null ? listScheduled.Count : 0;
        //                    }
        //                // Đếm số lần đã lên lịch cho lớp này để quyết định thứ tự (đảo thứ tự theo lần sắp lịch)
        //                int schedulingIteration = 0;
        //                // Duyệt theo từng ngày trong khoảng được chọn
        //                for (DateOnly date = semesterDate.StartDate; date<=semesterDate.EndDate; date=date.AddDays(1))
        //                    {
        //                    // Chỉ xử lý nếu ngày này nằm trong danh sách cho phép
        //                    if (!scheduledDays.Contains(date.DayOfWeek))
        //                        continue;
        //                    // Tăng biến đếm để xác định thứ tự giảng dạy (sự xen kẽ)
        //                    schedulingIteration++;
        //                    // Sắp xếp thứ tự các môn của lớp:
        //                    // Nếu iteration là số chẵn, đảo ngược thứ tự so với ban đầu.
        //                    List<ClassSubjectDTO> orderedSubjects;
        //                    if (schedulingIteration%2==0)
        //                        orderedSubjects=subjectsInClass.OrderByDescending(x => x.ClassSubjectId).ToList();
        //                    else
        //                        orderedSubjects=subjectsInClass.OrderBy(x => x.ClassSubjectId).ToList();
        //                    // Với mỗi môn của lớp, kiểm tra xem đã đủ số tiết chưa (so với NumberOfSlot của môn)
        //                    foreach (var cs in orderedSubjects)
        //                        {
        //                        // Lấy thông tin Subject để biết số tiết tối đa
        //                        var subjectRes = await GetSubjectById(cs.SubjectId);
        //                        if (subjectRes==null||!subjectRes.IsSuccess||subjectRes.Result==null)
        //                            continue;
        //                        var subjectDto = JsonConvert.DeserializeObject<SubjectDTO>(subjectRes.Result.ToString());
        //                        int maxSlot = subjectDto.NumberOfSlot;
        //                        // Nếu đã đủ số tiết, bỏ qua môn này
        //                        if (scheduledCount[cs.ClassSubjectId]>=maxSlot)
        //                            continue;
        //                        // Chọn TimeSlot dựa trên thứ tự của môn. Nếu index vượt quá danh sách thì dùng slot đầu tiên
        //                        int subjectIndex = orderedSubjects.IndexOf(cs);
        //                        if (subjectIndex>=timeSlots.Count)
        //                            subjectIndex=0;
        //                        TimeSlot ts = timeSlots[subjectIndex];
        //                        // Lấy lịch học đã sắp theo (ngày, timeSlot) hiện tại
        //                        var existingSchedules =  _scheduleRepository.GetSchedulesByDateAndSlot(date, ts.SlotId);
        //                        // Kiểm tra xung đột về slot: đảm bảo lớp (các ClassSubject cùng ClassId) không có lịch nào vào slot này
        //                        var classSubjectsOfClass = classSubjects.Where(x => x.ClassId==cs.ClassId).ToList();
        //                        if (CheckSlotConflict(classSubjectsOfClass, existingSchedules))
        //                            {
        //                            // Nếu có xung đột, không sắp môn này vào ngày hiện tại.
        //                            continue;
        //                            }
        //                        // Tìm phòng học khả dụng tại slot hiện tại, đảm bảo không có xung đột phòng
        //                        Room availableRoom = null;
        //                        foreach (var room in rooms)
        //                            {
        //                            if (!CheckRoomConflict(existingSchedules, room.RoomId))
        //                                {
        //                                availableRoom=room;
        //                                break;
        //                                }
        //                            }
        //                        if (availableRoom==null)
        //                            {
        //                            // Nếu không tìm được phòng phù hợp, bỏ qua môn này ngày hiện tại.
        //                            continue;
        //                            }
        //                        // Nếu đạt được các điều kiện, tạo lịch học mới
        //                        Schedule newSchedule = new Schedule
        //                            {
        //                            ClassSubjectId=cs.ClassSubjectId,
        //                            SlotId=ts.SlotId,
        //                            RoomId=availableRoom.RoomId,
        //                            Date=date,
        //                            Status=1,
        //                            SlotNoInSubject=scheduledCount[cs.ClassSubjectId]+1,
        //                            };
        //                        await _scheduleRepository.AddSchedule(newSchedule);
        //                        scheduledCount[cs.ClassSubjectId]++;
        //                        } 
        //                    } 
        //                } 
        //            } 
        //        response.IsSuccess=true;
        //        response.Message="Sắp lịch tự động cho lớp "+ classId+" học kỳ "+semesterId +" kỳ học số " + term + " thành công. Thời gian bắt đầu từ "
        //            + semesterDate.StartDate +" đến " + semesterDate.EndDate ;
        //        }
        //    catch (Exception ex)
        //        {
        //        response.IsSuccess=false;
        //        response.Message=ex.Message;
        //        }
        //    return response;
        //    }
        //#endregion

        #region Auto Generate Schedule for Class
        /// <summary>
        /// Auto Generate Schedule for Class
        /// </summary>
        /// <param name="majorId"></param>
        /// <param name="classId"></param>
        /// <param name="semesterId"></param>
        /// <param name="term"></param>
        /// <param name="scheduledDays"></param>
        /// <returns></returns>
        public async Task<APIResponse> AutoScheduleClasses(string majorId, string classId, string semesterId, int term, List<DayOfWeek> scheduledDays)
            {
            APIResponse response = new APIResponse();
            try
                {
                List<ClassSubjectDTO> classSubjects = null;
                try
                    {
                    classSubjects=getClassSubjectIdsByMajorIdClassIdSemesterIdAndTerm(majorId, classId, semesterId, term);
                    }
                catch (Exception ex)
                    {
                    response.IsSuccess=false;
                    response.Message=$"Lỗi khi lấy danh sách ClassSubject: {ex.Message}";
                    return response;
                    }
                if (classSubjects==null||classSubjects.Count==0)
                    {
                    response.IsSuccess=false;
                    response.Message="Không có lớp nào để sắp lịch.";
                    return response;
                    }
                // 2. Lấy thông tin học kỳ để lấy startDate và endDate
                SemesterDTO semesterDate = null;
                try
                    {
                    semesterDate=getSemesterBySemesterId(semesterId);
                    }
                catch (Exception ex)
                    {
                    response.IsSuccess=false;
                    response.Message=$"Lỗi khi lấy thông tin học kỳ: {ex.Message}";
                    return response;
                    }
                if (semesterDate==null)
                    {
                    response.IsSuccess=false;
                    response.Message="Không lấy được thông tin học kỳ.";
                    return response;
                    }
                using (var dbContext = new MyDbContext())
                    {
                    List<TimeSlot> timeSlots = null;
                    List<Room> rooms = null;
                    try
                        {
                        timeSlots=await dbContext.TimeSlot.Where(s => s.Status==1).ToListAsync();
                        rooms=await dbContext.Room.Where(s => s.Status==1).ToListAsync();
                        }
                    catch (Exception ex)
                        {
                        response.IsSuccess=false;
                        response.Message=$"Lỗi khi truy xuất dữ liệu Buổi học / Phòng học: {ex.Message}";
                        return response;
                        }
                    if (timeSlots==null||timeSlots.Count==0||rooms==null||rooms.Count==0)
                        {
                        response.IsSuccess=false;
                        response.Message="Không có dữ liệu về Buổi học hoặc Phòng học khả dụng.";
                        return response;
                        }
                    // 3. Nhóm các ClassSubject theo ClassId (vì cùng lớp có thể có nhiều môn)
                    var classGroups = classSubjects.GroupBy(cs => cs.ClassId);
                    // Duyệt từng nhóm lớp
                    foreach (var group in classGroups)
                        {
                        List<ClassSubjectDTO> subjectsInClass = group.ToList();
                        // 3.1. Lấy số tiết đã sắp cho từng ClassSubject
                        Dictionary<int, int> scheduledCount = new Dictionary<int, int>();
                        foreach (var cs in subjectsInClass)
                            {
                            try
                                {
                                var listScheduled = await _scheduleRepository.GetSchedulesByClassSubjectId(cs.ClassSubjectId);
                                scheduledCount[cs.ClassSubjectId]=listScheduled!=null ? listScheduled.Count : 0;
                                }
                            catch (Exception ex)
                                {
                                response.IsSuccess=false;
                                response.Message=$"Lỗi khi lấy lịch của Lớp học {cs.ClassSubjectId}: {ex.Message}";
                                return response;
                                }
                            }
                        // 3.2. Xác định ngày đại diện (representative day) từ học kỳ
                        DateOnly representativeDay = DateOnly.MaxValue;
                        try
                            {
                            for (DateOnly date = semesterDate.StartDate; date<=semesterDate.EndDate; date=date.AddDays(1))
                                {
                                if (scheduledDays.Contains(date.DayOfWeek))
                                    {
                                    representativeDay=date;
                                    break;
                                    }
                                }
                            }
                        catch (Exception ex)
                            {
                            response.IsSuccess=false;
                            response.Message=$"Lỗi khi tính ngày đại diện: {ex.Message}";
                            return response;
                            }
                        if (representativeDay==DateOnly.MaxValue)
                            {
                            response.IsSuccess=false;
                            response.Message="Không có ngày đại diện trong học kỳ khớp với các ngày được chọn.";
                            return response;
                            }
                        // 3.3. Lấy danh sách giáo viên khả dụng cho chuyên ngành dựa theo representativeDay.
                        APIResponse teacherResponse = null;
                        try
                            {
                            // Sử dụng slot đầu tiên của timeSlots để kiểm tra giáo viên khả dụng
                            teacherResponse=await GetAllTeacherAvailableForAddSchedule(majorId, representativeDay, timeSlots.First().SlotId);
                            }
                        catch (Exception ex)
                            {
                            response.IsSuccess=false;
                            response.Message=$"Lỗi khi lấy danh sách giáo viên: {ex.Message}";
                            return response;
                            }
                        if (teacherResponse==null||!teacherResponse.IsSuccess||teacherResponse.Result==null)
                            {
                            response.IsSuccess=false;
                            response.Message=$"Không lấy được giáo viên khả dụng cho lớp {classId}.";
                            return response;
                            }
                        var availableTeachers = teacherResponse.Result as List<TeacherDTO>;
                        if (availableTeachers==null)
                            {
                            response.IsSuccess=false;
                            response.Message="Không có giáo viên khả dụng.";
                            return response;
                            }
                        // Kiểm tra số lượng giáo viên so với số môn
                        if (availableTeachers.Count<subjectsInClass.Count)
                            {
                            response.IsSuccess=false;
                            response.Message=$"Thiếu giáo viên khả dụng: lớp {classId} có {subjectsInClass.Count} môn nhưng chỉ có {availableTeachers.Count} giáo viên khả dụng.";
                            return response;
                            }
                        // 3.4. Gán giáo viên cho từng môn: dùng các giáo viên đầu tiên theo thứ tự
                        Dictionary<int, string> teacherAssignment = new Dictionary<int, string>();
                        for (int i = 0; i<subjectsInClass.Count; i++)
                            {
                            teacherAssignment[subjectsInClass[i].ClassSubjectId]=availableTeachers[i].UserId;
                            }
                        // 4. Tiến hành sắp lịch cho từng ngày trong học kỳ có nằm trong scheduledDays
                        int schedulingIteration = 0;
                        for (DateOnly date = semesterDate.StartDate; date<=semesterDate.EndDate; date=date.AddDays(1))
                            {
                            if (!scheduledDays.Contains(date.DayOfWeek))
                                continue;
                            schedulingIteration++;
                            // Đảo thứ tự các môn để xen kẽ lịch giảng dạy
                            List<ClassSubjectDTO> orderedSubjects;
                            if (schedulingIteration%2==0)
                                orderedSubjects=subjectsInClass.OrderByDescending(x => x.ClassSubjectId).ToList();
                            else
                                orderedSubjects=subjectsInClass.OrderBy(x => x.ClassSubjectId).ToList();
                            // Với mỗi môn trong lớp
                            foreach (var cs in orderedSubjects)
                                {
                                int maxSlot = 0;
                                // Lấy thông tin Subject để biết số tiết tối đa của môn
                                try
                                    {
                                    var subjectRes = await GetSubjectById(cs.SubjectId);
                                    if (subjectRes==null||!subjectRes.IsSuccess||subjectRes.Result==null)
                                        continue;
                                    var subjectDto = JsonConvert.DeserializeObject<SubjectDTO>(subjectRes.Result.ToString());
                                    maxSlot=subjectDto.NumberOfSlot;
                                    }
                                catch (Exception)
                                    {
                                    // Nếu lỗi xảy ra với môn học hiện tại thì ghi log và tiếp tục với môn kế tiếp
                                    continue;
                                    }
                                // Nếu đã sắp đủ số tiết thì bỏ qua môn này
                                if (scheduledCount[cs.ClassSubjectId]>=maxSlot)
                                    continue;
                                // Chọn TimeSlot dựa trên thứ tự của môn; nếu index vượt quá danh sách thì dùng slot đầu tiên
                                int subjectIndex = orderedSubjects.IndexOf(cs);
                                if (subjectIndex>=timeSlots.Count)
                                    subjectIndex=0;
                                TimeSlot ts = timeSlots[subjectIndex];
                                // Lấy các lịch đã có cho ngày và slot hiện tại
                                List<Schedule> existingSchedules = null;
                                try
                                    {
                                    existingSchedules=_scheduleRepository.GetSchedulesByDateAndSlot(date, ts.SlotId);
                                    }
                                catch (Exception)
                                    {
                                    // Nếu lỗi xảy ra khi lấy lịch, chuyển sang môn tiếp theo
                                    continue;
                                    }
                                // Kiểm tra xung đột về slot cho các ClassSubject cùng lớp
                                var classSubjectsOfClass = classSubjects.Where(x => x.ClassId==cs.ClassId).ToList();
                                if (CheckSlotConflict(classSubjectsOfClass, existingSchedules))
                                    continue;
                                // Tìm phòng học khả dụng tại slot hiện tại
                                Room availableRoom = null;
                                foreach (var room in rooms)
                                    {
                                    if (!CheckRoomConflict(existingSchedules, room.RoomId))
                                        {
                                        availableRoom=room;
                                        break;
                                        }
                                    }
                                if (availableRoom==null)
                                    continue;
                                // Lấy giáo viên được gán cho môn này từ teacherAssignment
                                string teacherId = null;
                                if (!teacherAssignment.TryGetValue(cs.ClassSubjectId, out teacherId))
                                    {
                                    response.IsSuccess=false;
                                    response.Message=$"Không gán được giáo viên cho môn có ClassSubjectId {cs.ClassSubjectId}.";
                                    return response;
                                    }
                                // Tạo lịch học mới
                                Schedule newSchedule = new Schedule
                                    {
                                    ClassSubjectId=cs.ClassSubjectId,
                                    SlotId=ts.SlotId,
                                    RoomId=availableRoom.RoomId,
                                    Date=date,
                                    Status=1,
                                    SlotNoInSubject=scheduledCount[cs.ClassSubjectId]+1,
                                    TeacherId=teacherId
                                    };
                                try
                                    {
                                    await _scheduleRepository.AddSchedule(newSchedule);
                                    scheduledCount[cs.ClassSubjectId]++;
                                    }
                                catch (Exception)
                                    {
                                    continue;
                                    }
                                }
                            }
                        }
                    }
                response.IsSuccess=true;
                response.Message=$"Sắp lịch tự động cho lớp {classId} học kỳ {semesterId} kỳ học số {term} thành công. Thời gian từ {semesterDate.StartDate} đến {semesterDate.EndDate}";
                }
            catch (Exception ex)
                {
                response.IsSuccess=false;
                response.Message=$"Có lỗi xảy ra: {ex.Message}";
                }
            return response;
            }
        #endregion

        #region Get ClassSubjectId by Teacher Schedule
        /// <summary>
        /// Get list of ClassSubjectId by Teacher Schedule for add Request
        /// </summary>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<APIResponse> GetClassSubjectIdByTeacherSchedule(string teacherId)
            {
            APIResponse aPIResponse = new APIResponse();
            try
                {
                var schedules = await _scheduleRepository.GetClassSubjcetIdByTeacherSchedule(teacherId);
                aPIResponse.Result=schedules;
                return aPIResponse;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get SlotNoInSubject By ClassSubjectId
        /// <summary>
        /// Get Slot in subject by class subjectId
        /// </summary>
        /// <param name="classSubjectId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<APIResponse> GetSlotNoInSubjectByClassSubjectId(int classSubjectId)
            {
            APIResponse aPIResponse = new APIResponse();
            try
                {
                var schedules = await _scheduleRepository.GetSlotNoInSubjectByClassSubjectId(classSubjectId);
                aPIResponse.Result=schedules;
                return aPIResponse;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Schedule Data By ScheduleId and Slot In Subject
        public async Task<APIResponse> GetScheduleDataByScheduleIdandSlotInSubject(int classSubjectId, int slotInSubject)
            {
            APIResponse aPIResponse = new APIResponse();
            try
                {
                var schedules = await _scheduleRepository.GetScheduleDataByScheduleIdandSlotInSubject(classSubjectId, slotInSubject);
                aPIResponse.Result=schedules;
                return aPIResponse;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion
        }
    }
