using ClassBusinessObject;
using ClassBusinessObject.ModelDTOs;
using ClassService.Repositories.StudentInClassRepository;
using Repositories.SubjectRepository;

namespace ClassService.Services.StudentInClassServices
    {
    public class StudentInClassService
        {
        private readonly IStudentInClassRepository _repository;
        public StudentInClassService()
            {
            _repository = new StudentInClassRepository();
            }

        #region Get All Student In Class
        /// <summary>
        ///  Get All Student In Class
        /// </summary>
        /// <returns></returns>
        public APIResponse GetAllStudentInClass()
            {
            APIResponse aPIResponse = new APIResponse();
            List<StudentInClassDTO>? studentInClasses = _repository.GetAllStudentInClass();
            if(studentInClasses == null || studentInClasses.Count == 0)
                {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Không có học sinh nào trong lớp.";
                }
            aPIResponse.Result = studentInClasses;
            return aPIResponse;
            }
        #endregion

        #region Get All Student In Class
        /// <summary>
        ///  Get All Student In Class
        /// </summary>
        /// <returns></returns>
        public APIResponse GetClassSubjectId(string id)
            {
            APIResponse aPIResponse = new APIResponse();
            List<int>? studentInClasses = _repository.GetClassSubjectId(id);
            if(studentInClasses == null || studentInClasses.Count == 0)
                {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Không có học sinh nào trong lớp.";
                }
            aPIResponse.Result = studentInClasses;
            return aPIResponse;
            }
        #endregion

        }
    }
