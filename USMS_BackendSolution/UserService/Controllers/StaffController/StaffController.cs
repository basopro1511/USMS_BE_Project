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
        public async Task<IActionResult> GetAllStaff()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.GetAllStaff();
            return Ok(aPIResponse);
            }
        #endregion

        #region Add New Staff
        [HttpPost]
        public async Task<IActionResult> AddNewStaff(UserDTO userDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.AddNewStaff(userDTO);
            return Ok(aPIResponse);
            }
        #endregion

        #region Update Staff
        [HttpPut]
        public async Task<IActionResult> UpdateStaff(UserDTO userDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.UpdateStaff(userDTO);
            return Ok(aPIResponse);
            }
        #endregion
        #region Import From Excel
        [HttpPost("import")]
        public async Task<IActionResult> StaffImports(IFormFile file)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.ImportStaffsFromExcel(file);
            return Ok(aPIResponse);
            }
        #endregion

        #region Update Status
        [HttpPut("ChangeStatus")]
        public async Task<IActionResult> ChangeStudentStatus(List<string> userIds, int status)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _service.ChangeUsersStatusSelected(userIds, status);
            return Ok(aPIResponse);
            }
        #endregion
        }
    }
