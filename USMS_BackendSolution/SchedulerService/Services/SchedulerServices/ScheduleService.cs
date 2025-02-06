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
		public ScheduleService(HttpClient httpClient)
		{
			_scheduleRepository = new ScheduleRepository();
			_roomService = new RoomService();
			_httpClient = httpClient;
		}
		#region Get All Schedule
		public APIResponse GetAllSchedule()
		{
			APIResponse aPIResponse = new APIResponse();
			var schedule = _scheduleRepository.getAllSchedule();
			if (schedule == null)
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "Don't have any schedule available!";
			}
			aPIResponse.Result = schedule;
			return aPIResponse;
		}
		#endregion

		#region Post Schedule
		/// <summary>
		/// Post Schedule
		/// </summary>
		/// <param name="schedule"></param>
		/// <returns></returns>
		public async Task<APIResponse> PostSchedule(ClassScheduleDTO schedule)
		{
			APIResponse aPIResponse = new APIResponse();
			try
			{
				var classSubjectList = await GetClassSubjects();
				if (classSubjectList == null)
				{
					aPIResponse.IsSuccess = false;
					aPIResponse.Message = "Không tìm thấy bất kỳ lớp nào!";
					return aPIResponse;
				}
				// Dựa theo ClassSubjectId tìm classSubject
				var classSubject = classSubjectList.FirstOrDefault(cs => cs.ClassSubjectId == schedule.ClassSubjectId);
				if (classSubject == null)
				{
					aPIResponse.IsSuccess = false;
					aPIResponse.Message = "Không tìm thấy lớp này!";
					return aPIResponse;
				}
				// Này lấy hết tất cả lớp của SE1702
				var classSubjectOfClass = classSubjectList.Where(x => x.ClassId == classSubject.ClassId).ToList();
				// Này lấy tất cả thời khóa biểu của ngày hôm đó ngay slot đó
				var existingSchedules = await _scheduleRepository.GetSchedulesByDateAndSlot(schedule.Date, schedule.SlotId);
				// Kiểm tra xung đột lịch học
				if (CheckSlotConflict(classSubjectOfClass, existingSchedules))
				{
					aPIResponse.IsSuccess = false;
					aPIResponse.Message = "Lớp này đã có tiết vào thời gian này!";
					return aPIResponse;
				}
				// Kiểm tra tính hợp lệ của phòng học
				var room = _roomService.GetRoomById(schedule.RoomId);
				if (!room.IsSuccess)
				{
					aPIResponse.IsSuccess = false;
					aPIResponse.Message = "Không tìm thấy phòng học!";
					return aPIResponse;
				}
				// Kiểm tra xung đột phòng học
				if (CheckRoomConflict(existingSchedules, schedule.RoomId))
				{
					aPIResponse.IsSuccess = false;
					aPIResponse.Message = "Phòng đã được sử dụng vào thời gian này!";
					return aPIResponse;
				}
				// Không có gì thay đổi thì thêm lịch mới
				var newSchedule = new Schedule();
				newSchedule.CopyProperties(schedule);
				newSchedule.Status = 1;
				await _scheduleRepository.AddSchedule(newSchedule);
				aPIResponse.IsSuccess = true;
				aPIResponse.Message = "Thêm thời khóa biểu thành công!";
			}
			catch (Exception ex)
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = ex.Message;
			}
			return aPIResponse;
		}
        #endregion

        #region Get Class Subject
        /// <summary>
        /// Get ClassSubjectOfClass
        /// </summary>
        /// <returns></returns>
        private async Task<List<ClassSubjectDTO>?> GetClassSubjects()
		{
			// Lấy danh sách ClassSubject từ API
			var response = await _httpClient.GetAsync("https://localhost:7067/api/ClassSubject");
			var apiResponse = await response.Content.ReadFromJsonAsync<APIResponse>();
			if (apiResponse == null || !apiResponse.IsSuccess)
			{
				return null;
			}
			//Nhận dạng là một Json
			var classSubjectResponse = apiResponse.Result as JsonElement?;
			if (classSubjectResponse == null)
			{
				return null;
			}

			var options = new JsonSerializerOptions
			{
				PropertyNameCaseInsensitive = true
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
        private bool CheckSlotConflict(List<ClassSubjectDTO> classSubjectOfClass, List<Schedule>? existingSchedules)
		{
			if (existingSchedules == null || existingSchedules.Count == 0)
			{
				return false;
			}
			return existingSchedules.Any(cs =>
											classSubjectOfClass.Any(csc =>
																		csc.ClassSubjectId == cs.ClassSubjectId));
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
			return (existingSchedules != null &&
				existingSchedules.Any(es =>
										es.RoomId == roomId));
		}
        #endregion

        #region Get Class SubjectIds by Student Id
		private List<int> getClassSubjectIdsByStudentIds(string id)
			{
			try
				{
                var response = _httpClient.GetAsync($"https://localhost:7067/api/StudentInClass/ClassSubject/{id}").Result;
                var apiResponse = response.Content.ReadFromJsonAsync<APIResponse>().Result;
                if(apiResponse == null || !apiResponse.IsSuccess)
                    {
                    return null;
                    }
                var dataResponse = apiResponse.Result as JsonElement?;
                if(dataResponse == null)
                    {
                    return null;
                    }
                var options = new JsonSerializerOptions
                    {
                    PropertyNameCaseInsensitive = true
                    };
                return dataResponse.Value.Deserialize<List<int>>(options);
                }
            catch(Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Class SubjectIds by MajorId, ClassId, SubjectId
        private List<ClassSubjectDTO> getClassSubjectIdsByMajorIdClassIdSubjectId(string majorId, string classId, int term)
            {
            try
                {
                var response = _httpClient.GetAsync($"https://localhost:7286/api/ClassSubject/ClassSubject?majorId={majorId}&classId={classId}&term={term}").Result;
                var apiResponse = response.Content.ReadFromJsonAsync<APIResponse>().Result;
                if(apiResponse == null || !apiResponse.IsSuccess)
                    {
                    return null;
                    }
                var dataResponse = apiResponse.Result as JsonElement?;
                if(dataResponse == null)
                    {
                    return null;
                    }
                var options = new JsonSerializerOptions
                    {
                    PropertyNameCaseInsensitive = true
                    };
                return dataResponse.Value.Deserialize<List<ClassSubjectDTO>>(options);
                }
            catch(Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Schedule for Student
        public APIResponse GetClassSchedulesByStudentIds(string studentId)
            {
            APIResponse aPIResponse = new APIResponse();
			var classSubjectIds = getClassSubjectIdsByStudentIds(studentId);
            aPIResponse.Result = _scheduleRepository.GetClassSchedulesByClassSubjectIds(classSubjectIds);
            return aPIResponse;
            }
        #endregion

        #region Get Schedule for Class
        public APIResponse GetClassSchedulesForClass(string majorId, string classId, int term, DateTime startDay, DateTime endDay)
            {
            APIResponse aPIResponse = new APIResponse();
            try
                {
				var classSubjects = getClassSubjectIdsByMajorIdClassIdSubjectId(majorId, classId, term);
					if(classSubjects == null)
                    {
                    aPIResponse.IsSuccess = false;
                    aPIResponse.Message = "Không tìm thấy bất kỳ lớp nào!";
                    return aPIResponse;
                    };
				List<int> classSubjectIds = classSubjects.Select(s => s.ClassSubjectId).ToList();
        Dictionary<int, string> subjectMap = classSubjects.ToDictionary(s => s.ClassSubjectId, s => s.SubjectId);
                using(var _dbContext = new MyDbContext())
                    {
                    var schedules = _dbContext.Schedule
                        .Where(s => classSubjectIds.Contains(s.ClassSubjectId)
                                 && s.Date >= DateOnly.FromDateTime(startDay)
                                 && s.Date <= DateOnly.FromDateTime(endDay))
                        .Select(s => new ViewScheduleDTO
                            {
                            ClassScheduleId = s.ScheduleId,
                            ClassSubjectId = s.ClassSubjectId,
							ClassId = classId,
							MajorId = majorId,
                            SubjectId = subjectMap.ContainsKey(s.ClassSubjectId) ? subjectMap[s.ClassSubjectId] : null, // Lấy SubjectId theo ClassSubjectId
                            SlotId = s.SlotId,
                            TeacherId = s.TeacherId,
                            Date = s.Date,
                            Status = s.Status,
                            })
                        .ToList();
					aPIResponse.Result = schedules;

                    }
                return aPIResponse;
                }
            catch(Exception ex)
                {
                throw new Exception("An error occurred while fetching class schedules.", ex);
                }
			}
        #endregion

        }
    }
