using ClassBusinessObject;
using ClassBusinessObject.ModelDTOs;
using ClassService.Repositories.StudentInClassRepository;
using Repositories.SubjectRepository;

namespace ClassService.Services.StudentInClassServices
    {
    public class StudentInClassService
        {
        private readonly IStudentInClassRepository _repository;
        public StudentInClassService() {
            _repository=new StudentInClassRepository();
            }

        #region Get All Student In Class
        /// <summary>
        ///  Get All Student In Class
        /// </summary>
        /// <returns></returns>
        public APIResponse GetAllStudentInClass() {
            APIResponse aPIResponse = new APIResponse();
            List<StudentInClassDTO>? studentInClasses = _repository.GetAllStudentInClass();
            if(studentInClasses==null||studentInClasses.Count==0) {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không có học sinh nào trong lớp.";
                }
            aPIResponse.Result=studentInClasses;
            return aPIResponse;
            }
        #endregion

        #region Get All Student In Class
        /// <summary>
        ///  Get All Student In Class
        /// </summary>
        /// <returns></returns>
        public APIResponse GetStudentInClassByStudentId(string student) {
            APIResponse aPIResponse = new APIResponse();
            StudentInClassDTO studentInClasses = _repository.GetStudentInClassByStudentId(student);
            if(studentInClasses==null) {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không tìm thấy học sinh này trong lớp.";
                }
            aPIResponse.Result=studentInClasses;
            return aPIResponse;
            }
        #endregion

        #region Get Class SubjectId by studentId
        /// <summary>
        /// Class SubjectId by studentId
        /// </summary>
        /// <returns>a list int of classSubjectId</returns>
        public APIResponse GetClassSubjectId(string id) {
            APIResponse aPIResponse = new APIResponse();
            List<int>? studentInClasses = _repository.GetClassSubjectId(id);
            if(studentInClasses==null||studentInClasses.Count==0) {
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
        public APIResponse AddStudentToClass(StudentInClassDTO studentInClassDTO) {
            APIResponse aPIResponse = new APIResponse();
            var checkExist = _repository.GetStudentInClassByStudentId(studentInClassDTO.StudentId);
            if(checkExist!=null) {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Sinh viên này đã tồn tại trong lớp học";
                }
            bool success = _repository.AddStudentToClass(studentInClassDTO);
            if(success) {
                return new APIResponse {
                    IsSuccess=true,
                    Message="Thêm sinh viên vào lớp thành công"
                    };
                }
            return new APIResponse {
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
        public APIResponse AddMultipleStudentsToClass(List<StudentInClassDTO> studentsInClassDTO) {
            APIResponse aPIResponse = new APIResponse();
            foreach(var item in studentsInClassDTO) {
                var checkExist = _repository.GetStudentInClassByStudentId(item.StudentId);
                if(checkExist!=null) {
                    aPIResponse.IsSuccess=false;
                    aPIResponse.Message="Sinh viên với Id = " + item.StudentId +" đã tồn tại trong lớp học!";
                    return aPIResponse;
                    }
                }
            bool success = _repository.AddMultipleStudentsToClass(studentsInClassDTO);
            aPIResponse.IsSuccess=success;
            aPIResponse.Message=success ? "Thêm danh sách sinh viên thành công" : "Thêm danh sách sinh viên thất bại";
            return aPIResponse;
            }
        #endregion

        #region Update Student in Class
        /// <summary>
        /// Update a student's class information
        /// </summary>
        /// <param name="studentInClassDTO"></param>
        /// <returns>APIResponse indicating success</returns>
        public APIResponse UpdateStudentInClass(StudentInClassDTO studentInClassDTO) {
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
        public APIResponse DeleteStudentFromClass(int studentClassId) {
            APIResponse aPIResponse = new APIResponse();
            bool success = _repository.DeleteStudentFromClass(studentClassId);
            aPIResponse.IsSuccess=success;
            aPIResponse.Message=success ? "Xóa sinh viên khỏi lớp thành công" : "Xóa thất bại";
            return aPIResponse;
            }
        #endregion
        }
    }
//Copy Paste 
#region Copy + Pase  
#endregion