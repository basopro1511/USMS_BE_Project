using ISUZU_NEXT.Server.Core.Extentions;
using Repositories.ScheduleRepository;
using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;
using System.Text.Json;

namespace SchedulerDataAccess.Services.SchedulerServices
{
	public class ScheduleService
	{
		private readonly IScheduleRepository _scheduleRepository;
		private readonly HttpClient _httpClient;

		public ScheduleService(HttpClient httpClient)
		{
			_scheduleRepository = new ScheduleRepository();
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
				if (!CheckSlotConflict(classSubjectList, existingSchedules))
				{
					aPIResponse.IsSuccess = false;
					aPIResponse.Message = "Lớp này đã có tiết vào thời gian này!";
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

		/// <summary>
		/// Get ClassSubjectOfClass
		/// </summary>
		/// <returns></returns>
		private async Task<List<ClassSubjectDTO>?> GetClassSubjects()
		{
			// Lấy danh sách ClassSubject từ API
			var response = await _httpClient.GetAsync("https://localhost:7286/api/ClassSubject");
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

		/// <summary>
		/// Check Slot Conflict
		/// </summary>
		/// <param name="classSubjectOfClass"></param>
		/// <param name="existingSchedules"></param>
		/// <returns></returns>
		private bool CheckSlotConflict(List<ClassSubjectDTO> classSubjectOfClass, List<Schedule>? existingSchedules)
		{
			return (existingSchedules != null &&
				existingSchedules.Any(cs =>
										classSubjectOfClass.Any(csc =>
																	csc.ClassSubjectId == cs.ClassSubjectId)));
		}

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
	}
}
