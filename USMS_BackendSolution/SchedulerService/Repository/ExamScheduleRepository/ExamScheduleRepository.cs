using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using SchedulerBusinessObject.AppDBContext;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;

namespace SchedulerService.Repository.ExamScheduleRepository
    {
    public class ExamScheduleRepository : IExamScheduleRepository
        {

        #region GetAllExamSchedule
        /// <summary>
        /// Lấy toàn bộ Exam Schedule
        /// </summary>
        /// <returns>A list of all Class Subject</returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<ExamScheduleDTO>> GetAllExamSchedule()
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    List<ExamSchedule> examSchedules = await dbContext.ExamSchedule.ToListAsync();
                    List<ExamScheduleDTO> examScheduleDTOs = new List<ExamScheduleDTO>();
                    foreach (var examSchedule in examSchedules)
                        {
                        ExamScheduleDTO examScheduleDTO = new ExamScheduleDTO();
                        examScheduleDTO.CopyProperties(examSchedule);
                        examScheduleDTOs.Add(examScheduleDTO);
                        }
                    return examScheduleDTOs;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region GetExamScheduleById
        /// <summary>
        /// Lấy thông tin Exam Schedule theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ExamScheduleDTO> GetExamScheduleById(int id)
            {
            try
                {
                var examSchedules = await GetAllExamSchedule();
                ExamScheduleDTO? examScheduleDTO = examSchedules.FirstOrDefault(x => x.ExamScheduleId==id) ?? new ExamScheduleDTO();
                return examScheduleDTO;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region AssignTeacherToExamSchedule
        /// <summary>
        /// Thêm Teacher vào Exam Schedule
        /// </summary>
        /// <param name="examScheduleId"></param>
        /// <param name="teacherId"></param>
        /// <returns></returns>
        public async Task<bool> AssignTeacherToExamSchedule(int examScheduleId, string teacherId)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existingExamSchedule = await GetExamScheduleById(examScheduleId);
                    if (existingExamSchedule==null)
                        {
                        return false;
                        }

                    // Chuyển DTO -> Entity
                    ExamSchedule examSchedule = new ExamSchedule();
                    examSchedule.CopyProperties(existingExamSchedule);
                    examSchedule.TeacherId=teacherId;

                    dbContext.Entry(examSchedule).State=EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception)
                {
                return false;
                }
            }
        #endregion

        #region AssignRooomToExamSchedule
        /// <summary>
        /// Thêm Room vào ExamSchedule
        /// </summary>
        /// <param name="examScheduleId"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public async Task<bool> AssignRooomToExamSchedule(int examScheduleId, string roomId)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existingExamSchedule = await GetExamScheduleById(examScheduleId);
                    if (existingExamSchedule==null)
                        {
                        return false;
                        }

                    // Chuyển DTO -> Entity
                    ExamSchedule examSchedule = new ExamSchedule();
                    examSchedule.CopyProperties(existingExamSchedule);
                    examSchedule.RoomId=roomId;

                    dbContext.Entry(examSchedule).State=EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception)
                {
                return false;
                }
            }
        #endregion

        #region GetUnassignedTeacherExamSchedules
        /// <summary>
        /// Lấy danh sách ExamSchedule chưa có Teacher
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<ExamScheduleDTO>> GetUnassignedTeacherExamSchedules()
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var allExamSchedules = await GetAllExamSchedule();
                    var examSchedules = allExamSchedules.Where(x => x.TeacherId==null).ToList();

                    List<ExamScheduleDTO> examScheduleDTOs = new List<ExamScheduleDTO>();
                    foreach (var examSchedule in examSchedules)
                        {
                        ExamScheduleDTO examScheduleDTO = new ExamScheduleDTO();
                        examScheduleDTO.CopyProperties(examSchedule);
                        examScheduleDTOs.Add(examScheduleDTO);
                        }
                    return examScheduleDTOs;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region GetUnassignedRoomExamSchedules
        /// <summary>
        /// Lấy danh sách ExamSchedule chưa có Room
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<ExamScheduleDTO>> GetUnassignedRoomExamSchedules()
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var allExamSchedules = await GetAllExamSchedule();
                    var examSchedules = allExamSchedules.Where(x => x.RoomId==null).ToList();
                    List<ExamScheduleDTO> examScheduleDTOs = new List<ExamScheduleDTO>();
                    foreach (var examSchedule in examSchedules)
                        {
                        ExamScheduleDTO examScheduleDTO = new ExamScheduleDTO();
                        examScheduleDTO.CopyProperties(examSchedule);
                        examScheduleDTOs.Add(examScheduleDTO);
                        }
                    return examScheduleDTOs;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region AddNewExamSchedule
        /// <summary>
        /// Thêm mới 1 ExamSchedule
        /// </summary>
        /// <param name="examScheduleDTO"></param>
        /// <returns></returns>
        public async Task<bool> AddNewExamSchedule(ExamScheduleDTO examScheduleDTO)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var examSchedule = new ExamSchedule();
                    examSchedule.CopyProperties(examScheduleDTO);
                    examSchedule.CreatedAt=DateTime.Now;
                    await dbContext.ExamSchedule.AddAsync(examSchedule);
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception)
                {
                return false;
                }
            }
        #endregion

        #region Update Exam Schedule
        /// <summary>
        /// Update Exam Schedule
        /// </summary>
        /// <param name="examScheduleDTO"></param>
        /// <returns></returns>
        public async Task<bool> UpdateExamSchedule(ExamScheduleDTO examScheduleDTO)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var examschedule = dbContext.ExamSchedule.FirstOrDefault(x => x.ExamScheduleId==examScheduleDTO.ExamScheduleId);
                    if (examschedule==null)
                        return false;
                    examschedule.CopyProperties(examScheduleDTO);
                    dbContext.Entry(examschedule).State=EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception)
                {
                return false;
                }
            }
        #endregion

        #region ChangeExamScheduleStatus
        /// <summary>
        /// Thay đổi trạng thái của Exam Schedule
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newStatus"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> ChangeExamScheduleStatus(int id, int newStatus)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existingExamSchedule = await GetExamScheduleById(id);
                    if (existingExamSchedule==null)
                        {
                        return false;
                        }

                    // Chuyển DTO -> Entity
                    ExamSchedule examSchedule = new ExamSchedule();
                    examSchedule.CopyProperties(existingExamSchedule);
                    examSchedule.Status=newStatus;

                    dbContext.Entry(examSchedule).State=EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message, ex);
                }
            }
        #endregion

        /*
        #region GetAvailableTeachers
        /// <summary>
        /// Lấy danh sách Teacher còn trống trong khoảng thời gian
        /// </summary>
        /// <param name="date"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public async Task<List<Teacher>> GetAvailableTeachers(DateOnly date, TimeOnly startTime, TimeOnly endTime)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var teachers = await dbContext.Teacher
                        .Where(t => !dbContext.ExamSchedule.Any(es => es.TeacherId == t.TeacherId &&
                                                                      es.Date == date &&
                                                                      es.StartTime < endTime &&
                                                                      es.EndTime > startTime))
                        .ToListAsync();
                    return teachers;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        */
        #region Auto ADDEXAM
        public List<ExamScheduleDTO> GenerateAutoExamSchedules(DateOnly date, TimeOnly startTime, string semesterId, string majorId, int type)
            {
            List<ExamScheduleDTO> examSchedules = new List<ExamScheduleDTO>();
            List<string> rooms = new List<string> { "G301", "G302", "G303", "G304", "G305" };
            List<string> subjects = new List<string> { "PRN231", "PRM392", "MLN111" };
            List<string> teachers = new List<string> { "HieuNT", "DaQL" };

            // Thêm số lượng học sinh cho từng môn học
            Dictionary<string, int> subjectStudentCounts = new Dictionary<string, int>
    {
        { "PRN231", 60 },
        { "PRM392", 80 },
        { "MLN111", 30 }
    };

            int roomCapacity = 20; // Sức chứa mỗi phòng thi
            Random random = new Random();
            int turn = 1; // Thi lần đầu

            foreach (var subject in subjects)
                {
                int totalStudents = subjectStudentCounts[subject];
                int requiredRooms = (int)Math.Ceiling((double)totalStudents/roomCapacity); // Số phòng cần thiết

                // Lưu lại thời gian bắt đầu gốc cho môn học
                TimeOnly subjectStartTime = startTime;

                for (int i = 0; i<requiredRooms; i++)
                    {
                    if (i>=rooms.Count) break; // Nếu hết phòng thì dừng

                    string room = rooms[i];
                    string teacher = teachers[random.Next(teachers.Count)];
                    int duration = type==2 ? 80 : 120; // 80 phút cho PE, 120 phút cho FE  1 la FE 2 la PE

                    // Tạo lịch thi
                    examSchedules.Add(new ExamScheduleDTO
                        {
                        SemesterId=semesterId,
                        MajorId=majorId,
                        SubjectId=subject,
                        RoomId=room,
                        Type=type,
                        Turn=turn,
                        Date=date,
                        StartTime=subjectStartTime, // Dùng thời gian bắt đầu gốc
                        EndTime=subjectStartTime.AddMinutes(duration),
                        TeacherId=teacher,
                        Status=1,
                        CreatedAt=DateTime.Now
                        });
                    }

                // Sau khi xử lý tất cả các phòng cho môn học này, cập nhật thời gian bắt đầu cho môn học tiếp theo
                startTime=subjectStartTime.AddMinutes((type==2 ? 80 : 120)+30); // Thời gian thi + nghỉ giữa ca
                if (startTime.Hour>=18) // Nếu vượt qua 18:00, chuyển sang ngày mới
                    {
                    startTime=new TimeOnly(7, 0, 0); // Reset giờ bắt đầu
                    date=date.AddDays(1); // Sang ngày mới
                    }
                }

            return examSchedules;
            }
        #endregion

        #region Get Teacher in Exam Schedule by Date & Time
        /// <summary>
        ///   Get Teacher in Exam Schedule by Date & Time
        /// </summary>
        /// <param name="date"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<ExamScheduleDTO>> GetTeacherInExamSchedule(DateOnly date, TimeOnly startTime, TimeOnly endTime)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    List<ExamSchedule>? exams = await dbContext.ExamSchedule.Where(r => r.Date==date && r.StartTime==startTime && r.EndTime==endTime).ToListAsync();
                    List<ExamScheduleDTO> examScheduleDTOs = new List<ExamScheduleDTO>();
                    foreach (var item in exams)
                        {
                        ExamScheduleDTO examScheduleDTO = new ExamScheduleDTO();
                        examScheduleDTO.CopyProperties(item);
                        examScheduleDTOs.Add(examScheduleDTO);
                        }
                    return examScheduleDTOs;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        }
    }
