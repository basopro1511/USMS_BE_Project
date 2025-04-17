using BusinessObject;
using BusinessObject.AppDBContext;
using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using UserService.Repository.StudentRepository;
using UserService.Repository.UserRepository;
using UserService.Services.CloudService;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;

namespace UserService.Services.StudentServices
    {
    public class StudentService
        {
        private readonly IStudentRepository _repository;
        private readonly IUserRepository _userRepository;
        public StudentService()
            {
            _repository=new StudentRepository();
            _userRepository=new UserRepository();
            }

        #region Get All Student
        /// <summary>
        /// Get All Student from Database
        /// </summary>
        /// <param name="teacherDto"></param>
        /// <returns></returns>
        public async Task<APIResponse> GetAllStudent()
            {
            APIResponse aPIResponse = new APIResponse();
            List<User> users = await _repository.GetAllStudent();
            List<UserDTO> usersDTOs = new List<UserDTO>();
            foreach (var item in users)
                {
                using (var _db = new MyDbContext())
                    {
                    UserDTO userDTO = new UserDTO();
                    userDTO.CopyProperties(item);
                    Student? student = await _db.Student.FirstOrDefaultAsync(x => x.StudentId==userDTO.UserId);
                    if (student!=null)
                        userDTO.Term=student.Term;
                    usersDTOs.Add(userDTO);
                    }
                };
            #region validation 
            List<(bool condition, string errorMessage)>? validations = new List<(bool condition, string errorMessage)>
            {
                  (users == null, "Không tìm thấy sinh viên!"),
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
            aPIResponse.Result=usersDTOs;
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

        #region Generate Next Student Id
        /// <summary>
        /// Generate next UserId for students (with majorId) 
        /// </summary>
        /// <param name="majorId"></param>
        /// <returns></returns>
        public async Task<string> GenerateNextStudentId(string majorId)
            {
            // Lấy 2 chữ số cuối của năm hiện tại, ví dụ: 2025 -> "25"
            string yearSuffix = (DateTime.Now.Year%100).ToString("D2");
            // Tạo prefix mới: majorId + yearSuffix, ví dụ: "SE25"
            string prefix = majorId+yearSuffix;
            // Lấy danh sách sinh viên từ repository
            var allStudents = await _repository.GetAllStudent();
            var filteredStudents = allStudents.Where(s => s.UserId.StartsWith(prefix)).ToList();
            if (filteredStudents.Count==0)
                {
                return prefix+"0001";
                }
            // Lấy số thứ tự lớn nhất hiện có sau prefix
            int maxNumber = filteredStudents
                .Select(s => int.Parse(s.UserId.Substring(prefix.Length)))
                .Max();
            int nextNumber = maxNumber+1;
            // Trả về UserId mới với định dạng: majorId + yearSuffix + 4 chữ số
            return prefix+nextNumber.ToString("D4");
            }
        #endregion

        #region Generate StudentId
        private async Task<string> GeneratePreEmail(string firstName, string middleName, string lastName)
            {
            var teachers = await _repository.GetAllStudent();
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
            return userId;
            }
        #endregion

        #region Add New Student
        /// <summary>
        /// Add New Student Into Database
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<APIResponse> AddNewStudent(UserDTO userDTO)
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
                  (userDTO.PersonalEmail.Length>100, "Độ dài email không thể vượt quá 100 ký tự"),
                  (!userDTO.PhoneNumber.All(char.IsDigit) || userDTO.PhoneNumber.Length != 10 || !userDTO.PhoneNumber.StartsWith("0"),
                  "Số điện thoại phải có 10 số và bắt đầu bằng một số từ 0 (ví dụ: 0901234567)."),
                  (!IsValidEmail(userDTO.PersonalEmail), "Vui lòng nhập địa chỉ email hợp lệ (ví dụ: example@example.com)."),
                  (userDTO.PasswordHash.Length < 8 || userDTO.PasswordHash.Length > 36,"Độ dài mật khẩu phải từ 8 đến 20 ký tự."),
                  (userDTO.DateOfBirth > DateOnly.FromDateTime(DateTime.Now), "Ngày sinh không thể là ngày trong tương lai."),
                  (existEmail, "Email này đã tồn tại trong hệ thống."),
                  (existPhone, "Số điện thoại này đã tồn tại trong hệ thống."),
                  (!isFirstNameValid, "Tên chỉ được chứa chữ cái và không chứa số và khoảng trắng."),
                  (!isLastNameValid,"Họ chỉ được chứa chữ cái và không chứa số và khoảng trắng."),
                  (!isMiddleNameValid,"Tên đệm chỉ được chứa chữ cái và không chứa số .")
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
                #region 2. Generate UserID và Email 
                userDTO.UserId=await GenerateNextStudentId(userDTO.MajorId);
                string preEmail = await GeneratePreEmail(userDTO.FirstName, userDTO.MiddleName, userDTO.LastName);
                // tạo Email trường          
                userDTO.Email=preEmail+userDTO.UserId+"@fpt.edu.vn";   //HoangNQ + SE0001 + FPT.EDU.VN
                #endregion
                #region 3. Save Image vào Cloud
                CloudinaryService _cloudService = new CloudinaryService();
                userDTO.UserAvartar=await _cloudService.UploadImageFromBase64(userDTO.UserAvartar);
                #endregion
                userDTO.RoleId=5; //Id Student là 5
                userDTO.PasswordHash=HashPassword(userDTO.PasswordHash);
                userDTO.Status=1; // 1 là đang hoạt động
                User user = new User();
                user.CopyProperties(userDTO);
                bool isSuccess = await _repository.AddNewStudent(user);
                if (isSuccess)
                    {
                    Student student = new Student();
                    student.StudentId=userDTO.UserId;
                    student.MajorId=userDTO.MajorId;
                    student.Term=1;
                    bool isAdded = await _repository.AddNewStudentForStudentTable(student);
                    return new APIResponse
                        {
                        IsSuccess=true,
                        Message="Thêm mới sinh viên thành công."
                        };
                    }
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Thêm mới sinh viên thất bại."
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

        #region Update Student
        /// <summary>
        /// Update Student information
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        public async Task<APIResponse> UpdateStudent(UserDTO userDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            var user = await _userRepository.GetUserById(userDTO.UserId);
            #region 1. Validation
            // Kiểm tra số điện thoại: nếu số điện thoại mới khác với số hiện có, kiểm tra xem nó đã tồn tại trong DB hay chưa
            bool existPhone = false;
            if (user!=null&&userDTO.PhoneNumber!=user.PhoneNumber)
                {
                existPhone=await _userRepository.isPhonelExist(userDTO.PhoneNumber);
                }
            bool existEmail = false;
            if (user!=null&&userDTO.PersonalEmail!=user.PersonalEmail)
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
                  (!IsValidEmail(userDTO.PersonalEmail), "Vui lòng nhập địa chỉ email hợp lệ (ví dụ: example@example.com)."),
                  (!IsValidEmail(userDTO.Email), "Vui lòng nhập địa chỉ email hợp lệ (ví dụ: example@example.com)."),
                  (userDTO.DateOfBirth > DateOnly.FromDateTime(DateTime.Now), "Ngày sinh không thể là ngày trong tương lai."),
                  (user == null, "Không tìm thấy sinh viên viên cần cập nhật"),
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
            #region 3. Save Image vào Cloud
            // Nếu UserAvartar không null và bắt đầu bằng "data:" thì upload ảnh mới
            if (!string.IsNullOrEmpty(userDTO.UserAvartar)&&userDTO.UserAvartar.StartsWith("data:"))
                {
                CloudinaryService _cloudService = new CloudinaryService();
                userDTO.UserAvartar=await _cloudService.UploadImageFromBase64(userDTO.UserAvartar);
                }
            else
                {
                userDTO.UserAvartar=user.UserAvartar;
                }
            #endregion             
            User userModel = new User();
            userModel.CopyProperties(userDTO);
            bool isSuccess = await _repository.UpdateStudent(userModel);
            if (isSuccess)
                {
                await _repository.UpdateStudentTerm(userDTO.UserId, userDTO.Term); // Update Term trong bảng student
                return new APIResponse
                    {
                    IsSuccess=true,
                    Message="Cập nhật thông tin sinh viên thành công."
                    };
                }
            return new APIResponse
                {
                IsSuccess=false,
                Message="Cập nhật thông tin sinh viên thất bại."
                };
            }
        #endregion

        #region Generate Next Student Id for Excel
        /// <summary>
        /// Generate next UserId for students (with majorId) 
        /// </summary>
        /// <param name="majorId"></param>
        /// <returns></returns>
        public async Task<string> GenerateNextStudentIdForExcel(string majorId)
            {
            // Lấy 2 chữ số cuối của năm hiện tại, ví dụ: 2025 -> "25"
            string yearSuffix = (DateTime.Now.Year%100).ToString("D2");
            // Tạo prefix mới: majorId + yearSuffix, ví dụ: "SE25"
            string prefix = majorId+yearSuffix;
            // Lấy danh sách sinh viên từ repository
            var allStudents = await _repository.GetAllStudent();
            var filteredStudents = allStudents.Where(s => s.UserId.StartsWith(prefix)).ToList();
            if (filteredStudents.Count==0)
                {
                return prefix+"0001";
                }
            // Lấy số thứ tự lớn nhất hiện có sau prefix
            int maxNumber = filteredStudents
                .Select(s => int.Parse(s.UserId.Substring(prefix.Length)))
                .Max();
            int nextNumber = maxNumber+1;
            // Trả về UserId mới với định dạng: majorId + yearSuffix + 4 chữ số
            return prefix+nextNumber.ToString("D4");
            }
        #endregion

        #region Import from Excel
        /// <summary>
        /// Import file students information to add new student list
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<APIResponse> ImportStudentsFromExcel(IFormFile file)
            {
            try
                {
                if (file==null||file.Length==0)
                    {
                    return new APIResponse { IsSuccess=false, Message="File không hợp lệ." };
                    }
                var teachers = new List<User>();
                ExcelPackage.LicenseContext=LicenseContext.NonCommercial;
                // Lấy 2 chữ số cuối của năm hiện tại, ví dụ 2025 -> 25
                string currentYearSuffix = (DateTime.Now.Year%100).ToString("D2");
                // Lấy danh sách tất cả sinh viên từ DB trước khi thêm mới
                var allStudents = await _repository.GetAllStudent();
                var studentIdMap = allStudents
        .Where(s => !string.IsNullOrEmpty(s.UserId))
        .Where(s => s.UserId.StartsWith(s.MajorId+currentYearSuffix))
        .GroupBy(s => s.MajorId+currentYearSuffix)
        .ToDictionary(
            g => g.Key,
            g => g.Select(s => int.Parse(s.UserId.Substring(g.Key.Length))).DefaultIfEmpty(0).Max()
        );
                using (var stream = new MemoryStream())
                    {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                        {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row<=rowCount; row++)
                            {
                            if (string.IsNullOrWhiteSpace(worksheet.Cells[row, 2].Text))
                                {
                                break;
                                }
                            string majorId = worksheet.Cells[row, 9].Text;
                            // Tạo prefix: majorId + currentYearSuffix, ví dụ: "SE25"
                            string prefix = majorId+currentYearSuffix;
                            if (!studentIdMap.ContainsKey(prefix))
                                {
                                studentIdMap[prefix]=0;
                                }
                            studentIdMap[prefix]++;
                            string nextUserId = prefix+studentIdMap[prefix].ToString("D4");
                            DateOnly dob;
                            if (worksheet.Cells[row, 8].Value is DateTime dt)
                                {
                                dob=DateOnly.FromDateTime(dt);
                                }
                            else
                                {
                                string dobText = worksheet.Cells[row, 8].Text.Trim();
                                string[] acceptedFormats = { "d/M/yyyy", "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy" };
                                bool isParsed = DateOnly.TryParseExact(dobText, acceptedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dob);
                                }
                            var user = new User
                                {

                                FirstName=worksheet.Cells[row, 4].Text,
                                MiddleName=worksheet.Cells[row, 3].Text,
                                LastName=worksheet.Cells[row, 2].Text,
                                Gender=worksheet.Cells[row, 5].Text.ToLower()=="Nữ",  // nếu female là true, male sẽ là false
                                PasswordHash=HashPassword("123456789"),
                                PersonalEmail=worksheet.Cells[row, 6].Text,
                                PhoneNumber=worksheet.Cells[row, 7].Text,
                                DateOfBirth=dob,
                                RoleId=5,
                                MajorId=worksheet.Cells[row, 9].Text,
                                Status=1,
                                Address=worksheet.Cells[row, 10].Text,
                                CreatedAt=DateTime.Now,
                                UpdatedAt=DateTime.Now,
                                UserId=nextUserId
                                };
                            #region 1. Validation       
                            string stt = worksheet.Cells[row, 1].Text;
                            bool existEmail = await _userRepository.isPersonalEmailExist(user.PersonalEmail);
                            bool existPhone = await _userRepository.isPhonelExist(user.PhoneNumber);
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
                            string preEmail = await GeneratePreEmail(user.FirstName, user.MiddleName, user.LastName);
                            user.Email=preEmail+user.UserId+"@fpt.edu.vn";
                            teachers.Add(user);
                            }
                        }
                    }
                bool isSuccess = await _repository.AddStudentAsync(teachers);
                if (isSuccess)
                    {
                    foreach (var item in teachers)
                        {
                        Student student = new Student();
                        student.StudentId=item.UserId;
                        student.MajorId=item.MajorId;
                        student.Term=1;
                        await _repository.AddNewStudentForStudentTable(student);
                        }
                    return new APIResponse { IsSuccess=true, Message="Import sinh viên thành công." };
                    }
                return new APIResponse { IsSuccess=false, Message="Import sinh viên thất bại." };
                }
            catch (Exception ex)
                {
                return new APIResponse { IsSuccess=false, Message=ex.Message };
                }
            }
        #endregion

        #region Get Student By ID
        /// <summary>
        /// Get All Student from Database
        /// </summary>
        /// <param name="teacherDto"></param>
        /// <returns></returns>
        public async Task<APIResponse> GetUserById(string userId)
            {
            APIResponse aPIResponse = new APIResponse();
            User user = await _repository.GetStudentById(userId);
            UserDTO userDTO = new UserDTO();
            userDTO.CopyProperties(user);
            using (var _db = new MyDbContext())
                {
                var student = await _db.Student.FirstOrDefaultAsync(x => x.StudentId==userId);
                if (student==null)
                    {
                    return new APIResponse
                        {
                        IsSuccess=false,
                        Message="Không tìm thấy sinh viên"
                        };
                    }
                userDTO.Term=student.Term;
                }
            #region validation 
            List<(bool condition, string errorMessage)>? validations = new List<(bool condition, string errorMessage)>
            {
                  (user == null, "Không tìm thấy sinh viên!"),
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
            aPIResponse.Result=userDTO;
            return aPIResponse;
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
                aPIResponse.Message="Danh sách sinh viên không hợp lệ.";
                return aPIResponse;
                }
            bool isSuccess = await _userRepository.ChangeUserStatusSelected(userIds, status);
            if (isSuccess)
                {
                aPIResponse.IsSuccess=true;
                switch (status)
                    {
                    case 0:
                        aPIResponse.Message="Đã thay đổi trạng thái các sinh viên thành 'Vô hiệu hóa'.";
                        break;
                    case 1:
                        aPIResponse.Message="Các sinh viên đã được kích hoạt.";
                        break;
                    case 2:
                        aPIResponse.Message="Các sinh viên đã hoãn tạm thời.";
                        break;
                    case 3:
                        aPIResponse.Message="Các sinh viên đã tốt nghiệp.";
                        break;
                    default:
                        aPIResponse.Message="Trạng thái không hợp lệ.";
                        break;
                    }
                }
            return aPIResponse;
            }
        #endregion

        #region Export Student Information
        /// <summary>
        /// Export Student Data to Excel by MajorId ( MajorId can null to export all Student in Database )
        /// </summary>
        /// <param name="majorId"></param>
        /// <returns></returns>
        public async Task<byte[]> ExportStudentsToExcel(string? majorId, int? status)
            {
            var students = await _repository.GetAllStudent();
            if (!string.IsNullOrEmpty(majorId))
                students=students.Where(s => s.MajorId==majorId).ToList();
            if (status.HasValue)
                students=students.Where(s => s.Status==status.Value).ToList();
            ExcelPackage.LicenseContext=LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
                {
                var worksheet = package.Workbook.Worksheets.Add("Students");
                // Header
                worksheet.Cells[1, 1].Value="STT";
                worksheet.Cells[1, 2].Value="Mã sinh viên";
                worksheet.Cells[1, 3].Value="Họ";
                worksheet.Cells[1, 4].Value="Tên đệm";
                worksheet.Cells[1, 5].Value="Tên";
                worksheet.Cells[1, 6].Value="Giới tính";
                worksheet.Cells[1, 7].Value="Email";
                worksheet.Cells[1, 8].Value="SĐT";
                worksheet.Cells[1, 9].Value="Ngày sinh";
                worksheet.Cells[1, 10].Value="Ngành";
                worksheet.Cells[1, 11].Value="Địa Chỉ";
                worksheet.Cells[1, 12].Value="Trạng thái";
                int row = 2;
                int stt = 1;
                foreach (var s in students)
                    {
                    worksheet.Cells[row, 1].Value=stt++;
                    worksheet.Cells[row, 2].Value=s.UserId;
                    worksheet.Cells[row, 3].Value=s.LastName;
                    worksheet.Cells[row, 4].Value=s.MiddleName;
                    worksheet.Cells[row, 5].Value=s.FirstName;
                    worksheet.Cells[row, 6].Value=s.Gender ? "Nữ" : "Nam";
                    worksheet.Cells[row, 7].Value=s.Email;
                    worksheet.Cells[row, 8].Value=s.PhoneNumber;
                    worksheet.Cells[row, 9].Value=s.DateOfBirth.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 10].Value=s.MajorId;
                    worksheet.Cells[row, 11].Value=s.Address;
                    worksheet.Cells[row, 12].Value=s.Status==1 ? "Đang học" : "Ngừng học";
                    row++;
                    }
                return package.GetAsByteArray();
                }
            }
        #endregion

        #region Export Form Add Student Information
        /// <summary>
        /// Export From Add Student
        /// </summary>
        /// <param name="majorId"></param>
        /// <returns></returns>
        public async Task<byte[]> ExportFormAddStudent()
            {
            ExcelPackage.LicenseContext=LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
                {
                var worksheet = package.Workbook.Worksheets.Add("Students");

                // Header
                worksheet.Cells[1, 1].Value="STT";
                worksheet.Cells[1, 2].Value="Họ";
                worksheet.Cells[1, 3].Value="Tên đệm";
                worksheet.Cells[1, 4].Value="Tên";
                worksheet.Cells[1, 5].Value="Giới tính";
                worksheet.Cells[1, 6].Value="Email";
                worksheet.Cells[1, 7].Value="SĐT";
                worksheet.Cells[1, 8].Value="Ngày sinh";
                worksheet.Cells[1, 9].Value="Chuyên Ngành";
                worksheet.Cells[1, 10].Value="Địa Chỉ";
                // Gán công thức tự động tăng STT từ dòng 2 đến 1000
                for (int row = 2; row<=1000; row++)
                    {
                    worksheet.Cells[row, 1].Formula=$"=ROW()-1";
                    }
                // Định dạng các cột để bắt validation
                var range = worksheet.Cells[2, 8, 1000, 8]; // Cột ngày sinh theo định dạng ngày/tháng/năm
                range.Style.Numberformat.Format="dd/mm/yyyy";
                var phoneRange = worksheet.Cells[2, 7, 1000, 7]; // Cột số điện thoại
                phoneRange.Style.Numberformat.Format="@"; // Định dạng dạng text để giữ số 0 ở đầu tiên
                // Tạo dropdown cho chọn iới tính: Nam / Nữ
                var genderValidation = worksheet.DataValidations.AddListValidation("E2:E1000");
                genderValidation.Formula.Values.Add("Nam");
                genderValidation.Formula.Values.Add("Nữ");
                genderValidation.ShowErrorMessage=true;
                genderValidation.ErrorStyle=OfficeOpenXml.DataValidation.ExcelDataValidationWarningStyle.stop;
                genderValidation.ErrorTitle="Giá trị không hợp lệ";
                genderValidation.Error="Vui lòng chọn Nam hoặc Nữ";
                // Tạo dropdown cho Chuyên ngành do chuyên ngành mặc định có 4 nên chỉ để 4, có thể dùng hàm getmajor nếu có CN mới
                var majorValidation = worksheet.DataValidations.AddListValidation("I2:I1000");
                majorValidation.Formula.Values.Add("SE");
                majorValidation.Formula.Values.Add("BA");
                majorValidation.Formula.Values.Add("LG");
                majorValidation.Formula.Values.Add("CT");
                majorValidation.ShowErrorMessage=true;
                majorValidation.ErrorTitle="Sai chuyên ngành";
                majorValidation.Error="Chuyên ngành chỉ có thể là SE, BA, LG hoặc CT";
                worksheet.Cells.AutoFitColumns();
                return package.GetAsByteArray();
                }
            }
        #endregion

        }
    }

