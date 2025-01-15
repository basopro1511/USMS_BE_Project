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
		public async Task<APIResponse> PostSchedule(ClassScheduleDTO schedule)
		{
			APIResponse aPIResponse = new APIResponse();

			try
			{
				// Lấy danh sách ClassSubject từ API
				var response = await _httpClient.GetAsync("https://localhost:7286/api/ClassSubject");
				var apiResponse = await response.Content.ReadFromJsonAsync<APIResponse>();
				if (apiResponse == null || !apiResponse.IsSuccess)
				{
					aPIResponse.IsSuccess = false;
					aPIResponse.Message = $"Failed to fetch ClassSubject. Status code: {response.StatusCode}";
					return aPIResponse;
				}
				//Nhận dạng là một Json
				var classSubjectResponse = apiResponse.Result as JsonElement?;
				if (classSubjectResponse == null)
				{
					aPIResponse.IsSuccess = false;
					aPIResponse.Message = "Invalid ClassSubject data.";
					return aPIResponse;
				}

				var options = new JsonSerializerOptions
				{
					PropertyNameCaseInsensitive = true
				};
				var classSubjectList = classSubjectResponse.Value.Deserialize<List<ClassSubjectDTO>>(options);
				if (classSubjectList == null)
				{
					aPIResponse.IsSuccess = false;
					aPIResponse.Message = "No ClassSubject found!";
					return aPIResponse;
				}
				// Dựa theo ClassSubjectId tìm classSubject
				var classSubject = classSubjectList.FirstOrDefault(cs => cs.ClassSubjectId == schedule.ClassSubjectId);
				if (classSubject == null)
				{
					aPIResponse.IsSuccess = false;
					aPIResponse.Message = "ClassSubject not found!";
					return aPIResponse;
				}

				// Này lấy hết tất cả lớp của SE1702
				var classSubjectOfClass = classSubjectList.Where(x => x.ClassId == classSubject.ClassId).ToList();
				// Này lấy tất cả thời khóa biểu của ngày hôm đó ngay slot đó
				var existingSchedules = await _scheduleRepository.GetSchedulesByDateAndSlot(schedule.Date, schedule.SlotId);

				// Kiểm tra xung đột lịch học
				if (existingSchedules != null && existingSchedules.Any(cs => classSubjectOfClass.Any(csc => csc.ClassSubjectId == cs.ClassSubjectId)))
				{
					aPIResponse.IsSuccess = false;
					aPIResponse.Message = "Slot conflict!";
					return aPIResponse;
				}

				// Kiểm tra xung đột phòng học
				if (existingSchedules != null && existingSchedules.Any(es => es.RoomId == schedule.RoomId))
				{
					aPIResponse.IsSuccess = false;
					aPIResponse.Message = "Room is occupied!";
					return aPIResponse;
				}

				// Không có gì thay đổi thì thêm lịch mới
				var newSchedule = new Schedule();
				newSchedule.CopyProperties(schedule);
				newSchedule.Status = 1;

				await _scheduleRepository.AddSchedule(newSchedule);
				aPIResponse.IsSuccess = true;
				aPIResponse.Message = "Schedule added successfully!";
			}
			catch (Exception ex)
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = ex.Message;
			}

			return aPIResponse;
		}
		#endregion
	}
}
