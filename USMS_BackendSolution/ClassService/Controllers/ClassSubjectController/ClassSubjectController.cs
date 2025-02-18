using ClassBusinessObject;
using ClassBusinessObject.ModelDTOs;
using Services.ClassServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassBusinessObject.AppDBContext;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Newtonsoft.Json;
using Authorization.Services;

namespace ClassService.Controllers.ClassSubjectController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClassSubjectController : ControllerBase
    {
        private readonly ClassSubjectService _classSubjectService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorizationServicee _authService;
        public ClassSubjectController(ClassSubjectService classSubjectService, IHttpContextAccessor httpContextAccessor, IAuthorizationServicee authService)
        {
            _classSubjectService = classSubjectService;
            _httpContextAccessor = httpContextAccessor;
            _authService = authService;
        }
        // GET: api/ClassSubject
        [HttpGet]
        public async Task<APIResponse> GetAllClassSubject()
        {
            var authResponse = await _authService.ValidateUserRole(new[] { "1" });
            if (!authResponse.IsSuccess)
            {
                return new ClassBusinessObject.APIResponse
                {
                    IsSuccess = authResponse.IsSuccess,
                    Message = authResponse.Message,
                    Errors = authResponse.Errors,
                    Result = authResponse.Result
                };
            }
            return _classSubjectService.GetAllClassSubject();
        }
        // GET: api/ClassSubject/1
        [HttpGet("{id}")]
        public async Task<APIResponse> GetClassSubjectById(int id)
        {
            var authResponse = await _authService.ValidateUserRole(new[] { "1" });
            if (!authResponse.IsSuccess)
            {
                return new ClassBusinessObject.APIResponse
                {
                    IsSuccess = authResponse.IsSuccess,
                    Message = authResponse.Message,
                    Errors = authResponse.Errors,
                    Result = authResponse.Result
                };
            }
            return _classSubjectService.GetClassSubjectById(id);
        }
        // GET: api/ClassSubject/1
        [HttpGet("classId/{classId}")]
        public async Task<APIResponse> GetClassSubjectByClassId(string classId)
        {
            var authResponse = await _authService.ValidateUserRole(new[] { "1" });
            if (!authResponse.IsSuccess)
            {
                return new ClassBusinessObject.APIResponse
                {
                    IsSuccess = authResponse.IsSuccess,
                    Message = authResponse.Message,
                    Errors = authResponse.Errors,
                    Result = authResponse.Result
                };
            }
            return _classSubjectService.GetClassSubjectByClassId(classId);
        }
        // POST: api/ClassSubject
        [HttpPost]
        public async Task<APIResponse> AddNewClassSubject(AddUpdateClassSubjectDTO classSubjectDTO)
        {
            // Kiểm tra quyền Admin (role = "1")
            var authResponse = await _authService.ValidateUserRole(new[] { "1" });
            if (!authResponse.IsSuccess)
            {
                return new APIResponse
                {
                    IsSuccess = authResponse.IsSuccess,
                    Message = authResponse.Message,
                    Errors = authResponse.Errors,
                    Result = authResponse.Result
                };
            }
            // Nếu có quyền Admin thì thực hiện thêm mới
            return _classSubjectService.AddNewClassSubject(classSubjectDTO);
        }
        // PUT: api/ClassSubject
        [HttpPut]
        public async Task<APIResponse> UpdateClassSubject(AddUpdateClassSubjectDTO classSubjectDTO)
        {
            var authResponse = await _authService.ValidateUserRole(new[] { "1" });
            if (!authResponse.IsSuccess)
            {
                return new ClassBusinessObject.APIResponse
                {
                    IsSuccess = authResponse.IsSuccess,
                    Message = authResponse.Message,
                    Errors = authResponse.Errors,
                    Result = authResponse.Result
                };
            }
            return _classSubjectService.UpdateClassSubject(classSubjectDTO);
        }
        [HttpPut("ChangeStatus/{id}")]
        public async Task<APIResponse> ChangeStatusClassSubject(int id)
        {
            var authResponse = await _authService.ValidateUserRole(new[] { "1" });
            if (!authResponse.IsSuccess)
            {
                return new ClassBusinessObject.APIResponse
                {
                    IsSuccess = authResponse.IsSuccess,
                    Message = authResponse.Message,
                    Errors = authResponse.Errors,
                    Result = authResponse.Result
                };
            }
            return _classSubjectService.ChangeStatusClassSubject(id);
        }
        [HttpGet("ClassSubject")]
        public async Task<APIResponse> GetClassSubjects(string majorId, string classId, int term)
        {
            var authResponse = await _authService.ValidateUserRole(new[] { "1" });
            if (!authResponse.IsSuccess)
            {
                return new ClassBusinessObject.APIResponse
                {
                    IsSuccess = authResponse.IsSuccess,
                    Message = authResponse.Message,
                    Errors = authResponse.Errors,
                    Result = authResponse.Result
                };
            }
            return _classSubjectService.GetClassSubjects(majorId, classId, term);
        }

        #region Lấy danh sách ClassId dựa vào MajorId
        /// <summary>
        /// Endpoint GET: lấy danh sách ClassId (phân biệt) dựa theo MajorId.
        /// Ví dụ: GET /api/Class/ClassIdsByMajorId?majorId=IT
        /// </summary>
        /// <param name="majorId">Mã chuyên ngành</param>
        /// <returns>APIResponse chứa danh sách ClassId</returns>
        [HttpGet("ClassIdsByMajorId/{majorId}")]
        public APIResponse GetClassIdsByMajorId(string majorId)
        {
            var response = _classSubjectService.GetClassIdsByMajorId(majorId);
            return response;
        }
        #endregion
    }
}
