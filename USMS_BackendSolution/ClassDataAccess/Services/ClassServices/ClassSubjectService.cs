using ClassBusinessObject;
using ClassBusinessObject.ModelDTOs;
using ClassDataAccess.Repositories.ClassSubjectRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassDataAccess.Services.ClassServices
{
    public class ClassSubjectService
    {
        private readonly IClassRepository _classRepository;
        public ClassSubjectService()
        {
            _classRepository = new ClassRepository();
        }
        //Copy Paste 
        #region Copy + Pase  
        #endregion

        #region Get All Schedule
        /// <summary>
        /// Retrive all ClassSubject in Database
        /// </summary>
        /// <returns>a list of all Class Subject in DB</returns>
        public APIResponse GetAllClassSubject()
        {
            APIResponse aPIResponse = new APIResponse();
            List<ClassSubjectDTO> classSubjects = _classRepository.GetAllClassSubjects();
            if (classSubjects == null)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Don't have any Class Subject available!";
            }
            aPIResponse.Result = classSubjects;
            return aPIResponse;
        }
        #endregion

        #region Get ClassSubject By ClassSubjectId 
        /// <summary>
        /// Retrive a ClassSubject with ClassSubjectId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a ClassSubject by Id</returns>
        public APIResponse GetClassSubjectById(int id)
        {
            APIResponse aPIResponse = new APIResponse();
            ClassSubjectDTO classSubject = _classRepository.GetClassSubjectById(id);
            if (classSubject == null)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "ClassSubject with Id: " + id + " is not found";
            }
            aPIResponse.Result = classSubject;
            return aPIResponse;
        }
        #endregion

        #region Get ClassSubject By ClassId
        /// <summary>
        /// Retrive list ClassSubjects by ClassId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a list ClassSubjects by ClassId </returns>
        public APIResponse GetClassSubjectByClassId(string id)
        {
            APIResponse aPIResponse = new APIResponse();
            List<ClassSubjectDTO> classSubjects = _classRepository.GetClassSubjectByClassId(id);
            if (classSubjects == null || classSubjects.Count == 0)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "ClassSubjects with Id: " + id + " is not found";
            }
            aPIResponse.Result = classSubjects;
            return aPIResponse;
        }
        #endregion
    }
}
