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
        #region Get All ExamSchedule
        /// <summary>
        /// Retrive all Rooms in Database
        /// </summary>
        /// <returns>a list of all Rooms in DB</returns>
        public APIResponse GetAllExamSchedules()
        {
            APIResponse aPIResponse = new APIResponse();
            List<ExamScheduleDTO> examSchedules = _examScheduleRepository.GetAllExamSchedule();
            if (examSchedules == null)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Don't have any Exam Schedules available!";
            }
            aPIResponse.Result = examSchedules;
            return aPIResponse;
        }
        #endregion

        #region Get Unassigned Room ExamSchedule
        /// <summary>
        /// Retrive all Rooms in Database
        /// </summary>
        /// <returns>a list of all Rooms in DB</returns>
        public APIResponse GetUnassignedRoomExamSchedules()
        {
            APIResponse aPIResponse = new APIResponse();
            List<ExamScheduleDTO> examSchedules = _examScheduleRepository.GetUnassignedRoomExamSchedules();
            if (examSchedules == null)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Don't have any Exam Schedules available!";
            }
            aPIResponse.Result = examSchedules;
            return aPIResponse;
        }
        #endregion

        #region Get Unassigned Room ExamSchedule
        /// <summary>
        /// Retrive all Rooms in Database
        /// </summary>
        /// <returns>a list of all Rooms in DB</returns>
        public APIResponse GetUnassignedTeacherExamSchedules()
        {
            APIResponse aPIResponse = new APIResponse();
            List<ExamScheduleDTO> examSchedules = _examScheduleRepository.GetUnassignedTeacherExamSchedules();
            if (examSchedules == null)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Don't have any Exam Schedules available!";
            }
            aPIResponse.Result = examSchedules;
            return aPIResponse;
        }
        #endregion
        #region Get Unassigned Room ExamSchedule
        /// <summary>
        /// Retrive all Rooms in Database
        /// </summary>
        /// <returns>a list of all Rooms in DB</returns>
        public APIResponse GetAvailableRooms(DateOnly date, TimeOnly startTime, TimeOnly endTime)
        {
            APIResponse aPIResponse = new APIResponse();
            List<Room> examSchedules = _examScheduleRepository.GetAvailableRooms(date,startTime,endTime);
            if (examSchedules == null)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Don't have any Exam Schedules available!";
            }
            aPIResponse.Result = examSchedules;
            return aPIResponse;
        }
        #endregion


        public APIResponse AddNewExamSchedule(ExamScheduleDTO examSchedule)
        {
            APIResponse aPIResponse = new APIResponse();
            var validations = new List<(bool condition, string errorMessage)>
            {
                  (examSchedule.SemesterId.Length > 4, "Mã kì học không thể dài hơn 4 ký tự"),
                  (examSchedule.SubjectId.Length > 10, "Mã môn học không thể dài hơn 10 ký tự"),
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
