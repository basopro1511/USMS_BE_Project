using BusinessObject;
using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;
using NuGet.Protocol.Core.Types;
using OfficeOpenXml;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using UserService.Repository.StaffRepository;
using UserService.Repository.TeacherRepository;
using UserService.Repository.UserRepository;
using UserService.Services.CloudService;

namespace UserService.Services.StaffServices
    {
    public class StaffService
        {
        private readonly IStaffRepository _repository;
        private readonly IUserRepository _userRepository;

        public StaffService()                                                               
            {
            _repository=new StaffRepository();
            _userRepository=new UserRepository();
            }

        #region Get All Staff
        /// <summary>
        /// Get All Staff from Database
        /// </summary>
        /// <param name="teacherDto"></param>
        /// <returns></returns>
        public async Task<APIResponse> GetAllStaff()
            {
            APIResponse aPIResponse = new APIResponse();
            List<User> staffs = await _repository.GetAllStaff();
            #region validation 
            List<(bool condition, string errorMessage)>? validations = new List<(bool condition, string errorMessage)>
            {
                  (staffs == null, "Không tìm thấy nhân viên!"),
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
            aPIResponse.Result=staffs;
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
            return sb.ToString().Normalize(NormalizationForm.FormC); // Trả về chuỗi không dấu
            }
        #endregion

        #region Generate Staff Id
        /// <summary>
        /// Use to generate Staff Id
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="middleName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        private async Task<string> GenerateUserId(string firstName, string? middleName, string lastName)
            {
            var teachers = await _repository.GetAllStaff();
            // 1.  Viết hoa chữ cái đầu của họ, tên, và tên đệm ví dụ nguyen quoc hoang => Nguyen Quoc Hoang
            string firstNameGenerate = RemoveDiacritics(
                CultureInfo.CurrentCulture.TextInfo.ToTitleCase(firstName.ToLower())
            );
            string lastNameGenerate = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(lastName.ToLower()).Substring(0, 1).Replace("Đ", "D").Replace("đ", "d");
            string? middleNameGenerate = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(middleName.ToLower().Replace("Đ", "D").Replace("đ", "d"));
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

        #region Add New Staff
        /// <summary>
        /// Add New Staff Into Database
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<APIResponse> AddNewStaff(UserDTO userDTO)
            {
            try
                {
                APIResponse aPIResponse = new APIResponse();
                #region 1. Validation
                // Bắt tồn tại email cá nhân và số điện thoại
                bool existEmail = await _userRepository.isPersonalEmailExist(userDTO.PersonalEmail);
                bool existPhone = await _userRepository.isPhonelExist(userDTO.PhoneNumber);
                // Bắt tên phải toàn là chữ và không được nhập số
                bool isFirstNameValid = !string.IsNullOrEmpty(userDTO.FirstName)&&userDTO.FirstName.All(c => char.IsLetter(c));
                bool isLastNameValid = !string.IsNullOrEmpty(userDTO.LastName)&&userDTO.LastName.All(c => char.IsLetter(c));
                bool isMiddleNameValid = string.IsNullOrEmpty(userDTO.MiddleName)||userDTO.MiddleName.All(c => char.IsLetter(c)||char.IsWhiteSpace(c));
                List<(bool condition, string errorMessage)>? validations = new List<(bool condition, string errorMessage)>
            {
                        (userDTO.FirstName == null, "Tên không thể để trống" ),
                  (userDTO.LastName == null, "Họ không thể để trống" ),
                  (!userDTO.PhoneNumber.All(char.IsDigit) || userDTO.PhoneNumber.Length != 10 || !userDTO.PhoneNumber.StartsWith("0"),
                  "Số điện thoại phải có 10 số và bắt đầu bằng một số từ 0 (ví dụ: 0901234567)."),
                  (!IsValidEmail(userDTO.PersonalEmail), "Vui lòng nhập địa chỉ email hợp lệ (ví dụ: example@example.com)."),
                  (userDTO.PersonalEmail.Length>100, "Độ dài email không thể vượt quá 100 ký tự"),
                  (userDTO.PasswordHash.Length < 8 || userDTO.PasswordHash.Length > 36,"Độ dài mật khẩu phải từ 8 đến 20 ký tự."),
                  (userDTO.DateOfBirth > DateOnly.FromDateTime(DateTime.Now), "Ngày sinh không thể là ngày trong tương lai."),
                  (existEmail, "Email này đã tồn tại trong hệ thống."),
                  (existPhone, "Số điện thoại này đã tồn tại trong hệ thống."),
                 (!isFirstNameValid, "Tên chỉ được chứa chữ cái và không chứa số và khoảng trắng."),
                  (!isLastNameValid,"Họ chỉ được chứa chữ cái và không chứa số và khoảng trắng."),
                  (!isMiddleNameValid,"Tên đệm chỉ được chứa chữ cái và không chứa số ."),
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
                userDTO.UserId=await GenerateUserId(userDTO.FirstName, userDTO.MiddleName, userDTO.LastName);
                #region 2. Save Image vào Cloud
                CloudinaryService _cloudService = new CloudinaryService();
                userDTO.UserAvartar=await _cloudService.UploadImageFromBase64(userDTO.UserAvartar);
                #endregion
                //4. tạo Email trường
                userDTO.Email=userDTO.UserId+"@fpt.edu.vn";
                //5. Default Fields
                userDTO.RoleId=2; //Id staff là 2
                userDTO.PasswordHash=HashPassword(userDTO.PasswordHash);
                userDTO.Status=1; // 1 là đang hoạt động
                userDTO.MajorId=null;
                User user = new User();
                user.CopyProperties(userDTO);
                bool isSuccess = await _repository.AddNewStaff(user);
                if (isSuccess)
                    {
                    return new APIResponse
                        {
                        IsSuccess=true,
                        Message="Thêm mới nhân viên thành công."
                        };
                    }
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Thêm mới nhân viên thất bại."
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

        #region Update Staff
        /// <summary>
        /// Update Staff information
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        public async Task<APIResponse> UpdateStaff(UserDTO userDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            var staff = await _userRepository.GetUserById(userDTO.UserId);
            #region 1. Validation
            bool existPhone = false;
            if (staff!=null&&userDTO.PhoneNumber!=staff.PhoneNumber)
                {
                existPhone=await _userRepository.isPhonelExist(userDTO.PhoneNumber);
                }
            bool existEmail = false;
            if (staff!=null&&userDTO.PersonalEmail!=staff.PersonalEmail)
                {
                existEmail=await _userRepository.isPersonalEmailExist(userDTO.PersonalEmail);
                }
            bool isFirstNameValid = !string.IsNullOrEmpty(userDTO.FirstName)&&userDTO.FirstName.All(c => char.IsLetter(c));
            bool isLastNameValid = !string.IsNullOrEmpty(userDTO.LastName)&&userDTO.LastName.All(c => char.IsLetter(c));
            bool isMiddleNameValid = string.IsNullOrEmpty(userDTO.MiddleName)||userDTO.MiddleName.All(c => char.IsLetter(c)||char.IsWhiteSpace(c));
            List<(bool condition, string errorMessage)>? validations = new List<(bool condition, string errorMessage)>
            {
                  (userDTO.FirstName == null, "Tên không thể để trống" ),
                  (userDTO.LastName == null, "Họ không thể để trống" ),
                  (!userDTO.PhoneNumber.All(char.IsDigit) || userDTO.PhoneNumber.Length != 10 || !userDTO.PhoneNumber.StartsWith("0"),
                  "Số điện thoại phải có 10 số và bắt đầu bằng một số từ 0 (ví dụ: 0901234567)."),
                  (userDTO.PersonalEmail.Length>100, "Độ dài email không thể vượt quá 100 ký tự"),
                  (!IsValidEmail(userDTO.PersonalEmail), "Vui lòng nhập địa chỉ email hợp lệ (ví dụ: example@example.com)."),
                  (userDTO.DateOfBirth > DateOnly.FromDateTime(DateTime.Now), "Ngày sinh không thể là ngày trong tương lai."),
                  (staff == null, "Không tìm thấy nhân viên cần cập nhật") ,
                  (!isFirstNameValid, "Tên chỉ được chứa chữ cái và không chứa số và khoảng trắng."),
                  (!isLastNameValid,"Họ chỉ được chứa chữ cái và không chứa số và khoảng trắng."),
                  (!isMiddleNameValid,"Tên đệm chỉ được chứa chữ cái và không chứa số ."),
                  (existEmail, "Email này đã tồn tại trong hệ thống."),
                  (existPhone,"Số điện thoại này đã tồn tại trong hệ thống.")
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
            // Nếu UserAvartar không null và bắt đầu bằng "data:" thì upload ảnh mới
            if (!string.IsNullOrEmpty(userDTO.UserAvartar)&&userDTO.UserAvartar.StartsWith("data:"))
                {
                CloudinaryService _cloudService = new CloudinaryService();
                userDTO.UserAvartar=await _cloudService.UploadImageFromBase64(userDTO.UserAvartar);
                }
            else
                {
                userDTO.UserAvartar=staff.UserAvartar;
                }
            userDTO.MajorId=null; 
            User user = new User();
            user.CopyProperties(userDTO);
            bool isSuccess = await _repository.UpdateStaff(user);
            if (isSuccess)
                {
                return new APIResponse
                    {
                    IsSuccess=true,
                    Message="Cập nhật thông tin nhân viên thành công."
                    };
                }
            return new APIResponse
                {
                IsSuccess=false,
                Message="Cập nhật thông tin nhân viên thất bại."
                };
            }
        #endregion

        #region Import from Excel
        /// <summary>
        /// Import Staff Information by Excel
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<APIResponse> ImportStaffsFromExcel(IFormFile file)
            {
            try
                {
                if (file==null||file.Length==0)
                    {
                    return new APIResponse { IsSuccess=false, Message="File không hợp lệ." };
                    }
                var staffs = new List<User>();
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
                                RoleId=2,
                                MajorId=null,
                                Status=1,
                                Address=worksheet.Cells[row, 9].Text,
                                CreatedAt=DateTime.Now,
                                UpdatedAt=DateTime.Now
                                };
                            string stt = worksheet.Cells[row, 1].Text;
                            #region 1. Validation                                      
                            // Bắt tồn tại email cá nhân và số điện thoại
                            bool existEmail = await _userRepository.isPersonalEmailExist(user.PersonalEmail);
                            bool existPhone = await _userRepository.isPhonelExist(user.PhoneNumber);
                            // Bắt tên phải toàn là chữ và không được nhập số
                            bool isFirstNameValid = !string.IsNullOrEmpty(user.FirstName)&&user.FirstName.All(c => char.IsLetter(c));
                            bool isLastNameValid = !string.IsNullOrEmpty(user.LastName)&&user.LastName.All(c => char.IsLetter(c));
                            bool isMiddleNameValid = string.IsNullOrEmpty(user.MiddleName)||user.MiddleName.All(c => char.IsLetter(c));
                            List<(bool condition, string errorMessage)>? validations = new List<(bool condition, string errorMessage)>
                              {
                             (!user.PhoneNumber.All(char.IsDigit) || user.PhoneNumber.Length != 10 || !user.PhoneNumber.StartsWith("0"),
                             "Số điện thoại tại dòng số "+ stt +" phải có 10 số và bắt đầu bằng một số từ 0 (ví dụ: 0901234567)."),
                           (!IsValidEmail(user.PersonalEmail), "Vui lòng nhập địa chỉ email hợp lệ tại dòng số "+ stt +" (ví dụ: example@example.com)."),
                           (user.DateOfBirth > DateOnly.FromDateTime(DateTime.Now), "Ngày sinh tại dòng số "+ stt +" không thể là ngày trong tương lai."),
                           (existEmail, "Email tại dòng số "+ stt +" đã tồn tại trong hệ thống."),
                           (existPhone, "Số điện thoại tại dòng số "+ stt +" đã tồn tại trong hệ thống."),
                           (!isFirstNameValid, "Tên tại dòng số "+ stt +" chỉ được chứa chữ cái và không chứa số."),
                           (!isLastNameValid,"Họ tại dòng số "+ stt +" chỉ được chứa chữ cái và không chứa số."),
                           (!isMiddleNameValid,"Tên đệm tại dòng số "+ stt +" chỉ được chứa chữ cái và không chứa số .")
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
                            staffs.Add(user);
                            }
                        }
                    }
                bool isSuccess = await _repository.AddStaffsAsync(staffs);
                if (isSuccess)
                    {
                    return new APIResponse { IsSuccess=true, Message="Import nhân viên thành công." };
                    }
                return new APIResponse { IsSuccess=false, Message="Import nhân viên thất bại." };
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
                aPIResponse.Message="Danh sách nhân viên không hợp lệ.";
                return aPIResponse;
                }
            bool isSuccess = await _userRepository.ChangeUserStatusSelected(userIds, status);
            if (isSuccess)
                {
                aPIResponse.IsSuccess=true;
                switch (status)
                    {
                    case 0:
                        aPIResponse.Message="Đã thay đổi trạng thái các nhân viên thành 'Vô hiệu hóa'.";
                        break;
                    case 1:
                        aPIResponse.Message="Đã thay đổi trạng thái các nhân viên thành 'Đang khả dụng'.";
                        break;
                    case 2:
                        aPIResponse.Message="Đã thay đổi trạng thái các nhân viên thành 'Đang tạm hoãn'.";
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

