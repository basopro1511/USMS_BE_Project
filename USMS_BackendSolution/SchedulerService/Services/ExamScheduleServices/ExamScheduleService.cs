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
            _examScheduleRepository=new ExamScheduleRepository();
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

        #region Get AvailableRoom
        /// <summary>
        /// Retrive all Rooms in Database
        /// </summary>
        /// <returns>a list of all Rooms in DB</returns>
        public async Task<APIResponse> GetAvailableRooms(DateOnly date, TimeOnly startTime, TimeOnly endTime)
            {
            APIResponse aPIResponse = new APIResponse();
            var availableRooms = await _examScheduleRepository.GetAvailableRooms(date, startTime, endTime);
            if (availableRooms==null||availableRooms.Count==0)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Don't have any Rooms available for the selected time!";
                }
            else
                {
                aPIResponse.Result=availableRooms;
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
            return null; // Không có lỗi
            }

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

        }
    }
