using ClassBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using Repositories.SemesterRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.SemesterServices
{
    public class SemesterService
    {
        private readonly ISemesterRepository _semesterRepository;

        public SemesterService()
        {
            _semesterRepository = new SemesterRepository();
        }

        #region Get All Semesters
        /// <summary>
        /// Retrieve all Semesters in Database
        /// </summary>
        /// <returns>A list of all Semesters in DB</returns>
        public APIResponse GetAllSemesters()
        {
            APIResponse response = new APIResponse();
            List<SemesterDTO> semesters = _semesterRepository.GetAllSemesters();
            if (semesters == null || !semesters.Any())
            {
                response.IsSuccess = false;
                response.Message = "No semesters found!";
            }
            response.Result = semesters;
            return response;
        }
        #endregion

        #region Get Semester By ID
        /// <summary>
        /// Retrieve a Semester with SemesterId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>A Semester by ID</returns>
        public APIResponse GetSemesterById(string id)
        {
            APIResponse response = new APIResponse();
            SemesterDTO semester = _semesterRepository.GetSemesterById(id);
            if (semester == null)
            {
                response.IsSuccess = false;
                response.Message = $"Semester with ID: {id} not found.";
            }
            response.Result = semester;
            return response;
        }
        #endregion

        #region Add New Semester
        /// <summary>
        /// Add a new Semester to the database
        /// </summary>
        /// <param name="semesterDto"></param>
        public APIResponse AddSemester(SemesterDTO semesterDto)
        {
            APIResponse response = new APIResponse();
            SemesterDTO existingSemester = _semesterRepository.GetSemesterById(semesterDto.SemesterId);
            if (existingSemester != null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "A semester with the same name already exists."
                };
            }
            bool isAdded = _semesterRepository.AddNewSemester(semesterDto);
            if (isAdded)
            {
                response.IsSuccess = true;
                response.Message = "Semester added successfully.";
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Failed to add semester.";
            }
            return response;
        }
        #endregion

        #region Update Semester
        /// <summary>
        /// Update an existing Semester
        /// </summary>
        /// <param name="semesterDto"></param>
        public APIResponse UpdateSemester(SemesterDTO semesterDto)
        {
            APIResponse response = new APIResponse();
            SemesterDTO existingSemester = _semesterRepository.GetSemesterById(semesterDto.SemesterId);
            if (existingSemester == null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Semester with the given ID does not exist."
                };
            }
            bool isUpdated = _semesterRepository.UpdateSemester(semesterDto);
            if (isUpdated)
            {
                response.IsSuccess = true;
                response.Message = "Semester updated successfully.";
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Failed to update semester.";
            }
            return response;
        }
        #endregion

        #region Change Semester Status
        /// <summary>
        /// Change the status of a Semester
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public APIResponse ChangeStatusSemester(string id)
        {
            APIResponse response = new APIResponse();
            SemesterDTO existingSemester = _semesterRepository.GetSemesterById(id);
            if (existingSemester == null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Semester with the given ID does not exist."
                };
            }
            bool isSuccess = _semesterRepository.ChangeStatusSemester(id);
            if (isSuccess)
            {
                response.IsSuccess = true;
                response.Message = "Semester status changed successfully.";
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Failed to change semester status.";
            }
            return response;
        }
        #endregion

        #region Delete Semester
        /// <summary>
        /// Delete a Semester by ID
        /// </summary>
        /// <param name="id"></param>
        //public APIResponse DeleteSemester(string id)
        //{
        //    APIResponse response = new APIResponse();
        //    SemesterDTO existingSemester = _semesterRepository.GetSemesterById(id);
        //    if (existingSemester == null)
        //    {
        //        return new APIResponse
        //        {
        //            IsSuccess = false,
        //            Message = "Semester with the given ID does not exist."
        //        };
        //    }
        //    bool isDeleted = _semesterRepository.DeleteSemester(id);
        //    if (isDeleted)
        //    {
        //        response.IsSuccess = true;
        //        response.Message = "Semester deleted successfully.";
        //    }
        //    else
        //    {
        //        response.IsSuccess = false;
        //        response.Message = "Failed to delete semester.";
        //    }
        //    return response;
        //}
        #endregion
    }
}
