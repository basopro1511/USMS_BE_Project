using ClassBusinessObject;
using ClassBusinessObject.ModelDTOs;
using ClassBusinessObject.Models;
using ClassService.Repositories.StudentInClassRepository;
using Repositories.SubjectRepository;
using System.Net.Http;
using System.Text.Json;

namespace ClassService.Services.StudentInClassServices
    {
    public class StudentInClassService
        {
        private readonly IStudentInClassRepository _repository;
        private readonly HttpClient _httpClient;

        public StudentInClassService(HttpClient httpClient, IStudentInClassRepository repository)
            {
            _httpClient=httpClient;
            _repository=repository;
            }
        public StudentInClassService()
            {
            _repository=new StudentInClassRepository();
            _httpClient=new HttpClient();
            }

        #region Get All Student In Class
        /// <summary>
        ///  Get All Student In Class
        /// </summary>
        /// <returns></returns>
        public APIResponse GetAllStudentInClass()
            {
            APIResponse aPIResponse = new APIResponse();
            List<StudentInClassDTO>? studentInClasses = _repository.GetAllStudentInClass();
            if (studentInClasses==null||studentInClasses.Count==0)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không có học sinh nào trong lớp.";
                }
            aPIResponse.Result=studentInClasses;
            return aPIResponse;
            }
        #endregion

        #region Get Student In Class by Student Id
        /// <summary>
        ///  Get Student In Class by Student Id
        /// </summary>
        /// <returns></returns>
        public APIResponse GetStudentInClassByStudentId(string student)
            {
            APIResponse aPIResponse = new APIResponse();
            StudentInClassDTO studentInClasses = _repository.GetStudentInClassByStudentId(student);
            if (studentInClasses==null)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không tìm thấy học sinh này trong lớp.";
                }
            aPIResponse.Result=studentInClasses;
            return aPIResponse;
            }
        #endregion

        #region Get Student In Class by ClassId
        /// <summary>
        ///  Get Student In Class by ClassId
        /// </summary>
        /// <returns></returns>
        public APIResponse GetStudentInClassByClassId(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            List<StudentInClassDTO> studentsInClasses = _repository.GetStudentInClassByClassId(id);
            if (studentsInClasses==null)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không có học sinh nào trong lớp này.";
                }
            aPIResponse.Result=studentsInClasses;
            return aPIResponse;
            }
        #endregion

        #region Get Class SubjectId by studentId
        /// <summary>
        /// Class SubjectId by studentId
        /// </summary>
        /// <returns>a list int of classSubjectId</returns>
        public APIResponse GetClassSubjectId(string id)
            {
            APIResponse aPIResponse = new APIResponse();
            List<int>? studentInClasses = _repository.GetClassSubjectId(id);
            if (studentInClasses==null||studentInClasses.Count==0)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Sinh viên này đang không có ở trong lớp nào.";
                }
            aPIResponse.Result=studentInClasses;
            return aPIResponse;
            }
        #endregion

        #region Add Student To Class
        /// <summary>
        /// Add a single student to a class
        /// </summary>
        /// <param name="studentInClassDTO"></param>
        /// <returns>APIResponse indicating success</returns>
        public APIResponse AddStudentToClass(StudentInClassDTO studentInClassDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            var checkExist = _repository.GetStudentInClassByStudentIdAndClass(studentInClassDTO.StudentId,studentInClassDTO.ClassSubjectId);
            if (checkExist != null)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Sinh viên này đã tồn tại trong lớp học";
                return aPIResponse;
                }
            int numberOfStudent = GetStudentCountByClassSubjectId(studentInClassDTO.ClassSubjectId);
            int numberOfStudentAfterAdd = numberOfStudent + 1;
            if (numberOfStudentAfterAdd>40)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Số lượng sinh viên thêm vào đã giới hạn của lớp ( giới hạn 1 lớp có tối đa 40 sinh viên )";
                return aPIResponse;
                }
            bool success = _repository.AddStudentToClass(studentInClassDTO);
            if (success)
                {
                return new APIResponse
                    {
                    IsSuccess=true,
                    Message="Thêm sinh viên vào lớp thành công"
                    };
                }
            return new APIResponse
                {
                IsSuccess=false,
                Message="Thêm sinh viên thất bại! "
                };
            }
        #endregion

        #region Add Multiple Students To Class
        /// <summary>
        /// Add multiple students to a class
        /// </summary>
        /// <param name="studentsInClassDTO"></param>
        /// <returns>APIResponse indicating success</returns>
        public APIResponse AddMultipleStudentsToClass(List<StudentInClassDTO> studentsInClassDTO)
            {
            APIResponse apiResponse = new APIResponse();
            try
                {
                int classSubjectId = studentsInClassDTO.First().ClassSubjectId;
                List<StudentInClassDTO> existingStudents = _repository.GetStudentInClassByClassId(classSubjectId);
                var existingStudentIds = new HashSet<string>(existingStudents.Select(s => s.StudentId));
                // Lọc sinh viên chưa có trong lớp
                List<StudentInClassDTO> newStudents = studentsInClassDTO
                    .Where(dto => !existingStudentIds.Contains(dto.StudentId))
                    .ToList();
                if (newStudents.Count==0)
                    {
                    apiResponse.IsSuccess=false;
                    apiResponse.Message="Tất cả sinh viên đã có trong lớp.";
                    return apiResponse;
                    }
                int numberOfStudent = GetStudentCountByClassSubjectId(classSubjectId);
                int numberOfStudentAfterAdd = numberOfStudent + newStudents.Count;
                if (numberOfStudentAfterAdd>40) {
                    apiResponse.IsSuccess=false;
                    apiResponse.Message="Số lượng sinh viên thêm vào đã giới hạn của lớp ( giới hạn 1 lớp có tối đa 40 sinh viên )" ;
                    return apiResponse;
                    }
                bool success = _repository.AddMultipleStudentsToClass(newStudents);
                apiResponse.IsSuccess=success;
                apiResponse.Message=success ? $"Thêm {newStudents.Count} sinh viên thành công." : "Thêm sinh viên thất bại.";
                apiResponse.Result=newStudents;
                }
            catch (Exception ex)
                {
                apiResponse.IsSuccess=false;
                apiResponse.Message="Lỗi khi thêm sinh viên: "+ex.Message;
                }
            return apiResponse;
            }
        #endregion

        #region Update Student in Class
        /// <summary>
        /// Update a student's class information
        /// </summary>
        /// <param name="studentInClassDTO"></param>
        /// <returns>APIResponse indicating success</returns>
        public APIResponse UpdateStudentInClass(StudentInClassDTO studentInClassDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            bool success = _repository.UpdateStudentInClass(studentInClassDTO);
            aPIResponse.IsSuccess=success;
            aPIResponse.Message=success ? "Cập nhật thông tin sinh viên thành công" : "Cập nhật thất bại";
            return aPIResponse;
            }
        #endregion

        #region Delete Student From Class
        /// <summary>
        /// Delete a student from a class
        /// </summary>
        /// <param name="studentClassId"></param>
        /// <returns>APIResponse indicating success</returns>
        public APIResponse DeleteStudentFromClass(int studentClassId)
            {
            APIResponse aPIResponse = new APIResponse();
            bool success = _repository.DeleteStudentFromClass(studentClassId);
            aPIResponse.IsSuccess=success;
            aPIResponse.Message=success ? "Xóa sinh viên khỏi lớp thành công" : "Xóa thất bại";
            return aPIResponse;
            }
        #endregion

        #region Get Student Data by StudentId ( Call Student In Class API )          
        /// <summary>
        /// Call Student API to get StudentData
        /// </summary>
        /// <param name="studentId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private StudentDTO getStudentData(string studentId)
            {
            try
                {
                var response = _httpClient.GetAsync($"https://localhost:7067/api/Student/{studentId}").Result;
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
                return dataResponse.Value.Deserialize<StudentDTO>(options);
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Student In Class By ClassId With Student Data
        /// <summary>
        /// Retrive Student Data to display Student In CLass by class Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public APIResponse GetStudentInClassByClassIdWithStudentData(int id)
            {
            try
                {
                APIResponse aPIResponse = new APIResponse();
                List<StudentInClassDTO> studentsInClasses = _repository.GetStudentInClassByClassId(id);
                List<StudentDTO> studentDTOs = new List<StudentDTO>();
                if (studentsInClasses==null)
                    {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Không có học sinh nào trong lớp này.";
                    }
                foreach (var item in studentsInClasses)
                    {
                    StudentDTO studentDTO = new StudentDTO();
                    studentDTO=getStudentData(item.StudentId);
                    studentDTO.StudentClassId = item.StudentClassId;
                    if (studentDTO==null)
                        {
                        aPIResponse.IsSuccess=false;
                        aPIResponse.Message="Không tìm thấy học sinh với ID = "+item.StudentId;
                        }
                    studentDTOs.Add(studentDTO);
                    }
                aPIResponse.Result=studentDTOs;
                return aPIResponse;
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
        private List<StudentDTO> getAllStudent()
            {
            try
                {
                var response = _httpClient.GetAsync($"https://localhost:7067/api/Student").Result;
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
        public APIResponse GetAvailableStudentsForClass(int classId)
            {
            APIResponse aPIResponse = new APIResponse();
            // Lấy danh sách tất cả sinh viên
            List<StudentDTO> allStudents = getAllStudent();
            // Lấy danh sách sinh viên đã có trong lớp
            List<StudentInClassDTO> studentsInClass = _repository.GetStudentInClassByClassId(classId);
            if (allStudents==null||allStudents.Count==0)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không tìm thấy sinh viên nào.";
                return aPIResponse;
                }
            if (studentsInClass==null)
                {
                studentsInClass=new List<StudentInClassDTO>();
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

        #region GetStudentCountByClassSubjectId
        public int GetStudentCountByClassSubjectId(int classSubjectId)
            {
            return _repository.GetStudentCountByClassSubjectId(classSubjectId);
            }
        #endregion

        }
    }
//Copy Paste 
#region Copy + Pase  
#endregion