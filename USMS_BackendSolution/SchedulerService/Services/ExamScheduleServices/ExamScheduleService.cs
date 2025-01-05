using Repositories.RoomRepository;
using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;
using SchedulerService.Repository.ExamScheduleRepository;

namespace SchedulerService.Services.ExamScheduleServices
{
    public class ExamScheduleService
    {
        private readonly IExamScheduleRepository _examScheduleRepository;
        public ExamScheduleService()
        {
            _examScheduleRepository = new ExamScheduleRepository();
        }
        public APIResponse AddNewExamSchedule(ExamScheduleDTO examSchedule)
        {
            APIResponse aPIResponse = new APIResponse();
            var validations = new List<(bool condition, string errorMessage)>
            {
                  (examSchedule.SemesterId.Length > 4, "Mã kì học không thể dài hơn 4 ký tự"),
                  (examSchedule.SubjectId.Length > 10, "Mã môn học không thể dài hơn 10 ký tự"),
                  (examSchedule.RoomId.Length > 6, "Mã phòng học không thể dài hơn 6 ký tự")
            };
            foreach (var validation in validations)
            {
                if (validation.condition)
                {
                    return new APIResponse
                    {
                        IsSuccess = false,
                        Message = validation.errorMessage
                    };
                }
            }
            bool isAdded = _examScheduleRepository.AddNewExamSchedule(examSchedule);

            if (isAdded)
            {
                return new APIResponse
                {
                    IsSuccess = true,
                    Message = "Thêm lịch thi thành công!"
                };
            }
            return new APIResponse
            {
                IsSuccess = false,
                Message = "Thêm lịch thi thất bại !"
            };
        }
    }
}
