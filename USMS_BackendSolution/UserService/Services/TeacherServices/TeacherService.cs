using Azure;
using BusinessObject;
using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using NuGet.DependencyResolver;
using OfficeOpenXml;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using UserService.Repository.TeacherRepository;
using UserService.Repository.UserRepository;
using UserService.Services.CloudService;
using UserService.Services.UserServices;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UserService.Services.TeacherService
    {
    public class TeacherService
        {
        private readonly ITeacherRepository _repository;
        private readonly IUserRepository _userRepository;

        public TeacherService()
            {
            _repository=new TeacherRepository();
            _userRepository =new UserRepository();
            }


        #region Get All Teacher
        /// <summary>
        /// Get All Teacher from Database
        /// </summary>
        /// <param name="teacherDto"></param>
        /// <returns></returns>
        public async Task<APIResponse> GetAllTeacher()
            {
            APIResponse aPIResponse = new APIResponse();
            List<User> teachers = await _repository.GetAllTeacher();
            #region validation 
            List<(bool condition, string errorMessage)>? validations = new List<(bool condition, string errorMessage)>
            {
                  (teachers == null, "Không tìm thấy giáo viên!"),
            };
            foreach (var validation in validations)
                {
                if (validation.condition)
                    {
                    return new APIResponse
                        {
                        IsSuccess=false,
                        Message=validation.errorMessage
                        };
                    }
                }
            #endregion
            aPIResponse.Result=teachers;
            return aPIResponse;
            }
        #endregion

        #region Get All Teachers Available
        /// <summary>
        /// Get All Teacher Available from Database
        /// </summary>
        /// <param name="teacherDto"></param>
        /// <returns></returns>
        public async Task<APIResponse> GetAllTeacherAvailableByMajorId(string majorId)
            {
            APIResponse aPIResponse = new APIResponse();
            List<User> teachers = await _repository.GetAllTeacherAvailableByMajorId(majorId);
            #region validation 
            List<(bool condition, string errorMessage)>? validations = new List<(bool condition, string errorMessage)>
            {
                  (teachers == null, "Không tìm thấy giáo viên!"),
            };
            foreach (var validation in validations)
                {
                if (validation.condition)
                    {
                    return new APIResponse
                        {
                        IsSuccess=false,
                        Message=validation.errorMessage
                        };
                    }
                }
            #endregion       
            aPIResponse.IsSuccess=true;
            aPIResponse.Result=teachers;
            return aPIResponse;
            }
        #endregion

        #region Valid Email
        /// <summary>
        /// Check email is valid or not
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool IsValidEmail(string email)
            {
            try
                {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address==email;
                }
            catch
                {
                return false;
                }
            }
        #endregion

        #region HashPassword
        /// <summary>
        /// Hashing password
        /// </summary>
        /// <param name="plainPassword"></param>
        /// <returns></returns>
        public string HashPassword(string plainPassword)
            {
            using (SHA256 sha256 = SHA256.Create())
                {
                byte[] bytes = Encoding.UTF8.GetBytes(plainPassword);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hashBytes);
                }
            }
        #endregion

        #region Remove Diacritics 
        /// <summary>
        /// Xóa dấu và xóa ký tự Đ.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string RemoveDiacritics(string text)
            {
            if (string.IsNullOrWhiteSpace(text))
                return text;
            text=text.Normalize(NormalizationForm.FormD); // Chuẩn hóa chuỗi để tách dấu
            StringBuilder sb = new StringBuilder();
            foreach (char c in text)
                {
                UnicodeCategory uc = CharUnicodeInfo.GetUnicodeCategory(c);
                if (uc!=UnicodeCategory.NonSpacingMark) // Loại bỏ dấu
                    {
                    sb.Append(c);
                    }
                }
            string result = sb.ToString().Normalize(NormalizationForm.FormC);
            result=result.Replace("Đ", "D").Replace("đ", "d");
            return result;
            }
        #endregion

        #region Add New Teacher
        /// <summary>
        /// Add New Teacher Into Database
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<APIResponse> AddNewTeacher(UserDTO userDTO)
            {
            try
                {
                APIResponse aPIResponse = new APIResponse();
                var teachers = await _userRepository.GetAllUser();
                #region 1. Validation
                var existEmail = teachers.FirstOrDefault(x => x.Email==userDTO.Email);
                var existPhone = teachers.FirstOrDefault(x => x.PhoneNumber==userDTO.PhoneNumber);
                List<(bool condition, string errorMessage)>? validations = new List<(bool condition, string errorMessage)>
            {
                  (!userDTO.PhoneNumber.All(char.IsDigit) || userDTO.PhoneNumber.Length != 10 || !userDTO.PhoneNumber.StartsWith("0"),
                  "Số điện thoại phải có 10 số và bắt đầu bằng một số từ 0 (ví dụ: 0901234567)."),
                  (!IsValidEmail(userDTO.PersonalEmail), "Vui lòng nhập địa chỉ email hợp lệ (ví dụ: example@example.com)."),
                  (userDTO.PasswordHash.Length < 8 || userDTO.PasswordHash.Length > 36,"Độ dài mật khẩu phải từ 8 đến 20 ký tự."),
                  (userDTO.DateOfBirth > DateOnly.FromDateTime(DateTime.Now), "Ngày sinh không thể là ngày trong tương lai."),
                  (existEmail != null, "Email này đã tồn tại trong hệ thống."),
                  (existPhone != null, "Số điện thoại này đã tồn tại trong hệ thống."),
            };
                foreach (var validation in validations)
                    {
                    if (validation.condition)
                        {
                        return new APIResponse
                            {
                            IsSuccess=false,
                            Message=validation.errorMessage
                            };
                        }
                    }
                #endregion

                #region 2. Generate TeacherID
                //// 1.  Viết hoa chữ cái đầu của họ, tên, và tên đệm ví dụ nguyen quoc hoang => Nguyen Quoc Hoang
                //string firstName = RemoveDiacritics(
                //    CultureInfo.CurrentCulture.TextInfo.ToTitleCase(userDTO.FirstName.ToLower())
                //);
                //string lastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(userDTO.LastName.ToLower().Substring(0, 1).Replace("Đ", "D").Replace("đ", "d")); ;
                //string middleName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(userDTO.MiddleName.ToLower().Replace("Đ", "D").Replace("đ", "d"));
                //// 2. Lấy chữ cái đầu tiên của mỗi phần trong MiddleName
                //string secondMidName = "";
                //var middleNameParts = userDTO.MiddleName.Split(' ');
                //foreach (var part in middleNameParts)
                //    {
                //    if (!string.IsNullOrEmpty(part))
                //        {
                //        secondMidName+=part.Substring(0, 1).ToUpper().Replace("Đ", "D").Replace("đ", "d"); // Lấy chữ cái đầu tiên của từng phần trong MiddleName
                //        }
                //    }
                //// 3. Tạo UserId ví dụ Nguyen Quoc Hoang => HoangNQ
                //userDTO.UserId=firstName+lastName+secondMidName;
                //// 4. Kiểm tra xem TeacherId này đã tồn tại trong cơ sở dữ liệu chưa (Check viết hoa hay viết thường)
                //string baseUserId = userDTO.UserId; // Giữ lại UserId gốc để append số
                ////var count = teachers.Count(u => u.UserId.Equals(userDTO.UserId, StringComparison.OrdinalIgnoreCase));
                //int count = 0;
                //while (teachers.Any(x=> x.UserId.Equals(userDTO.UserId, StringComparison.OrdinalIgnoreCase)))
                //    {
                //    count++;
                //    userDTO.UserId=$"{baseUserId}{count:D2}"; // Luôn nối vào UserId gốc
                //    }
                #endregion

                userDTO.UserId=await GenerateUserId(userDTO.FirstName, userDTO.MiddleName, userDTO.LastName);
                #region 3. Save Image vào Cloud
                CloudinaryService _cloudService = new CloudinaryService();
                userDTO.UserAvartar=await _cloudService.UploadImageFromBase64(userDTO.UserAvartar);
                #endregion
                //4. tạo Email trường                                             
                userDTO.Email=userDTO.UserId+"@fpt.edu.vn";
                //5. Default Fields
                userDTO.RoleId=4; //Id teacher là 4
                userDTO.PasswordHash=HashPassword(userDTO.PasswordHash);
                userDTO.Status=1; // 1 là đang hoạt động
                User user = new User();
                user.CopyProperties(userDTO);
                bool isSuccess = await _repository.AddNewTeacher(user);
                if (isSuccess)
                    {
                    return new APIResponse
                        {
                        IsSuccess=true,
                        Message="Thêm mới giáo viên thành công."
                        };
                    }
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Thêm mới giáo viên thất bại."
                    };
                }
            catch (Exception ex)
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message=ex.Message
                    };
                }
            }
        #endregion

        #region Update Teacher
        /// <summary>
        /// Update Teacher information
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        public async Task<APIResponse> UpdateTeacher(UserDTO userDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            var teachers = await _userRepository.GetAllUser();
            var teacher = teachers.FirstOrDefault(x => x.UserId==userDTO.UserId);
            #region 1. Validation
            var existEmail = teachers.FirstOrDefault(x => x.Email==userDTO.Email);
            var existPhone = teachers.FirstOrDefault(x => x.PhoneNumber==userDTO.PhoneNumber);
            List<(bool condition, string errorMessage)>? validations = new List<(bool condition, string errorMessage)>
            {
                  (!userDTO.PhoneNumber.All(char.IsDigit) || userDTO.PhoneNumber.Length != 10 || !userDTO.PhoneNumber.StartsWith("0"),
                  "Số điện thoại phải có 10 số và bắt đầu bằng một số từ 0 (ví dụ: 0901234567)."),
                  (!IsValidEmail(userDTO.PersonalEmail), "Vui lòng nhập địa chỉ email hợp lệ (ví dụ: example@example.com)."),
                  (userDTO.DateOfBirth > DateOnly.FromDateTime(DateTime.Now), "Ngày sinh không thể là ngày trong tương lai."),
                  (teacher == null, "Không tìm thấy giáo viên cần cập nhật")
            };
            foreach (var validation in validations)
                {
                if (validation.condition)
                    {
                    return new APIResponse
                        {
                        IsSuccess=false,
                        Message=validation.errorMessage
                        };
                    }
                }
            #endregion
            #region 3. Save Image vào Cloud
            // Nếu UserAvartar không null và bắt đầu bằng "data:" thì upload ảnh mới
            if (!string.IsNullOrEmpty(userDTO.UserAvartar)&&userDTO.UserAvartar.StartsWith("data:"))
                {
                CloudinaryService _cloudService = new CloudinaryService();
                userDTO.UserAvartar=await _cloudService.UploadImageFromBase64(userDTO.UserAvartar);
                }
            else
                {
                userDTO.UserAvartar=teacher.UserAvartar;
                }
            #endregion                             
            User user = new User();
            user.CopyProperties(userDTO);
            bool isSuccess = await _repository.UpdateTeacher(user);
            if (isSuccess)
                {
                return new APIResponse
                    {
                    IsSuccess=true,
                    Message="Cập nhật thông tin giáo viên thành công."
                    };
                }
            return new APIResponse
                {
                IsSuccess=false,
                Message="Cập nhật thông tin giáo viên thất bại."
                };
            }
        #endregion

        #region Generate Teacher Id
        /// <summary>
        /// Use to generate Teacher Id
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="middleName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        private async Task<string> GenerateUserId(string firstName, string middleName, string lastName)
            {
            var teachers = await _repository.GetAllTeacher();
            // 1.  Viết hoa chữ cái đầu của họ, tên, và tên đệm ví dụ nguyen quoc hoang => Nguyen Quoc Hoang
            string firstNameGenerate = RemoveDiacritics(
                CultureInfo.CurrentCulture.TextInfo.ToTitleCase(firstName.ToLower())
            );
            string lastNameGenerate = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(lastName.ToLower()).Substring(0, 1).Replace("Đ", "D").Replace("đ", "d");
            string middleNameGenerate = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(middleName.ToLower().Replace("Đ", "D").Replace("đ", "d"));
            // 2. Lấy chữ cái đầu tiên của mỗi phần trong MiddleName
            string secondMidName = "";
            var middleNameParts = middleName.Split(' ');
            foreach (var part in middleNameParts)
                {
                if (!string.IsNullOrEmpty(part))
                    {
                    secondMidName+=part.Substring(0, 1).ToUpper().Replace("Đ", "D").Replace("đ", "d"); // Lấy chữ cái đầu tiên của từng phần trong MiddleName
                    }
                }
            // 3. Tạo UserId ví dụ Nguyen Quoc Hoang => HoangNQ
            string userId = firstNameGenerate+lastNameGenerate+secondMidName;
            // 4. Kiểm tra xem TeacherId này đã tồn tại trong cơ sở dữ liệu chưa (Check viết hoa hay viết thường)
            string baseUserId = userId; // Giữ lại UserId gốc để append số
                                                //var count = teachers.Count(u => u.UserId.Equals(userDTO.UserId, StringComparison.OrdinalIgnoreCase));
            int count = 0;
            while (teachers.Any(x => x.UserId.Equals(userId, StringComparison.OrdinalIgnoreCase)))
                {
                count++;
                userId=$"{baseUserId}{count:D2}"; // Luôn nối vào UserId gốc
                }
            return userId;
            }
        #endregion

        #region Import from Excel
        /// <summary>
        /// Import Teacher Information by Excel
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<APIResponse> ImportTeachersFromExcel(IFormFile file)
            {
            try
                {
                if (file==null||file.Length==0)
                    {
                    return new APIResponse { IsSuccess=false, Message="File không hợp lệ." };
                    }
                var teachers = new List<User>();
                ExcelPackage.LicenseContext=LicenseContext.NonCommercial;
                using (var stream = new MemoryStream())
                    {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                        {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row<=rowCount; row++)
                            {
                            var user = new User
                                {
                                FirstName=worksheet.Cells[row, 4].Text,
                                MiddleName=worksheet.Cells[row, 3].Text,
                                LastName=worksheet.Cells[row, 2].Text,
                                Gender=worksheet.Cells[row, 5].Text.ToLower()=="female",  // nếu female là true, male sẽ là false
                                PasswordHash=HashPassword("123456789"),
                                PersonalEmail=worksheet.Cells[row, 6].Text,
                                PhoneNumber=worksheet.Cells[row, 7].Text,
                                DateOfBirth=DateOnly.ParseExact(worksheet.Cells[row, 8].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                                RoleId=4,
                                MajorId=worksheet.Cells[row, 9].Text,
                                Status=1,
                                Address=worksheet.Cells[row, 10].Text,
                                CreatedAt=DateTime.Now,
                                UpdatedAt=DateTime.Now
                                };
                                string stt = worksheet.Cells[row, 1].Text;
                            #region 1. Validation                                      
                            var teachersCheck = await _userRepository.GetAllUser();
                            var existEmail = teachersCheck.FirstOrDefault(x => x.Email==user.Email);
                            var existPhone = teachersCheck.FirstOrDefault(x => x.PhoneNumber==user.PhoneNumber);
                            List<(bool condition, string errorMessage)>? validations = new List<(bool condition, string errorMessage)>
                              {
                             (!user.PhoneNumber.All(char.IsDigit) || user.PhoneNumber.Length != 10 || !user.PhoneNumber.StartsWith("0"),
                             "Số điện thoại tại dòng số "+ stt +" phải có 10 số và bắt đầu bằng một số từ 0 (ví dụ: 0901234567)."),
                           (!IsValidEmail(user.PersonalEmail), "Vui lòng nhập địa chỉ email hợp lệ tại dòng số "+ stt +" (ví dụ: example@example.com)."),
                           (user.DateOfBirth > DateOnly.FromDateTime(DateTime.Now), "Ngày sinh tại dòng số "+ stt +" không thể là ngày trong tương lai."),
                           (existEmail != null, "Email tại dòng số "+ stt +" đã tồn tại trong hệ thống."),
                           (existPhone != null, "Số điện thoại tại dòng số "+ stt +" đã tồn tại trong hệ thống."),
                              };
                            foreach (var validation in validations)
                                {
                                if (validation.condition)
                                    {
                                    return new APIResponse
                                        {
                                        IsSuccess=false,
                                        Message=validation.errorMessage
                                        };
                                    }
                                }
                            #endregion
                            user.UserId=await GenerateUserId(user.FirstName, user.MiddleName, user.LastName);
                            user.Email=user.UserId+"@fpt.edu.vn";
                            teachers.Add(user);
                            }
                        }
                    }
                bool isSuccess = await _repository.AddTeachersAsync(teachers);
                if (isSuccess)
                    {
                    return new APIResponse { IsSuccess=true, Message="Import giáo viên thành công." };
                    }
                return new APIResponse { IsSuccess=false, Message="Import giáo viên thất bại." };
                }
            catch (Exception ex)
                {
                return new APIResponse { IsSuccess=false, Message=ex.Message };
                }
            }
        #endregion

        #region Change Users Status Selected 
        /// <summary>
        /// Change user status
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<APIResponse> ChangeUsersStatusSelected(List<string> userIds, int status)
            {
            APIResponse aPIResponse = new APIResponse();
            if (userIds==null||!userIds.Any())
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Danh sách giáo viên không hợp lệ.";
                return aPIResponse;
                }
            bool isSuccess = await _userRepository.ChangeUserStatusSelected(userIds, status);
            if (isSuccess)
                {
                aPIResponse.IsSuccess=true;
                switch (status)
                    {
                    case 0:
                        aPIResponse.Message="Đã thay đổi trạng thái các giáo viên thành 'Vô hiệu hóa'.";
                        break;
                    case 1:
                        aPIResponse.Message="Đã thay đổi trạng thái các giáo viên thành 'Đang khả dụng'.";
                        break;
                    case 2:
                        aPIResponse.Message="Đã thay đổi trạng thái các giáo viên thành 'Đang tạm hoãn'.";
                        break;
                    default:
                        aPIResponse.Message="Trạng thái không hợp lệ.";
                        break;
                    }
                }
            return aPIResponse;
            }
        #endregion
        }
    }

