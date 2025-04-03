using ClassBusinessObject;
using ClassBusinessObject.ModelDTOs;
using Services.ClassServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClassBusinessObject.AppDBContext;

namespace ClassService.Controllers.ClassSubjectController
    {
    [Route("api/[controller]")]
    [ApiController]
    public class ClassSubjectController : ControllerBase
        {
        private readonly ClassSubjectService _classSubjectService;
        public ClassSubjectController(ClassSubjectService classSubjectService)
            {
            _classSubjectService = classSubjectService;
            }

        // GET: api/ClassSubject
        [HttpGet]
        public async Task<IActionResult> GetAllClassSubject()
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse = await _classSubjectService.GetAllClassSubject();
            return Ok(aPIResponse);
            }

        // GET: api/ClassSubject/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClassSubjectById(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _classSubjectService.GetClassSubjectById(id);
            return Ok(aPIResponse);
            }

        // GET: api/ClassSubject/1
        [HttpGet("classId/{classId}")]
        public async Task<IActionResult> GetClassSubjectByClassId(string classId)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _classSubjectService.GetClassSubjectByClassId(classId);
            return Ok(aPIResponse);
            }

        // POST: api/ClassSubject
        [HttpPost]
        public async Task<IActionResult> AddNewClassSubject(AddUpdateClassSubjectDTO classSubjectDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _classSubjectService.AddNewClassSubject(classSubjectDTO);
            return Ok(aPIResponse);
            }

        // PUT: api/ClassSubject
        [HttpPut]
        public async Task<IActionResult> UpdateClassSubject(AddUpdateClassSubjectDTO classSubjectDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _classSubjectService.UpdateClassSubject(classSubjectDTO);
            return Ok(aPIResponse);
            }

        [HttpPut("ChangeStatus/{id}")]
        public async Task<IActionResult> ChangeStatusClassSubject(int id)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _classSubjectService.ChangeStatusClassSubject(id);
            return Ok(aPIResponse);
            }

        [HttpGet("ClassSubject")]
        public async Task<IActionResult> GetClassSubjects(string majorId, string classId, int term)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse =await _classSubjectService.GetClassSubjectByMajorIdClassIdSubjectId(majorId, classId, term);
            return Ok(aPIResponse);
            }

        #region Lấy danh sách ClassId dựa vào MajorId
        /// <summary>
        /// Endpoint GET: lấy danh sách ClassId (phân biệt) dựa theo MajorId.
        /// Ví dụ: GET /api/Class/ClassIdsByMajorId?majorId=IT
        /// </summary>
        /// <param name="majorId">Mã chuyên ngành</param>
        /// <returns>APIResponse chứa danh sách ClassId</returns>
        [HttpGet("ClassIdsByMajorId/{majorId}")]
        public async Task<IActionResult> GetClassIdsByMajorId(string majorId)
            {
            var aPIResponse = await _classSubjectService.GetClassIdsByMajorId(majorId);
            return Ok(aPIResponse);
            }
        #endregion

        #region Get SubjectIds by MajorId and SemesterId
        [HttpGet("SubjectIds/{majorId}/{semesterId}")]
        public async Task<IActionResult> GetSubjectIdsByMajorIdAndSemesterId(string majorId, string semesterId)
            {
            var aPIResponse = await _classSubjectService.GetSubjectIdsByMajorIdAndSemesterId(majorId, semesterId);
            return Ok(aPIResponse);
            }
        #endregion

        [HttpPut("ChangeSelectStatus")]
        public async Task<IActionResult> ChangeStatusClassSubject(List<int> ids, int status)
            {
            APIResponse aPIResponse = new APIResponse();
            aPIResponse=await _classSubjectService.ChangeClassStatusSelected(ids,status);
            return Ok(aPIResponse);
            }
        }

    }
