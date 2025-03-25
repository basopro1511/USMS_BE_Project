using BusinessObject;
using BusinessObject.ModelDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserService.Services.StaffServices;
using UserService.Services.TeacherService;

namespace UserService.Controllers.StaffController
    {
    [Route("api/[controller]")]
    [ApiController]
    public class StaffController : ControllerBase
        {
        private readonly StaffService _service;
        public StaffController(StaffService service)
            {
            _service=service;
            }

        #region Get All Staff
        [HttpGet]
        public async Task<APIResponse> GetAllStaff()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetAllStaff();
            return aPIResponse;
            }
        #endregion

        #region Add New Staff
        [HttpPost]
        public async Task<APIResponse> AddNewStaff(UserDTO userDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.AddNewStaff(userDTO);
            return aPIResponse;
            }
        #endregion

        #region Update Staff
        [HttpPut]
        public async Task<APIResponse> UpdateStaff(UserDTO userDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.UpdateStaff(userDTO);
            return aPIResponse;
            }
        #endregion
        #region Import From Excel
        [HttpPost("import")]
        public async Task<APIResponse> StaffImports(IFormFile file)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.ImportStaffsFromExcel(file);
            return aPIResponse;
            }
        #endregion
        }
    }
