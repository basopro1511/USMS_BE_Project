using Repositories.RoomRepository;
using Repositories.ScheduleRepository;
using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;
using SchedulerService.Repository.ExamScheduleRepository;
using Services.RoomServices;
using System.Net.Http;
using System.Text.Json;

namespace SchedulerService.Services.ExamScheduleServices
    {
    public class ExamScheduleService
        {
        private readonly IExamScheduleRepository _examScheduleRepository;
        private readonly HttpClient _httpClient;
        public ExamScheduleService(HttpClient httpClient)
            {
            _examScheduleRepository= new ExamScheduleRepository();
            _httpClient=httpClient;
            }

        #region Get All ExamSchedule
        /// <summary>
        /// Retrive all exam schedule in Database
        /// </summary>
        /// <returns>a list of exam schedule  Rooms in DB</returns>
        public async Task<APIResponse> GetAllExamSchedules()
            {
            APIResponse aPIResponse = new APIResponse();
            List<ExamScheduleDTO> examSchedules = await _examScheduleRepository.GetAllExamSchedule();
            if (examSchedules==null)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không tìm thấy lịch thi khả dụng!";
                }
            aPIResponse.Result=examSchedules;
            return aPIResponse;
            }
        #endregion

        #region Get Unassigned Room ExamSchedule
        /// <summary>
        /// Retrive all Rooms in Database
        /// </summary>
        /// <returns>a list of all Rooms in DB</returns>
        public async Task<APIResponse> GetUnassignedRoomExamSchedules()
            {
            APIResponse aPIResponse = new APIResponse();
            var examSchedules = await _examScheduleRepository.GetUnassignedRoomExamSchedules();
            if (examSchedules==null||examSchedules.Count==0)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Don't have any Exam Schedules available!";
                }
            else
                {
                aPIResponse.Result=examSchedules;
                }
            return aPIResponse;
            }
        #endregion

        #region Get Unassigned Teacher ExamSchedule
        /// <summary>
        /// Retrive all Teacher in Database
        /// </summary>
        /// <returns>a list of all Teacher in DB</returns>
        public async Task<APIResponse> GetUnassignedTeacherExamSchedules()
            {
            APIResponse aPIResponse = new APIResponse();
            var examSchedules = await _examScheduleRepository.GetUnassignedTeacherExamSchedules();
            if (examSchedules==null||examSchedules.Count==0)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Don't have any Exam Schedules available!";
                }
            else
                {
                aPIResponse.Result=examSchedules;
                }
            return aPIResponse;
            }
        #endregion

        #region Add new Exam Schedule
        /// <summary>
        /// Hàm riêng để rút gọn logic kiểm tra điều kiện
        /// </summary>
        /// <param name="examSchedule"></param>
        /// <returns>Trả về chuỗi lỗi nếu có, null nếu hợp lệ</returns>
        private string ValidateExamSchedule(ExamScheduleDTO examSchedule)
            {
            if (examSchedule.SemesterId.Length>4)
                {
                return "Mã kì học không thể dài hơn 4 ký tự";
                }
            if (examSchedule.SubjectId.Length>10)
                {
                return "Mã môn học không thể dài hơn 10 ký tự";
                }
            if (examSchedule.StartTime > examSchedule.EndTime)
                {
                return "Thời gian kết thúc không thể sớm hơn thời gian bắt đầu.";
                }
            if (examSchedule.Date < DateOnly.FromDateTime(DateTime.Now))
                {
                return " Ngày thi không thể là ngày đã xảy ra trong quá khứ.";
                }
            return null; // Không có lỗi
            }
        /// <summary>
        /// Add Exam Schedule mới
        /// </summary>
        /// <param name="examSchedule"></param>
        /// <returns></returns>
        public async Task<APIResponse> AddNewExamSchedule(ExamScheduleDTO examSchedule)
            {
            // Kiểm tra điều kiện đầu vào
            var errorMsg = ValidateExamSchedule(examSchedule);
            if (!string.IsNullOrEmpty(errorMsg))
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message=errorMsg
                    };
                }
            bool isAdded = await _examScheduleRepository.AddNewExamSchedule(examSchedule);
            if (isAdded)
                {
                return new APIResponse
                    {
                    IsSuccess=true,
                    Message="Thêm lịch thi thành công!"
                    };
                }
            return new APIResponse
                {
                IsSuccess=false,
                Message="Thêm lịch thi thất bại !"
                };
            }
        #endregion

        #region Update Exam Schedule
        /// <summary>
        /// Cập nhật lịch thi
        /// </summary>
        /// <param name="examSchedule"></param>
        /// <returns></returns>
        public async Task<APIResponse> UpdateExamSchedule(ExamScheduleDTO examSchedule)
            {
            // Kiểm tra điều kiện đầu vào
            var errorMsg = ValidateExamSchedule(examSchedule);
            if (!string.IsNullOrEmpty(errorMsg))
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message=errorMsg
                    };
                }
            bool isAdded = await _examScheduleRepository.UpdateExamSchedule(examSchedule);
            if (isAdded)
                {
                return new APIResponse
                    {
                    IsSuccess=true,
                    Message="Cập nhật lịch thi thành công!"
                    };
                }
            return new APIResponse
                {
                IsSuccess=false,
                Message="Cập nhật lịch thi thất bại !"
                };
            }
        #endregion


        #region Assign Room into Exam Schedule
        public async Task<APIResponse> AssignRoomToExamSchedule(int id, string roomId)
            {
            APIResponse aPIResponse = new APIResponse();
            bool isUpdated = await _examScheduleRepository.AssignRooomToExamSchedule(id, roomId);
            if (isUpdated)
                {
                aPIResponse.IsSuccess=true;
                aPIResponse.Message="Thêm Phòng vào lịch thi thành công!";
                }
            else
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Thêm Phòng vào lịch thi thất bại !";
                }
            return aPIResponse;
            }
        #endregion

        #region Assign Teacher into Exam Schedule
        public async Task<APIResponse> AssignTeacherToExamSchedule(int id, string teacherId)
            {
            APIResponse aPIResponse = new APIResponse();
            bool isUpdated = await _examScheduleRepository.AssignTeacherToExamSchedule(id, teacherId);
            if (isUpdated)
                {
                aPIResponse.IsSuccess=true;
                aPIResponse.Message="Thêm giáo viên vào lịch thi thành công!";
                }
            else
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Thêm giáo viên vào lịch thi thất bại !";
                }
            return aPIResponse;
            }
        #endregion

        #region Change Exam Schedule Status
        public async Task<APIResponse> ChangeExamScheduleStatus(int id, int newStatus)
            {
            APIResponse aPIResponse = new APIResponse();
            // Kiểm tra xem lịch thi có tồn tại không
            var existingExam = await _examScheduleRepository.GetExamScheduleById(id);
            if (existingExam==null)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Mã lịch thi được cung cấp không tồn tại.";
                return aPIResponse;
                }
            // Thay đổi trạng thái
            bool isSuccess = await _examScheduleRepository.ChangeExamScheduleStatus(id, newStatus);
            if (isSuccess)
                {
                // Tùy theo giá trị trạng thái mà trả về message khác nhau
                switch (newStatus)
                    {
                    case 0:
                        aPIResponse.IsSuccess=true;
                        aPIResponse.Message=$"Lịch thi với mã: {id} đã được chuyển về trạng thái chưa bắt đầu.";
                        break;
                    case 1:
                        aPIResponse.IsSuccess=true;
                        aPIResponse.Message=$"Lịch thi với mã: {id} đã được chuyển về trạng thái đang diễn ra.";
                        break;
                    case 2:
                        aPIResponse.IsSuccess=true;
                        aPIResponse.Message=$"Lịch thi với mã: {id} đã được chuyển về trạng thái đã hoàn thành.";
                        break;
                    default:
                        aPIResponse.IsSuccess=true;
                        aPIResponse.Message=$"Lịch thi với mã: {id} đã chuyển sang trạng thái: {newStatus}.";
                        break;
                    }
                }
            else
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Thay đổi trạng thái lịch thi thất bại.";
                }
            return aPIResponse;
            }
        #endregion

        // #region AUTO EXAM SCHEDULE
        // public List<ExamScheduleDTO> GenerateExamSchedule(
        //string majorId,
        //List<string> subjectIds,
        //List<string> availableRooms,
        //List<string> availableTeachers,
        //DateOnly startDate,
        //TimeOnly startTime,
        //TimeOnly endTime,
        //int examType,
        //int turnType,
        //List<StudentDTO> students,
        //int breakTime = 15)
        //     {
        //     var examSchedules = new List<ExamScheduleDTO>();

        //     // Xác định thời lượng mỗi ca thi (phút) tùy theo loại thi
        //     int examDuration = (examType==2) ? 120 : 90; // ví dụ: thi thực hành 120', lý thuyết 90'
        //     DateOnly currentDate = startDate;
        //     TimeOnly currentTime = startTime;
        //     int teacherIndex = 0;

        //     foreach (string subjectId in subjectIds)
        //         {
        //         // 1. Lấy danh sách sinh viên cho môn hiện tại
        //         var studentsForSubject = students
        //             .Where(s => s.SubjectId==subjectId||s.ClassSubjectId==subjectId)
        //             .ToList();
        //         int totalStudents = studentsForSubject.Count;
        //         if (totalStudents==0) continue;  // nếu không có sinh viên thì bỏ qua

        //         // 2. Chia sinh viên thành các nhóm tối đa 20 người
        //         List<List<StudentDTO>> groups = new List<List<StudentDTO>>();
        //         for (int i = 0; i<totalStudents; i+=20)
        //             {
        //             var group = studentsForSubject.Skip(i).Take(20).ToList();
        //             groups.Add(group);
        //             }

        //         // 3, 5, 6. Xếp các nhóm này vào các ca thi, phòng thi theo thời gian
        //         int groupIndex = 0;
        //         while (groupIndex<groups.Count)
        //             {
        //             // Nếu không đủ thời gian trong ngày cho ca thi mới, chuyển sang ngày kế tiếp
        //             if (currentTime.AddMinutes(examDuration)>endTime)
        //                 {
        //                 currentDate=currentDate.AddDays(1);
        //                 currentTime=startTime;
        //                 }

        //             // 3. Gán nhóm sinh viên vào các phòng trong cùng một ca (đồng thời)
        //             for (int roomIndex = 0; roomIndex<availableRooms.Count&&groupIndex<groups.Count; roomIndex++)
        //                 {
        //                 // Lấy nhóm sinh viên kế tiếp và thông tin phòng, giáo viên
        //                 var studentGroup = groups[groupIndex++];
        //                 string room = availableRooms[roomIndex];
        //                 string teacher = availableTeachers[teacherIndex];
        //                 teacherIndex=(teacherIndex+1)%availableTeachers.Count;  // xoay vòng giáo viên

        //                 // 7. Tạo đối tượng lịch thi và thêm vào danh sách kết quả
        //                 var schedule = new ExamScheduleDTO
        //                     {
        //                     MajorId=majorId,
        //                     SubjectId=subjectId,
        //                     Date=currentDate,
        //                     StartTime=currentTime,
        //                     RoomId=room,
        //                     TeacherId=teacher,
        //                     Type=examType,
        //                     Turn=turnType,
        //                     // Danh sách sinh viên hoặc mã sinh viên có thể được lưu nếu cần:
        //                     };
        //                 examSchedules.Add(schedule);

        //                 // Lưu ý: Có thể lưu vào DB tại đây (ExamSchedule, StudentInExamSchedule) nếu cần
        //                 }

        //             // 5. Sau khi xếp xong một ca (đã gán phòng cho một nhóm hoặc nhiều nhóm đồng thời),
        //             // tiến tới khung giờ tiếp theo trong ngày, cộng thêm thời lượng làm bài + thời gian nghỉ.
        //             currentTime=currentTime.AddMinutes(examDuration+breakTime);
        //             } // tiếp tục while cho đến khi hết nhóm sinh viên của môn
        //         } // tiếp tục foreach cho các môn tiếp theo

        //     return examSchedules;
        //     }

        // #endregion

        #region Get all Teacher for Add ExamSchedule
        private async Task<List<TeacherDTO>> GetAvailableTeachers()
        {
            try
                {
                var response = _httpClient.GetAsync($"https://localhost:7067/api/Teacher").Result;
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

        #region Get All Teachers Available for Add Exam Schedule
        /// <summary>
        /// Get All Teacher Available from Database
        /// </summary>
        /// <param name="teacherDto"></param>
        /// <returns></returns>
        public async Task<APIResponse> GetAllTeacherAvailableForAddExamSchedule(DateOnly date,TimeOnly startTime, TimeOnly endTime)
            {
            APIResponse aPIResponse = new APIResponse();
            List<TeacherDTO> teachers = await GetAvailableTeachers();
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
            // 2. Lấy các exam schedule có date & time
            var examSchedules = await _examScheduleRepository.GetTeacherInExamSchedule(date, startTime, endTime);
            // 3. Lấy danh sách teacherId đã bị chiếm
            var usedTeacherIds = examSchedules.Select(sch => sch.TeacherId).Distinct().ToHashSet();
            // 4. Lọc ra các giáo viên còn trống
            var availableTeacher = teachers
                .Where(r => !usedTeacherIds.Contains(r.UserId))
                .ToList();
            aPIResponse.IsSuccess=true;
            aPIResponse.Result=availableTeacher;
            return aPIResponse;
            }
        #endregion    

        #region AUTO EXAM SCHEDULE 2
        /// <summary>
        /// Hàm sinh tự động các lịch thi dựa trên thông tin đầu vào.
        /// Tham số:
        /// - date: Ngày bắt đầu (DateOnly)
        /// - startTime: Giờ bắt đầu buổi thi (TimeOnly)
        /// - semesterId: Mã học kỳ
        /// - majorId: Mã chuyên ngành
        /// - type: "PE" hay "FE" tuỳ bạn quy ước (PE = 80 phút, FE = 120 phút)
        /// 
        /// Logic:
        /// - Sử dụng danh sách room, subject, teacher tạm thời (hoặc lấy từ DB).
        /// - Chia phòng mỗi phòng tối đa 20 thí sinh.
        /// - Mỗi môn tính số nhóm phòng = ceil(tổng sinh viên / 20).
        /// - Tạo các ExamScheduleDTO và trả về.
        /// - Nếu cần lưu DB, ta gọi repository để lưu.
        /// </summary>
        public async Task<List<ExamScheduleDTO>> GenerateAutoExamSchedules(
                DateOnly date,
                TimeOnly startTime,
                string semesterId,
                string majorId,
                string type)
                {
                // Chuẩn bị kết quả
                List<ExamScheduleDTO> examSchedules = new List<ExamScheduleDTO>();
                // Giả lập danh sách phòng
                List<string> rooms = new List<string> { "G301", "G302", "G303", "G304", "G305" };
                // Giả lập danh sách môn
                List<string> subjects = new List<string> { "PRN231", "PRM392", "MLN111" };
                // Giả lập danh sách giáo viên
                List<string> teachers = new List<string> { "HieuNT", "DaQL" };

                // Giả lập số lượng sinh viên cho từng môn
                Dictionary<string, int> subjectStudentCounts = new Dictionary<string, int>
                  {
                { "PRN231", 60 },
                { "PRM392", 80 },
                { "MLN111", 30 }
                    };

                int roomCapacity = 20; // Sức chứa tối đa mỗi phòng
                Random random = new Random();
                int turn = 1; // Thi lần đầu
                int breakTime = 30; // Thời gian nghỉ giữa ca (phút)

                foreach (var subject in subjects)
                    {
                    // Tổng số sinh viên cần thi môn này
                    int totalStudents = subjectStudentCounts[subject];
                    // Tính số phòng cần thiết
                    int requiredRooms = (int)Math.Ceiling((double)totalStudents/roomCapacity);

                    // Lưu thời gian bắt đầu gốc cho môn học
                    TimeOnly subjectStartTime = startTime;

                    for (int i = 0; i<requiredRooms; i++)
                        {
                        if (i>=rooms.Count) break; // Nếu vượt quá số phòng, dừng (hoặc chuyển sang ngày hôm sau tuỳ logic)
                        string room = rooms[i];
                        string teacher = teachers[random.Next(teachers.Count)];
                        // 80 phút nếu là PE, 120 phút nếu là FE
                        int duration = (type=="PE") ? 80 : 120;

                        // Tạo lịch thi
                        var examSchedule = new ExamScheduleDTO
                            {
                            SemesterId=semesterId,
                            MajorId=majorId,
                            SubjectId=subject,
                            RoomId=room,
                            Type=(type=="PE") ? 1 : 2, // convert sang int, tuỳ bạn
                            Turn=turn,
                            Date=date,
                            StartTime=subjectStartTime,
                            EndTime=subjectStartTime.AddMinutes(duration),
                            TeacherId=teacher,
                            Status=1,
                            CreatedAt=DateTime.Now
                            };

                        examSchedules.Add(examSchedule);
                        }

                    // Sau khi xếp xong các phòng cho môn này, cập nhật thời gian bắt đầu cho môn tiếp theo
                    startTime=subjectStartTime.AddMinutes(
                        (type=="PE" ? 80 : 120)+breakTime
                    );

                    // Ví dụ: nếu sang 18h => chuyển ngày
                    if (startTime.Hour>=18)
                        {
                        startTime=new TimeOnly(7, 0, 0);
                        date=date.AddDays(1);
                        }
                    }

                // Nếu bạn muốn lưu luôn kết quả vào DB (bảng ExamSchedule),
                // thì có thể gọi repository tại đây. Ví dụ:
                // await _examScheduleRepository.AddRangeAsync(examSchedules);
                // await _examScheduleRepository.SaveChangesAsync();

                return examSchedules;
                }
        #endregion

        }
    }


