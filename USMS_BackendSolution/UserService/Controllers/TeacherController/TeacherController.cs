using BusinessObject;
using BusinessObject.ModelDTOs;
using ExcelDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Services.StudentService;
using UserService.Services.TeacherService;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace UserService.Controllers.TeacherController
    {
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
        {
        private readonly TeacherService _service;
        public TeacherController(TeacherService service)
            {
            _service=service;
            }

        #region Get All Teacher
        [HttpGet]
        public async Task<APIResponse> GetAllTeacher()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetAllTeacher();
            return aPIResponse;
            }
        #endregion

        #region Get All Teacher Available
        [HttpGet("Available/{majorId}")]
        public async Task<APIResponse> GetAllTeacherAvailableByMajorId(string majorId)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetAllTeacherAvailableByMajorId(majorId);
            return aPIResponse;
            }
        #endregion

        #region Add New Teacher
        [HttpPost]
        public async Task<APIResponse> AddNewTeacher(UserDTO userDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.AddNewTeacher(userDTO);
            return aPIResponse;
            }
        #endregion

        #region Update Teacher
        [HttpPut]
        public async Task<APIResponse> UpdateTeacher(UserDTO userDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.UpdateTeacher(userDTO);
            return aPIResponse;
            }
        #endregion

        //#region Import Teacher from Excel
        //[HttpPost("ImportExcel")]
        //public APIResponse AddNewTeacherFromExcel([FromForm] IFormFile file)
        //    {
        //    try
        //        {
        //        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        //        APIResponse overallResponse = new APIResponse();
        //        List<string> errorMessages = new List<string>();
        //        int successCount = 0;
        //        int totalCount = 0;

        //        if (file==null)
        //            {
        //            return new APIResponse
        //                {
        //                IsSuccess=false,
        //                Message="No file found."
        //                };
        //            }

        //        // Tạo tên file duy nhất để tránh trùng lặp
        //        var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
        //        if (!Directory.Exists(uploadFolder))
        //            {
        //            Directory.CreateDirectory(uploadFolder);
        //            }
        //        var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        //        var filePath = Path.Combine(uploadFolder, uniqueFileName);

        //        // Lưu file vào server tạm thời
        //        using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //            file.CopyTo(stream);
        //            }

        //        // Đọc file Excel
        //        using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
        //            {
        //            using (var reader = ExcelReaderFactory.CreateReader(stream))
        //                {
        //                bool isHeaderSkipped = false;
        //                do
        //                    {
        //                    while (reader.Read())
        //                        {
        //                        if (!isHeaderSkipped)
        //                            {
        //                            isHeaderSkipped=true;
        //                            continue;
        //                            }

        //                        totalCount++;

        //                        try
        //                            {
        //                            // Giả sử các cột theo thứ tự: FirstName, MiddleName, LastName, PersonalEmail, PhoneNumber, DateOfBirth, MajorId, Address, Password
        //                            UserDTO userDTO = new UserDTO
        //                                {

        //                                FirstName=reader.GetValue(1)?.ToString().Trim()??"",
        //                                MiddleName=reader.GetValue(2)?.ToString().Trim()??"",
        //                                LastName=reader.GetValue(3)?.ToString().Trim()??"",
        //                                PersonalEmail=reader.GetValue(4)?.ToString().Trim()??"",
        //                                PhoneNumber=reader.GetValue(5)?.ToString().Trim()??"",
        //                                // Chuyển đổi DateOfBirth theo định dạng phù hợp
        //                                DateOfBirth=DateOnly.Parse(reader.GetValue(6)?.ToString().Trim()??""),
        //                                MajorId=reader.GetValue(7)?.ToString().Trim()??"",
        //                                Address=reader.GetValue(8)?.ToString().Trim()??"",
        //                                PasswordHash=reader.GetValue(9)?.ToString().Trim()??""
        //                                };

        //                            // Gọi hàm thêm giáo viên
        //                            var response = _service.AddNewTeacher(userDTO);
        //                            if (response.IsSuccess)
        //                                {
        //                                successCount++;
        //                                }
        //                            else
        //                                {
        //                                errorMessages.Add($"Row {totalCount}: {response.Message}");
        //                                }
        //                            }
        //                        catch (Exception exRow)
        //                            {
        //                            errorMessages.Add($"Row {totalCount}: {exRow.Message}");
        //                            }
        //                        }
        //                    } while (reader.NextResult());
        //                }
        //            }

        //        overallResponse.IsSuccess=errorMessages.Count==0;
        //        overallResponse.Message=$"Processed {totalCount} rows, successfully added {successCount} teachers.";
        //        if (errorMessages.Count>0)
        //            {
        //            overallResponse.Message+=" Errors: "+string.Join(" | ", errorMessages);
        //            }
        //        return overallResponse;
        //        }
        //    catch (Exception ex)
        //        {
        //        throw new Exception("Error importing teacher from Excel: "+ex.Message, ex);
        //        }
        //    }
        //#endregion

        }
    }
