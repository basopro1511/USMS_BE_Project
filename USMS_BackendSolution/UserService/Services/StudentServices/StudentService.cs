using BusinessObject;
using BusinessObject.AppDBContext;
using BusinessObject.ModelDTOs;
using BusinessObject.Models;
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
            List<UserDTO> users = await _repository.GetAllStudent();
            foreach (var userDTO in users)
                {
                using (var _db = new MyDbContext())
                    {
                    var student = _db.Student.FirstOrDefault(x => x.StudentId==userDTO.UserId);      
                    if (student !=null) 
                    userDTO.Term=student.Term;
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
            aPIResponse.Result=users;
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
                var users = await _userRepository.GetAllUser();
                #region 1. Validation
                var existEmail = users.FirstOrDefault(x => x.Email==userDTO.Email);
                var existPhone = users.FirstOrDefault(x => x.PhoneNumber==userDTO.PhoneNumber);
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
                #region 2. Generate UserID và Email 
                userDTO.UserId=await GenerateNextStudentId(userDTO.MajorId);
                string preEmail = await GeneratePreEmail(userDTO.FirstName, userDTO.MiddleName, userDTO.LastName);
                // tạo Email trường          
                userDTO.Email=preEmail+userDTO.UserId+"@fpt.edu.vn";   //HoangNQ + SE0001 + FPT.EDU.VN
                #endregion
                #region 3. Save Image vào Cloud
                CloudinaryService _cloudService = new CloudinaryService();
                userDTO.UserAvartar = await _cloudService.UploadImageFromBase64(userDTO.UserAvartar);
                #endregion
                userDTO.RoleId=5; //Id Student là 5
                userDTO.PasswordHash=HashPassword(userDTO.PasswordHash);
                userDTO.Status=1; // 1 là đang hoạt động
                bool isSuccess = await _repository.AddNewStudent(userDTO);
                if (isSuccess)
                    {
                    StudentTableDTO studentDTO = new StudentTableDTO();
                    studentDTO.StudentId=userDTO.UserId;
                    studentDTO.MajorId=userDTO.MajorId;
                    studentDTO.Term=1;
                    bool isAdded = await _repository.AddNewStudentForStudentTable(studentDTO);
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
            var users = await _userRepository.GetAllUser();
            var user = await _userRepository.GetUserById(userDTO.UserId);
            #region 1. Validation
            var existEmail = users.FirstOrDefault(x => x.Email==userDTO.Email);
            var existPhone = users.FirstOrDefault(x => x.PhoneNumber==userDTO.PhoneNumber);
            List<(bool condition, string errorMessage)>? validations = new List<(bool condition, string errorMessage)>
            {
                  (!userDTO.PhoneNumber.All(char.IsDigit) || userDTO.PhoneNumber.Length != 10 || !userDTO.PhoneNumber.StartsWith("0"),
                  "Số điện thoại phải có 10 số và bắt đầu bằng một số từ 0 (ví dụ: 0901234567)."),
                  (!IsValidEmail(userDTO.PersonalEmail), "Vui lòng nhập địa chỉ email hợp lệ (ví dụ: example@example.com)."),
                  (userDTO.DateOfBirth > DateOnly.FromDateTime(DateTime.Now), "Ngày sinh không thể là ngày trong tương lai."),
                  (user == null, "Không tìm thấy sinh viên viên cần cập nhật")
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
            bool isSuccess = await _repository.UpdateStudent(userDTO);
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
                            string majorId = worksheet.Cells[row, 9].Text;
                            // Tạo prefix: majorId + currentYearSuffix, ví dụ: "SE25"
                            string prefix = majorId+currentYearSuffix;
                            if (!studentIdMap.ContainsKey(prefix))
                                {
                                studentIdMap[prefix]=0;
                                }
                            studentIdMap[prefix]++;
                            string nextUserId = prefix+studentIdMap[prefix].ToString("D4");
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
                            var usersCheck = await _userRepository.GetAllUser();
                            var existEmail = usersCheck.FirstOrDefault(x => x.Email==user.Email);
                            var existPhone = usersCheck.FirstOrDefault(x => x.PhoneNumber==user.PhoneNumber);
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
                        StudentTableDTO studentDTO = new StudentTableDTO();
                        studentDTO.StudentId=item.UserId;
                        studentDTO.MajorId=item.MajorId;
                        studentDTO.Term=1;
                        await _repository.AddNewStudentForStudentTable(studentDTO);
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

        #endregion
        /// <summary>
        /// Get All Student from Database
        /// </summary>
        /// <param name="teacherDto"></param>
        /// <returns></returns>
        public async Task<APIResponse> GetUserById(string userId)
            {
            APIResponse aPIResponse = new APIResponse();
           UserDTO user = await _repository.GetStudentById(userId);
            using (var _db= new MyDbContext())
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
                user.Term=student.Term;
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
            aPIResponse.Result=user;
            return aPIResponse;
            }
        }
    }

