using BusinessObject;
using BusinessObject.ModelDTOs;
using System.Security.Cryptography;
using System.Text;
using UserService.Repository.StudentRepository;

namespace UserService.Services.StudentService
    {
    public class StudentService
        {
        private readonly IStudentRepository _studentRepository;
        public StudentService()
            {
            _studentRepository=new StudentRepository();
            }

        #region Get All Student
        /// <summary>
        /// Get list all student
        /// </summary>
        /// <returns></returns>
        public APIResponse GetAllStudent()
            {
            APIResponse aPIResponse = new APIResponse();
            List<StudentDTO> students = _studentRepository.GetAllStudent();
            if (students==null||students.Count==0)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không tìm thấy sinh viên nào.";
                }
            else
                {
                aPIResponse.IsSuccess=true;
                aPIResponse.Result=students;
                }
            return aPIResponse;
            }
        #endregion

        #region Get Student By Id
        /// <summary>
        /// Get User by Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public APIResponse GetStudentById(string userId)
            {
            APIResponse aPIResponse = new APIResponse();
            StudentDTO student = _studentRepository.GetStudentById(userId);
            if (student==null)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message=$"Không tìm thấy sinh viên với mã: {userId}.";
                }
            else
                {
                aPIResponse.IsSuccess=true;
                aPIResponse.Result=student;
                }
            return aPIResponse;
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

        #region Generate Next Student Id
        /// <summary>
        /// Generate next UserId for students (with majorId) 
        /// </summary>
        /// <param name="majorId"></param>
        /// <returns></returns>
        public string GenerateNextStudentId(string majorId)
            {
            //Select a list of User with the same MajorId
            var allStudents = _studentRepository.GetAllStudent();
            var filteredStudents = allStudents.Where(s => s.UserId.StartsWith(majorId)).ToList();
            if (filteredStudents.Count==0)
                {
                return majorId+"0001";
                }
            //Get the max number in UserId of the list
            int maxNumber = filteredStudents
                .Select(s => int.Parse(s.UserId.Substring(majorId.Length)))
                .Max();
            int nextNumber = maxNumber+1;
            return majorId+nextNumber.ToString("D4"); // Format as MajorId_xxxx;
            }
        #endregion

        #region Generate Email
        /// <summary>
        /// generate email like the format of FPT Email
        /// </summary>
        /// <param name="first"></param>
        /// <param name="mid"></param>
        /// <param name="last"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GenerateEmail(string first, string mid, string last, string userId)
            {
            //Take the first letter of lastname
            char firstCharacterLastName = last[0];
            //Take the first letter of middlename
            char firstCharacterMidName = mid[0];
            return $"{first}{firstCharacterLastName}{firstCharacterMidName}{userId}@gmail.com";
            }
        #endregion

        #region Add Student
        /// <summary>
        /// add new student
        /// </summary>
        /// <param name="addStudentDTO"></param>
        /// <returns></returns>
        public APIResponse AddStudent(AddStudentDTO addStudentDTO)
            {
            // Require MajorID for Student
            if (string.IsNullOrEmpty(addStudentDTO.MajorId))
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Vui lòng chọn chuyên ngành cho sinh viên."
                    };
                }
            string generatedUserId = GenerateNextStudentId(addStudentDTO.MajorId);
            var studentDTO = new StudentDTO
                {
                UserId=generatedUserId,
                FirstName=addStudentDTO.FirstName,
                MiddleName=addStudentDTO.MiddleName,
                LastName=addStudentDTO.LastName,
                PasswordHash=HashPassword(addStudentDTO.PasswordHash),
                PersonalEmail=addStudentDTO.PersonalEmail,
                Email=GenerateEmail(addStudentDTO.FirstName, addStudentDTO.MiddleName, addStudentDTO.LastName, GenerateNextStudentId(addStudentDTO.MajorId)),
                PhoneNumber=addStudentDTO.PhoneNumber,
                UserAvartar=addStudentDTO.UserAvartar,
                RoleId=5,
                MajorId=addStudentDTO.MajorId,
                Address=addStudentDTO.Address,
                Status=1,
                Term=1,
                DateOfBirth=addStudentDTO.DateOfBirth,
                CreatedAt=addStudentDTO.CreatedAt,
                UpdatedAt=addStudentDTO.UpdatedAt,
                };
            StudentDTO existingUser = _studentRepository.GetStudentById(studentDTO.UserId);
            #region validation cua Add Student
            var validations = new List<(bool condition, string errorMessage)>
              {
             (existingUser != null, $" Sinh viên với mã: {GenerateNextStudentId(addStudentDTO.MajorId)} đã tồn tại. Vui lòng kiểm tra lại"),
             (!IsValidPhoneNumber(addStudentDTO.PhoneNumber), "Số điện thoại không hợp lệ. Số điện thoại phải có đúng 10 chữ số (ví dụ: 0987654321)."),
             (!IsValidEmail(addStudentDTO.PersonalEmail),"Email không hợp lệ. Vui lòng nhập đúng định dạng (ví dụ: example@gmail.com)."),
             (string.IsNullOrEmpty(addStudentDTO.MajorId),"Vui lòng chọn chuyên ngành cho sinh viên."),
             (addStudentDTO.PasswordHash.Length < 6, "Mật khẩu không thể nhỏ hơn 6 ký tự.")
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
            bool isAdded = _studentRepository.AddNewStudent(studentDTO);
            if (isAdded)
                {
                return new APIResponse
                    {
                    IsSuccess=true,
                    Message="Thêm sinh viên thành công."
                    };
                }
            return new APIResponse
                {
                IsSuccess=false,
                Message="Thêm sinh viên thất bại."
                };
            }
        //Validate Email Format
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
        //Validate Phonenumber Format
        private bool IsValidPhoneNumber(string phoneNumber)
            {
            return phoneNumber.All(char.IsDigit)&&phoneNumber.Length>=10&&phoneNumber.Length<=15&&phoneNumber.StartsWith("0");
            }
        #endregion

        #region Update Student
        /// <summary>
        /// admin Update personal email and phone number for student
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="updateUser"></param>
        /// <returns></returns>
        public APIResponse UpdateStudent(string userId, UpdateStudentDTO updateStudent)
            {
            StudentDTO existingStudent = _studentRepository.GetStudentById(userId);
            #region validation cua Update Student
            var validations = new List<(bool condition, string errorMessage)>
   {
         (existingStudent == null, $"Không tìm thấy sinh viên với mã:{userId}."),
         (!string.IsNullOrEmpty(updateStudent.PersonalEmail) && !IsValidEmail(updateStudent.PersonalEmail), "Định dạng email không hợp lệ."),
         (!string.IsNullOrEmpty(updateStudent.PhoneNumber) && !IsValidPhoneNumber(updateStudent.PhoneNumber), "Định dạng số điện thoại không hợp lệ.")

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
            existingStudent.PersonalEmail=updateStudent.PersonalEmail??existingStudent.PersonalEmail;
            existingStudent.PhoneNumber=updateStudent.PhoneNumber??existingStudent.PhoneNumber;
            existingStudent.MajorId=updateStudent.MajorId??existingStudent.MajorId;
            existingStudent.LastName=updateStudent.LastName??existingStudent.LastName;
            existingStudent.MiddleName=updateStudent.MiddleName??existingStudent.MiddleName;
            existingStudent.FirstName=updateStudent.FirstName??existingStudent.FirstName;
            existingStudent.UserAvartar=updateStudent.UserAvartar??existingStudent.UserAvartar;
            existingStudent.DateOfBirth=updateStudent.DateOfBirth;
            existingStudent.Status=updateStudent.Status;
            existingStudent.Term=updateStudent.Term;
            existingStudent.UpdatedAt=DateTime.Now;
            existingStudent.Address=updateStudent.Address;
            bool isUpdated = _studentRepository.UpdateStudent(existingStudent);
            if (isUpdated)
                {
                return new APIResponse
                    {
                    IsSuccess=true,
                    Message="Cập nhật sinh viên thành công."
                    };
                }
            return new APIResponse
                {
                IsSuccess=false,
                Message="Cập nhật sinh viên thất bại."
                };
            }
        #endregion

        #region UpdateStudentStatus
        /// <summary>
        /// Change status of a student
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public APIResponse UpdateStudentStatus(string userId, int status)
            {
            APIResponse aPIResponse = new APIResponse();

            StudentDTO user = _studentRepository.GetStudentById(userId);
            #region validation cua Update Student Status
            var validations = new List<(bool condition, string errorMessage)>
   {
         (status < 0 || status > 3, "Trạng thái không hợp lệ. Vui lòng nhập từ 0 đến 3"),
         (user == null , $"Không tìm thấy sinh viên với mã:{userId}."),
         (user.RoleId != 5, "Chỉ có thể thay đổi trạng thái cho sinh viên."),
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
            // Update the student's status
            bool isUpdated = _studentRepository.UpdateStudentStatus(userId, status);
            if (isUpdated)
                {
                aPIResponse.IsSuccess=true;
                // Provide the message based on the status value
                switch (status)
                    {
                    case 0:
                        aPIResponse.Message=$"Vô hiệu hóa sinh viên với mã: {userId} thành công.";
                        break;
                    case 1:
                        aPIResponse.Message=$"Đặt trạng thái học tiếp cho sinh viên với mã: {userId} thành công.";
                        break;
                    case 2:
                        aPIResponse.Message=$"Đặt trạng thái bảo lưu cho sinh viên với mã: {userId} thành công.";
                        break;
                    case 3:
                        aPIResponse.Message=$"Đặt trạng thái đã tốt nghiệp cho sinh viên với mã: {userId} thành công.";
                        break;
                    }
                }
            else
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Cập nhật trạng thái sinh viên thất bại.";
                }
            return aPIResponse;
            }
        #endregion
        }
    }
