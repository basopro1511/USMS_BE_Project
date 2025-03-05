using SchedulerBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;
using SchedulerService.Repository.SlotRepository;
using SchedulerService.Repository.StudentInExamScheduleRepository;
using System.Net.Http;
using System.Text.Json;

namespace SchedulerService.Services.StudentInExamScheduleServices
    {
    public class StudentInExamScheduleService
        {
        private readonly IStudentInExamScheduleRepository _repository;
        private readonly HttpClient _httpClient;
        public StudentInExamScheduleService(HttpClient httpClient)
            {
            _httpClient=httpClient;
            _repository=new StudentInExamScheduleRepository();
            }
        public StudentInExamScheduleService() { }
        #region Get All Student in Exam Schedule by ExamScheduleID
        /// <summary>
        ///  Get All Student In Exam Schedule by ExamScheduleID
        /// </summary>
        /// <param name="examScheduleId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<APIResponse> GetAllStudentInExamScheduleByExamScheduleId(int examScheduleId)
            {
            try
                {
                APIResponse aPIResponse = new APIResponse();
                List<StudentInExamScheduleDTO> studentInExamSchedules = await _repository.GetAllStudentInExamScheduleByExamScheduleId(examScheduleId);
                List<StudentDTO> studentDTOs = new List<StudentDTO>();
                if (studentInExamSchedules==null)
                    {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Không có sinh viên nào trong phòng thi";
                    return aPIResponse;
                    }
                foreach (var item in studentInExamSchedules)
                    {
                    StudentDTO studentDTO = new StudentDTO();
                    studentDTO=await getStudentData(item.StudentId);
                    if (studentDTO==null)
                        {
                        aPIResponse.IsSuccess=false;
                        aPIResponse.Message="Không tìm thấy học sinh với ID = "+item.StudentId;
                        }
                    studentDTO.ExamScheduleId=examScheduleId;
                    studentDTO.StudentExamId=item.StudentExamId;
                    studentDTOs.Add(studentDTO);

                    }
                aPIResponse.IsSuccess=true;
                aPIResponse.Result=studentDTOs;
                return aPIResponse;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Add Student into Exam Schedule Class
        /// <summary>
        /// Add new Student into Exam Schedule Class
        /// </summary>
        /// <param name="examScheduleDTO"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<APIResponse> AddNewStudentToExamSchedule(StudentInExamScheduleDTO examScheduleDTO)
            {
            try
                {
                APIResponse aPIResponse = new APIResponse();
                var student = await _repository.GetStudentInExamScheduleByExamScheduleIdAndStudentId(examScheduleDTO.ExamScheduleId, examScheduleDTO.StudentId);
                if (student==null)
                    {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Không tìm thấy sinh viên trong phòng.";
                    }
                int numberOfStudent = await _repository.CountStudentInExamSchedule(examScheduleDTO.ExamScheduleId);
                int numberOfStudentAfterAdd = numberOfStudent+1;
                if (numberOfStudentAfterAdd>20)
                    {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Số lượng sinh viên thêm vào đã giới hạn của phòng ( giới hạn 1 phòng có tối đa 20 sinh viên )";
                    return aPIResponse;
                    }
                bool success = await _repository.AddNewStudentToExamSchedule(examScheduleDTO);
                if (success)
                    {
                    return new APIResponse
                        {
                        IsSuccess=true,
                        Message="Thêm sinh viên vào phòng thi thành công"
                        };
                    }
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Thêm sinh viên vào phòng thi thất bại! "
                    };
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Remove Student in Exam Schedule Class
        /// <summary>
        /// Remove student from exam schedule class
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<APIResponse> RemoveStudentInExamScheduleClass(int studentExamId)
            {
            try
                {
                APIResponse aPIResponse = new APIResponse();
                bool success = await _repository.RemoveStudentInExamScheduleClass(studentExamId);
                aPIResponse.IsSuccess=success;
                aPIResponse.Message=success ? "Xóa sinh viên khỏi phòng thi thành công" : "Xóa thất bại";
                return aPIResponse;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Add Mutiple Student into Exam Schedule
        /// <summary>
        /// Add Mutiple Student into Exam Schedule
        /// </summary>
        /// <param name="studentInExamScheduleDTOs"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<APIResponse> AddMultipleStudentsToExamSchedule(List<StudentInExamScheduleDTO> studentInExamScheduleDTOs)
            {
            try
                {
                APIResponse aPIResponse = new APIResponse();
                int examScheduleId = studentInExamScheduleDTOs.First().ExamScheduleId;
                List<StudentInExamScheduleDTO> existingStudents = await _repository.GetAllStudentInExamScheduleByExamScheduleId(examScheduleId);
                var existingStudentIds = new HashSet<string>(existingStudents.Select(s => s.StudentId));
                // Lọc sinh viên chưa có trong lớp
                List<StudentInExamScheduleDTO> newStudents = studentInExamScheduleDTOs
                    .Where(dto => !existingStudentIds.Contains(dto.StudentId))
                    .ToList();
                if (newStudents.Count==0)
                    {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Tất cả sinh viên đã có trong lớp.";
                    return aPIResponse;
                    }
                int numberOfStudent = await _repository.CountStudentInExamSchedule(examScheduleId);
                int numberOfStudentAfterAdd = numberOfStudent+newStudents.Count;
                if (numberOfStudentAfterAdd>40)
                    {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Số lượng sinh viên thêm vào đã giới hạn của lớp ( giới hạn 1 lớp có tối đa 40 sinh viên )";
                    return aPIResponse;
                    }
                bool success = await _repository.AddMultipleStudentsToExamSchedule(newStudents);
                aPIResponse.IsSuccess=success;
                aPIResponse.Message=success ? $"Thêm {newStudents.Count} sinh viên thành công." : "Thêm sinh viên thất bại.";
                aPIResponse.Result=newStudents;
                return aPIResponse;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Student Data by StudentId ( Call Student In Class API )          
        /// <summary>
        /// Call Student API to get StudentData
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<StudentDTO> getStudentData(string studentId)
            {
            try
                {
                var response = await _httpClient.GetAsync($"https://localhost:7067/api/Student/{studentId}");
                if (!response.IsSuccessStatusCode)
                    {
                    return null;
                    }
                var apiResponse = await response.Content.ReadFromJsonAsync<APIResponse>();
                if (apiResponse==null||!apiResponse.IsSuccess)
                    {
                    return null;
                    }
                if (apiResponse.Result is not JsonElement dataResponse)
                    {
                    return null;
                    }
                var options = new JsonSerializerOptions
                    {
                    PropertyNameCaseInsensitive=true
                    };
                var student = dataResponse.Deserialize<StudentDTO>(options);
                return student;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get All Student from Student API        
        /// <summary>
        /// Call Student API to get StudentData
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<List<StudentDTO>> getAllStudent()
            {
            try
                {
                var response = await _httpClient.GetAsync($"https://localhost:7067/api/Student");
                var apiResponse = await response.Content.ReadFromJsonAsync<APIResponse>();
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
                return dataResponse.Value.Deserialize<List<StudentDTO>>(options);
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Available Students (Students Not in Class)
        /// <summary>
        /// Get all students who are NOT in the given class
        /// </summary>
        /// <param name="classId">Class ID</param>
        /// <returns>List of students not in the class</returns>
        public async Task<APIResponse> GetAvailableStudentsForExamSchedule(int examScheduleId)
            {
            APIResponse aPIResponse = new APIResponse();
            // Lấy danh sách tất cả sinh viên
            List<StudentDTO> allStudents = await getAllStudent();
            // Lấy danh sách sinh viên đã có trong lớp
            List<StudentInExamScheduleDTO> studentsInClass = await _repository.GetAllStudentInExamScheduleByExamScheduleId(examScheduleId);
            if (allStudents==null||allStudents.Count==0)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không tìm thấy sinh viên nào.";
                return aPIResponse;
                }
            if (studentsInClass==null)
                {
                studentsInClass=new List<StudentInExamScheduleDTO>();
                }
            // Loại bỏ những sinh viên đã có trong lớp
            List<StudentDTO> availableStudents = allStudents.Where(student => !studentsInClass.Any(sic => sic.StudentId==student.UserId))
                .ToList();
            if (availableStudents.Count==0)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Tất cả sinh viên đã có trong lớp.";
                }
            else
                {
                aPIResponse.IsSuccess=true;
                aPIResponse.Result=availableStudents;
                }
            return aPIResponse;
            }
        #endregion

        #region Count student in exam schedule
        /// <summary>
        /// Count number of student in exam Schedule
        /// </summary>
        /// <param name="examScheduleId"></param>
        /// <returns></returns>
        public async Task<int> CountStudentInExamSchedule(int examScheduleId)
            {
            int count =await _repository.CountStudentInExamSchedule(examScheduleId);
            return count;
            }
        #endregion
        }
    }
