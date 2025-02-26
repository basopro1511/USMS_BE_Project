using Azure;
using BusinessObject;
using BusinessObject.ModelDTOs;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using UserService.Repository.TeacherRepository;
using UserService.Services.CloudService;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UserService.Services.TeacherService
    {
    public class TeacherService
        {
        private readonly ITeacherRepository _repository;
        private readonly HttpClient _httpClient;
        public TeacherService()
            {
            _repository=new TeacherRepository();
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
            List<UserDTO> teachers = await _repository.GetAllTeacher();
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
            List<UserDTO> teachers = await _repository.GetAllTeacherAvailableByMajorId(majorId);
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
                var teachers = await _repository.GetAllTeacher();
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
                // 1.  Viết hoa chữ cái đầu của họ, tên, và tên đệm ví dụ nguyen quoc hoang => Nguyen Quoc Hoang
                string firstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(userDTO.FirstName.ToLower());
                string lastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(userDTO.LastName.ToLower());
                string middleName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(userDTO.MiddleName.ToLower());
                // 2. Lấy chữ cái đầu tiên của mỗi phần trong MiddleName
                string secondMidName = "";
                var middleNameParts = userDTO.MiddleName.Split(' ');
                foreach (var part in middleNameParts)
                    {
                    if (!string.IsNullOrEmpty(part))
                        {
                        secondMidName+=part.Substring(0, 1).ToUpper(); // Lấy chữ cái đầu tiên của từng phần trong MiddleName
                        }
                    }
                // 3. Tạo UserId ví dụ Nguyen Quoc Hoang => HoangNQ
                userDTO.UserId=userDTO.FirstName+userDTO.LastName.Substring(0, 1)+secondMidName;
                // 4. Kiểm tra xem TeacherId này đã tồn tại trong cơ sở dữ liệu chưa (Check viết hoa hay viết thường)
                var count = teachers.Count(u => u.UserId.Equals(userDTO.UserId, StringComparison.OrdinalIgnoreCase));
                // 5. Nếu đã tồn tại, thêm số vào cuối UserId để Id độc nhất
                if (count>0)
                    {
                    userDTO.UserId=$"{userDTO.UserId}{(count+1):D2}"; // Ví dụ: HoangNQ01
                    }
                #endregion
                #region 3. Save Image vào Cloud
                CloudinaryService _cloudService = new CloudinaryService();
                userDTO.UserAvartar=_cloudService.UploadImageFromBase64(userDTO.UserAvartar);
                #endregion
                //4. tạo Email trường
                userDTO.Email=userDTO.UserId+"@fpt.edu.vn";
                //5. Default Fields
                userDTO.RoleId=4; //Id teacher là 4
                userDTO.PasswordHash=HashPassword(userDTO.PasswordHash);
                userDTO.Status=1; // 1 là đang hoạt động
                bool isSuccess = await _repository.AddNewTeacher(userDTO);
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
            var teachers = await _repository.GetAllTeacher();
            var teacher = teachers.FirstOrDefault(x => x.UserId==userDTO.UserId);
            #region 1. Validation
            var existEmail = teachers.FirstOrDefault(x => x.Email==userDTO.Email);
            var existPhone = teachers.FirstOrDefault(x => x.PhoneNumber==userDTO.PhoneNumber);
            List<(bool condition, string errorMessage)>? validations = new List<(bool condition, string errorMessage)>
            {
                  (!userDTO.PhoneNumber.All(char.IsDigit)&&userDTO.PhoneNumber.Length!=10&&!userDTO.PhoneNumber.StartsWith("0"),
                  "Số điện thoại phải có 10 số và bắt đầu bằng một số từ 0 (ví dụ: 0901234567)."),
                  (!IsValidEmail(userDTO.PersonalEmail), "Vui lòng nhập địa chỉ email hợp lệ (ví dụ: example@example.com)."),
                  (userDTO.PasswordHash.Length < 8 || userDTO.PasswordHash.Length > 36,"Độ dài mật khẩu phải từ 8 đến 20 ký tự."),
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
            bool isSuccess = await _repository.UpdateTeacher(userDTO);
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
        }
    #endregion
    }

