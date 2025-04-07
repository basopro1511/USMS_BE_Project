using ClassBusinessObject;
using ClassBusinessObject.AppDBContext;
using ClassBusinessObject.ModelDTOs;
using ClassBusinessObject.Models;
using ClassService.Services.StudentInClassServices;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using Repositories.ClassSubjectRepository;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services.ClassServices
    {
    public class ClassSubjectService
        {
        private readonly IClassRepository _classRepository;
        private readonly HttpClient _httpClient;
        public ClassSubjectService()
            {
            _httpClient=new HttpClient();
            _classRepository=new ClassRepository();
            }

        #region Get All Class Subject
        /// <summary>
        /// Retrive all ClassSubject in Database
        /// </summary>
        /// <returns>a list of all Class Subject in DB</returns>
        public async Task<APIResponse> GetAllClassSubject()
            {
            APIResponse aPIResponse = new APIResponse();
            List<ClassSubject> classSubjectModels = await _classRepository.GetAllClassSubjects();
            List<ClassSubjectDTO> classSubjectDTOs = new List<ClassSubjectDTO>();
            foreach (var model in classSubjectModels)
                {
                ClassSubjectDTO dto = new ClassSubjectDTO();
                dto.CopyProperties(model);
                classSubjectDTOs.Add(dto);
                }
            StudentInClassService studentInClassService = new StudentInClassService();
            foreach (var item in classSubjectDTOs)
                {
                int numberOfStudent = await studentInClassService.GetStudentCountByClassSubjectId(item.ClassSubjectId);
                item.NumberOfStudentInClasss=numberOfStudent; // Gán vào thuộc tính đúng
                }
            if (classSubjectDTOs==null||classSubjectDTOs.Count==0)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không tìm thấy lớp học khả dụng";
                }
            else
                {
                foreach (var item in classSubjectDTOs)
                    {
                    var majorName = await GetMajorNameById(item.MajorId);
                    item.MajorName=majorName.MajorName??"Null";
                    }
                }
            aPIResponse.Result=classSubjectDTOs;
            return aPIResponse;
            }
        #endregion

        #region get all Major by major API
        /// <summary>
        /// Get All Major by Major API
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        private async Task<List<ClassSubjectDTO>>? GetAllMajor()
            {
            try
                {
                var response = await _httpClient.GetAsync("https://localhost:7067/api/Major");
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
                return dataResponse.Value.Deserialize<List<ClassSubjectDTO>>(options);
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region get Major Name by Major Id
        /// <summary>
        /// Get the name of a Major by its Id
        /// </summary>
        /// <param name="id">Major Id</param>
        /// <returns>A ClassSubjectDTO containing Major Name if found, otherwise null</returns>
        private async Task<ClassSubjectDTO> GetMajorNameById(string id)
            {
            try
                {
                var majors = await GetAllMajor();
                var major = majors?.FirstOrDefault(x => x.MajorId==id);
                if (major==null)
                    {
                    throw new Exception($"Chuyên ngành với mã '{id}' không tìm thấy.");
                    }
                return major;
                }
            catch (Exception ex)
                {
                return null;
                }
            }
        #endregion

        #region Get ClassSubject By ClassSubjectId 
        /// <summary>
        /// Retrive a ClassSubject with ClassSubjectId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a ClassSubject by Id</returns>
        public async Task<APIResponse> GetClassSubjectById(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            ClassSubject model = await _classRepository.GetClassSubjectById(id);
            if (model==null)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="ClassSubject with Id: "+id+" is not found";
                }
            else
                {
                ClassSubjectDTO dto = new ClassSubjectDTO();
                dto.CopyProperties(model);
                aPIResponse.Result=dto;
                }
            return aPIResponse;
            }
        #endregion

        #region Get ClassSubject By ClassId
        /// <summary>
        /// Retrive list ClassSubjects by ClassId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a list ClassSubjects by ClassId </returns>
        public async Task<APIResponse> GetClassSubjectByClassId(string id)
            {
            APIResponse aPIResponse = new APIResponse();
            List<ClassSubject> models = await _classRepository.GetClassSubjectByClassIds(id);
            List<ClassSubjectDTO> dtos = new List<ClassSubjectDTO>();
            foreach (var model in models)
                {
                ClassSubjectDTO dto = new ClassSubjectDTO();
                dto.CopyProperties(model);
                dtos.Add(dto);
                }
            if (dtos==null||dtos.Count==0)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Lớp với mã :"+id+" Không tìm thấy !";
                }
            else
                {
                aPIResponse.Result=dtos;
                }
            return aPIResponse;
            }
        #endregion

        #region Get ClassSubject By MajorId, ClassId, Subject Id
        /// <summary>
        /// Retrive list ClassSubjects by MajorId, ClassId, Subject Id
        /// </summary>
        /// <param name="majorId"></param>
        /// <param name="classId"></param>
        /// <param name="term"></param>
        /// <returns>a list ClassSubjects by MajorId, ClassId, Subject Id </returns>
        public async Task<APIResponse> GetClassSubjectByMajorIdClassIdSubjectId(string majorId, string classId, int term)
            {
            APIResponse aPIResponse = new APIResponse();
            List<ClassSubject> models = await _classRepository.GetClassSubjectByMajorIdClassIdTerm(majorId, classId, term);
            List<ClassSubjectDTO> dtos = new List<ClassSubjectDTO>();
            foreach (var model in models)
                {
                ClassSubjectDTO dto = new ClassSubjectDTO();
                dto.CopyProperties(model);
                dtos.Add(dto);
                }
            if (dtos==null||dtos.Count==0)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không tìm thấy lớp học";
                }
            else
                {
                aPIResponse.Result=dtos;
                }
            return aPIResponse;
            }
        #endregion

        #region Add New ClassSubject
        /// <summary>
        /// Add New ClassSubject to database
        /// </summary>
        /// <param name="classSubject">AddUpdateClassSubjectDTO object</param>
        public async Task<APIResponse> AddNewClassSubject(AddUpdateClassSubjectDTO classSubject)
            {
            APIResponse aPIResponse = new APIResponse();
            int count = 1;
            string subtringSemesterId = classSubject.SemesterId.Substring(2); // Ví dụ FA25 -> "25"
                                                                              // Kiểm tra trùng lặp trực tiếp từ database
            using (var dbContext = new MyDbContext())
                {
                classSubject.ClassId=$"{classSubject.MajorId}C{subtringSemesterId}{count:D2}";
                while (await dbContext.ClassSubject.AnyAsync(x => x.ClassId==classSubject.ClassId&&x.SubjectId==classSubject.SubjectId&&x.SemesterId==classSubject.SemesterId))
                    {
                    count++; // Tăng số thứ tự
                    classSubject.ClassId=$"{classSubject.MajorId}C{subtringSemesterId}{count:D2}";
                    }
                }
            ClassSubject model = new ClassSubject();
            model.CopyProperties(classSubject);
            ClassSubject result = await _classRepository.AddNewClassSubject(model);
            ClassSubjectDTO classSubjectDTO = new ClassSubjectDTO();
            classSubjectDTO.CopyProperties(result);
            if (result != null)
                {
                return new APIResponse
                    {
                    IsSuccess=true,
                    Message=$"Thêm mới lớp học thành công! Mã lớp học là: {classSubject.ClassId}" ,
                    Result=classSubjectDTO
                    };
                }
            else
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Thêm mới lớp học thất bại!"
                    };
                }
            }
        #endregion

        #region Update ClassSubject
        /// <summary>
        /// Update ClassSubject in database
        /// </summary>
        /// <param name="classSubject">AddUpdateClassSubjectDTO object</param>
        public async Task<APIResponse> UpdateClassSubject(AddUpdateClassSubjectDTO classSubject)
            {
            APIResponse aPIResponse = new APIResponse();
            // Mapping từ DTO sang model
            ClassSubject model = new ClassSubject();
            model.CopyProperties(classSubject);
            bool isUpdated = await _classRepository.UpdateClassSubject(model);
            if (isUpdated)
                {
                return new APIResponse
                    {
                    IsSuccess=true,
                    Message="Cập nhật lớp học thành công!"
                    };
                }
            return new APIResponse
                {
                IsSuccess=false,
                Message="Cập nhật lớp học thất bại!"
                };
            }
        #endregion

        #region Get Class Subject by MajorId (Change Status)
        /// <summary>
        /// Change Status of Class Subject
        /// </summary>
        /// <param name="id"></param>
        /// <returns>APIResponse with status change result</returns>
        public async Task<APIResponse> ChangeStatusClassSubject(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            ClassSubject model = await _classRepository.GetClassSubjectById(id);
            if (model==null)
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Lớp học với mã lớp đã cung cấp không tồn tại!"
                    };
                }
            bool isSuccess = await _classRepository.ChangeStatusClassSubject(id);
            if (isSuccess)
                {
                return new APIResponse
                    {
                    IsSuccess=true,
                    Message="Thay đổi trạng thái lớp học thành công."
                    };
                }
            return new APIResponse
                {
                IsSuccess=false,
                Message="Thay đổi trạng thái lớp học thất bại."
                };
            }
        #endregion

        #region Lấy danh sách ClassId dựa vào MajorId
        /// <summary>
        /// Gọi xuống repository để lấy danh sách ClassId theo MajorId.
        /// Gói kết quả vào APIResponse và trả về cho Controller.
        /// </summary>
        /// <param name="majorId"></param>
        /// <returns></returns>
        public async Task<APIResponse> GetClassIdsByMajorId(string majorId)
            {
            APIResponse aPIResponse = new APIResponse();
            try
                {
                var classIds = await _classRepository.GetClassIdsByMajorId(majorId);

                if (classIds==null||classIds.Count==0)
                    {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message=$"Không tìm thấy bất kỳ ClassId nào cho MajorId = {majorId}.";
                    }
                else
                    {
                    aPIResponse.IsSuccess=true;
                    aPIResponse.Message="Lấy danh sách ClassId thành công.";
                    }
                aPIResponse.Result=classIds;
                }
            catch (Exception ex)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message=$"Đã xảy ra lỗi: {ex.Message}";
                }
            return aPIResponse;
            }
        #endregion

        #region Get SubjectId by Major and SemesterId to add Exam Schedule
        /// <summary>
        /// Get All Subject in ClassSubject to provide SubjectId for add ExamSchedule
        /// </summary>
        /// <param name="majorId"></param>
        /// <param name="semesterId"></param>
        /// <returns></returns>
        public async Task<APIResponse> GetSubjectIdsByMajorIdAndSemesterId(string majorId, string semesterId)
            {
            APIResponse aPIResponse = new APIResponse();
            List<ClassSubject> models = await _classRepository.GetSubjectInClassSubjectByMajorIdAndSemesterId(majorId, semesterId);
            List<ClassSubjectDTO> dtos = new List<ClassSubjectDTO>();
            foreach (var model in models)
                {
                ClassSubjectDTO dto = new ClassSubjectDTO();
                dto.CopyProperties(model);
                dtos.Add(dto);
                }
            if (dtos==null||dtos.Count==0)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không tìm thấy mã môn học.";
                }
            else
                {
                aPIResponse.IsSuccess=true;
                aPIResponse.Result=dtos;
                }
            return aPIResponse;
            }
        #endregion

        #region Change Class Status Selected 
        /// <summary>
        /// Change class status
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<APIResponse> ChangeClassStatusSelected(List<int> Ids, int status)
            {
            APIResponse aPIResponse = new APIResponse();
            if (Ids==null||!Ids.Any())
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Danh sách lớp học không hợp lệ.";
                return aPIResponse;
                }
            bool isSuccess = await _classRepository.ChangeClassStatusSelected(Ids, status);
            if (isSuccess)
                {
                aPIResponse.IsSuccess=true;
                switch (status)
                    {
                    case 0:
                        aPIResponse.Message="Đã thay đổi trạng thái các lớp học thành 'Chưa bắt đầu'.";
                        break;
                    case 1:
                        aPIResponse.Message="Đã thay đổi trạng thái các lớp học thành 'Đang diễn ra'.";
                        break;
                    case 2:
                        aPIResponse.Message="Đã thay đổi trạng thái các lớp học thành 'Đã kết thúc'.";
                        break;
                    default:
                        aPIResponse.Message="Trạng thái không hợp lệ.";
                        break;
                    }
                }
            return aPIResponse;
            }
        #endregion

        #region AUTO GENERATE CLASS SUBJECT
        public async Task<APIResponse> AutoCreateClassesForStudents(
    List<string> studentIds, // danh sách StudentId từ file import
    int classCapacity,       // giới hạn số sinh viên mỗi lớp (cấu hình qua parameter)
    string majorId,
    string subjectId,
    string semesterId,
    int term)
            {
            try
                {
                StudentInClassService _studentInClassService = new StudentInClassService();
                int totalStudents = studentIds.Count;
                if (totalStudents==0)
                    {
                    return new APIResponse { IsSuccess=false, Message="Không có sinh viên nào được import." };
                    }
                // Tính số lớp cần tạo: theo yêu cầu chia đều, số lớp là số tối thiểu sao cho không vượt quá classCapacity
                int numClasses = (int)Math.Ceiling((double)totalStudents/classCapacity);
                // Để chia đều theo kiểu 33-33-32-32, ta tính:
                int quotient = totalStudents/numClasses;   // số sinh viên cơ bản mỗi lớp
                int remainder = totalStudents%numClasses;    // số lớp sẽ nhận thêm 1 sinh viên
                // Danh sách các lớp được tạo (để trả về hoặc dùng trong log)
                List<ClassSubject> createdClasses = new List<ClassSubject>();
                // Và giả sử _classRepository có hàm AddStudentInClassList để thêm list StudentInClass
                int studentIndex = 0;
                for (int i = 0; i<numClasses; i++)
                    {
                    // Số sinh viên lớp thứ i: nếu i < remainder thì nhận quotient + 1, ngược lại nhận quotient
                    int classStudentCount = quotient+(i<remainder ? 1 : 0);
                    // Tạo lớp mới bằng cách sử dụng DTO đã có sẵn.
                    AddUpdateClassSubjectDTO newClassDto = new AddUpdateClassSubjectDTO
                        {
                        MajorId=majorId,
                        SubjectId=subjectId,
                        SemesterId=semesterId,
                        Term=term,
                        // Các thuộc tính khác nếu cần; Lưu ý: ClassId sẽ được tạo tự động trong hàm AddNewClassSubject của ClassSubjectService.
                        };
                    ClassSubject classSubject = new ClassSubject();
                    APIResponse addResponse = await AddNewClassSubject(newClassDto);
                    if (!addResponse.IsSuccess)
                        {
                        return new APIResponse { IsSuccess=false, Message="Lỗi tạo lớp: "+addResponse.Message };
                        }
                    // Giả sử addResponse.Result trả về ClassSubjectDTO chứa thông tin lớp vừa tạo
                    ClassSubjectDTO? createdDto = addResponse.Result as ClassSubjectDTO;
                    if (createdDto ==null)
                        {
                        return new APIResponse { IsSuccess=false, Message="Không thể lấy thông tin lớp vừa tạo." };
                        }
                    // Tạo danh sách các bản ghi StudentInClass cho lớp này
                    List<StudentInClassDTO> studentAssignments = new List<StudentInClassDTO>();
                    for (int j = 0; j<classStudentCount; j++)
                        {
                        if (studentIndex>=totalStudents)
                            {
                            break;
                            }
                        StudentInClassDTO sic = new StudentInClassDTO
                            {
                            // ClassSubjectId của lớp vừa tạo
                            ClassSubjectId=createdDto.ClassSubjectId,
                            StudentId=studentIds[studentIndex]
                            };
                        studentAssignments.Add(sic);
                        studentIndex++;
                        }
                    APIResponse assignResult = await _studentInClassService.AddMultipleStudentsToClass(studentAssignments);
                    if (!assignResult.IsSuccess)
                        {
                        return new APIResponse { IsSuccess=false, Message=$"Lỗi phân chia sinh viên cho lớp {createdDto.ClassId}." };
                        }
                    createdClasses.Add(new ClassSubject
                        {
                        ClassSubjectId=createdDto.ClassSubjectId,
                        ClassId=createdDto.ClassId,
                        SubjectId=createdDto.SubjectId,
                        SemesterId=createdDto.SemesterId,
                        MajorId=createdDto.MajorId,
                        Term=createdDto.Term,
                        CreatedAt=createdDto.CreatedAt,
                        Status=createdDto.Status
                        });
                    }
                return new APIResponse
                    {
                    IsSuccess=true,
                    Message=$"Tạo tự động {numClasses} lớp thành công và phân chia {totalStudents} sinh viên.",
                    Result=createdClasses
                    };
                }
            catch (Exception ex)
                {
                return new APIResponse { IsSuccess=false, Message=ex.Message };
                }
            }

        #endregion

        #region Copy + Paste  
        #endregion
        }
    }