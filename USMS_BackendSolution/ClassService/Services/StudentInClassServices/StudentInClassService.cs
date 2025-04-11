using ClassBusinessObject;
using ClassBusinessObject.ModelDTOs;
using ClassBusinessObject.Models;
using ClassService.Repositories.StudentInClassRepository;
using ISUZU_NEXT.Server.Core.Extentions;
using OfficeOpenXml;
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
        public async Task<APIResponse> GetAllStudentInClass()
            {
            APIResponse aPIResponse = new APIResponse();
            List<StudentInClass>? studentInClasses = await _repository.GetAllStudentInClass();
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
        public async Task<APIResponse> GetStudentInClassByStudentId(string student)
            {
            APIResponse aPIResponse = new APIResponse();
            StudentInClass studentInClasses = await _repository.GetStudentInClassByStudentId(student);
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
        public async Task<APIResponse> GetStudentInClassByClassId(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            List<StudentInClass> studentsInClasses = await _repository.GetStudentInClassByClassId(id);
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
        public async Task<APIResponse> GetClassSubjectId(string id)
            {
            APIResponse aPIResponse = new APIResponse();
            List<int>? studentInClasses = await _repository.GetClassSubjectId(id);
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
        public async Task<APIResponse> AddStudentToClass(StudentInClassDTO studentInClassDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            var checkExist = await _repository.GetStudentInClassByStudentIdAndClass(studentInClassDTO.StudentId, studentInClassDTO.ClassSubjectId);
            if (checkExist!=null)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Sinh viên này đã tồn tại trong lớp học";
                return aPIResponse;
                }
            int numberOfStudent = await GetStudentCountByClassSubjectId(studentInClassDTO.ClassSubjectId);
            int numberOfStudentAfterAdd = numberOfStudent+1;
            if (numberOfStudentAfterAdd>40)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Số lượng sinh viên thêm vào đã giới hạn của lớp ( giới hạn 1 lớp có tối đa 40 sinh viên )";
                return aPIResponse;
                }
            StudentInClass studentInClass = new StudentInClass();
            studentInClass.CopyProperties(studentInClassDTO);
            bool success = await _repository.AddStudentToClass(studentInClass);
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
        public async Task<APIResponse> AddMultipleStudentsToClass(List<StudentInClassDTO> studentsInClassDTO)
            {
            APIResponse apiResponse = new APIResponse();
            try
                {
                List<StudentInClass> studentInClasses = new List<StudentInClass>();
                int classSubjectId = studentsInClassDTO.First().ClassSubjectId;
                List<StudentInClass> existingStudents = await _repository.GetStudentInClassByClassId(classSubjectId);
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
                int numberOfStudent = await GetStudentCountByClassSubjectId(classSubjectId);
                int numberOfStudentAfterAdd = numberOfStudent+newStudents.Count;
                if (numberOfStudentAfterAdd>40)
                    {
                    apiResponse.IsSuccess=false;
                    apiResponse.Message="Số lượng sinh viên thêm vào đã giới hạn của lớp ( giới hạn 1 lớp có tối đa 40 sinh viên )";
                    return apiResponse;
                    }
                foreach (var item in newStudents)
                    {
                    StudentInClass studentInClass = new StudentInClass();
                    studentInClass.CopyProperties(item);
                    studentInClasses.Add(studentInClass);
                    }
                studentInClasses.CopyProperties(studentsInClassDTO);
                bool success = await _repository.AddMultipleStudentsToClass(studentInClasses);
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
        public async Task<APIResponse> UpdateStudentInClass(StudentInClassDTO studentInClassDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            StudentInClass studentInClass = new StudentInClass();
            studentInClass.CopyProperties(studentInClassDTO);
            bool success = await _repository.UpdateStudentInClass(studentInClass);
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
        public async Task<APIResponse> DeleteStudentFromClass(int studentClassId)
            {
            APIResponse aPIResponse = new APIResponse();
            bool success = await _repository.DeleteStudentFromClass(studentClassId);
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
        public async Task<APIResponse> GetStudentInClassByClassIdWithStudentData(int id)
            {
            try
                {
                APIResponse aPIResponse = new APIResponse();
                List<StudentInClass> studentsInClasses = await _repository.GetStudentInClassByClassId(id);
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
                    if (studentDTO==null)
                        {
                        aPIResponse.IsSuccess=false;
                        aPIResponse.Message="Không tìm thấy học sinh với ID = "+item.StudentId;
                        return aPIResponse;
                        }
                    studentDTO.StudentClassId=item.StudentClassId;
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
        public async Task<APIResponse> GetAvailableStudentsForClass(int classId)
            {
            APIResponse aPIResponse = new APIResponse();
            // Lấy danh sách tất cả sinh viên
            List<StudentDTO> allStudents = getAllStudent();
            // Lấy danh sách sinh viên đã có trong lớp
            List<StudentInClass> studentsInClass = await _repository.GetStudentInClassByClassId(classId);
            if (allStudents==null||allStudents.Count==0)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không tìm thấy sinh viên nào.";
                return aPIResponse;
                }
            if (studentsInClass==null)
                {
                studentsInClass=new List<StudentInClass>();
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
        public async Task<int> GetStudentCountByClassSubjectId(int classSubjectId)
            {
            return await _repository.GetStudentCountByClassSubjectId(classSubjectId);
            }
        #endregion

        #region Export Room Information
        /// <summary>
        /// Export Student in Class Information
        /// </summary>
        /// <param name="majorId"></param>
        /// <returns></returns>
        public async Task<byte[]> ExportStudentInClassToExcel(int id)
            {
            APIResponse aPIResponse  = await GetStudentInClassByClassIdWithStudentData(id);
            List<StudentDTO>? models = aPIResponse.Result as List<StudentDTO>;
            if (models==null)
                {
                models=new List<StudentDTO>();
                }
            ExcelPackage.LicenseContext=OfficeOpenXml.LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
                {
                var worksheet = package.Workbook.Worksheets.Add("StudentInClass");
                // Header
                worksheet.Cells[1, 1].Value="STT";
                worksheet.Cells[1, 2].Value="Mã sinh viên";
                worksheet.Cells[1, 3].Value="Tên sinh viên";
                worksheet.Cells[1, 4].Value="Email";
                worksheet.Cells[1, 5].Value="Số điện thoại";
                worksheet.Cells[1, 6].Value="Chuyên ngành";
                int row = 2;
                int stt = 1;
                foreach (var s in models)
                    {
                    worksheet.Cells[row, 1].Value=stt++;
                    worksheet.Cells[row, 2].Value=s.UserId;
                    worksheet.Cells[row, 3].Value=s.LastName+" "+s.MiddleName+" "+s.LastName;
                    worksheet.Cells[row, 4].Value=s.Email;
                    worksheet.Cells[row, 5].Value=s.PhoneNumber;
                    worksheet.Cells[row, 6].Value=s.MajorId;
                    row++;
                    }
                return package.GetAsByteArray();
                }
            }
        #endregion

        }
    }
//Copy Paste 
#region Copy + Pase  
#endregion