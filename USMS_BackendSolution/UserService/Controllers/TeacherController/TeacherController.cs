using BusinessObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Services.StudentService;
using UserService.Services.TeacherService;

namespace UserService.Controllers.TeacherController
    {
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
        {
        private readonly TeacherService _service;
        public TeacherController(TeacherService service)
            {
            _service=service;
            }

        #region Get All Teacher
        [HttpGet]
        public APIResponse GetAllTeacher()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse= _service.GetAllTeacher();
            return aPIResponse;
            }
        #endregion

        #region Get All Teacher Available
        [HttpGet("Available/{majorId}")]
        public APIResponse GetAllTeacherAvailableByMajorId(string majorId)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=_service.GetAllTeacherAvailableByMajorId(majorId);
            return aPIResponse;
            }
        #endregion
        }
    }
