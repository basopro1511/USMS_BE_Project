using Azure;
using BusinessObject;
using BusinessObject.ModelDTOs;
using System.Net.Http;
using System.Text.Json;
using UserService.Repository.TeacherRepository;
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
        public APIResponse GetAllTeacher()
            {
            APIResponse aPIResponse = new APIResponse();
            List<UserDTO> teachers = _repository.GetAllTeacher();
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
        public APIResponse GetAllTeacherAvailableByMajorId(string majorId)
            {
            APIResponse aPIResponse = new APIResponse();
            List<UserDTO> teachers = _repository.GetAllTeacherAvailableByMajorId(majorId);
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

        }
    }
