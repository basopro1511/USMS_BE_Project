using ClassBusinessObject;
using ClassBusinessObject.ModelDTOs;
using ClassBusinessObject.Models;
using Repositories.ClassSubjectRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.ClassServices
{
    public class ClassSubjectService
    {
        private readonly IClassRepository _classRepository;
        public ClassSubjectService()
        {
            _classRepository = new ClassRepository();
        }

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

        #region Add New ClassSubject
        /// <summary>
        /// Add New ClassSubject to databse
        /// </summary>
        /// <param name="ClassSubject"></param>
        public APIResponse AddNewClassSubject(AddUpdateClassSubjectDTO classSubject)
        {
            APIResponse aPIResponse = new APIResponse();
            ClassSubjectDTO existingClassSubject = _classRepository.GetExistingClassSubject(classSubject.ClassId,classSubject.SubjectId,classSubject.SemesterId);
            if (existingClassSubject != null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Class Subject with the given ClassId, SubjectId, SemesterId already exists."
                };
            }
            bool isAdded = _classRepository.AddNewClassSubject(classSubject);
            if (isAdded)
            {
                return new APIResponse
                {
                    IsSuccess = true,
                    Message = "Class Subject added successfully."
                };
            }
            return new APIResponse
            {
                IsSuccess = false,
                Message = "Failed to add Class Subject."
            };
        }
        #endregion

        #region Update ClassSubject
        /// <summary>
        /// Udate ClassSubject in databse
        /// </summary>
        /// <param name="ClassSubject"></param>
        public APIResponse UpdateClassSubject(AddUpdateClassSubjectDTO classSubject)
        {
            APIResponse aPIResponse = new APIResponse();
            ClassSubjectDTO existingClassSubject = _classRepository.GetClassSubjectById(classSubject.ClassSubjectId);
            if (existingClassSubject == null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Class Subject with the given ID is not exists."
                };
            }
            bool isAdded = _classRepository.UpdateClassSubject(classSubject);
            if (isAdded)
            {
                return new APIResponse
                {
                    IsSuccess = true,
                    Message = "Class Subject updated successfully."
                };
            }
            return new APIResponse
            {
                IsSuccess = false,
                Message = "Failed to updated Class Subject."
            };
        }
        #endregion

        #region Change Class Subject Status
        /// <summary>
        /// Change Status of Class Subject
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public APIResponse ChangeStatusClassSubject(int id)
        {
            var response = new APIResponse();
            ClassSubjectDTO existingClassSubject = _classRepository.GetClassSubjectById(id);
            if (existingClassSubject == null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Class Subject with the given ID is not exists."
                };
            }
            bool isSuccess = _classRepository.ChangeStatusClassSubject(id);
            if (isSuccess)
            {
                return new APIResponse
                {
                    IsSuccess = true,
                    Message = "Class Subject change status successfully."
                };
            }
            return new APIResponse
            {
                IsSuccess = false,
                Message = "Failed to change status of Class Subject."
            };
        }
        #endregion
    }
}
//Copy Paste 
#region Copy + Pase  
#endregion