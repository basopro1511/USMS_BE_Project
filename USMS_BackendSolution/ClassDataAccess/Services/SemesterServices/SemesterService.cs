using ClassBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerDataAccess.Repositories.SemesterRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchedulerBusinessObject.Services
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
        public async Task<APIResponse> GetAllSemestersAsync()
        {
            APIResponse response = new APIResponse();
            var semesters = await _semesterRepository.GetAllSemestersAsync();
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
        public async Task<APIResponse> GetSemesterByIdAsync(string id)
        {
            APIResponse response = new APIResponse();
            var semester = await _semesterRepository.GetSemesterByIdAsync(id);
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
        public async Task<APIResponse> AddSemesterAsync(SemesterDTO semesterDto)
        {
            APIResponse response = new APIResponse();
            var existingSemesters = await _semesterRepository.GetActiveSemestersAsync();
            if (existingSemesters.Any(s => s.SemesterName == semesterDto.SemesterName))
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "A semester with the same name already exists."
                };
            }
            try
            {
                await _semesterRepository.AddSemesterAsync(semesterDto);
                response.IsSuccess = true;
                response.Message = "Semester added successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Failed to add semester: {ex.Message}";
            }
            return response;
        }
        #endregion
        #region Update Semester
        /// <summary>
        /// Update an existing Semester
        /// </summary>
        /// <param name="semesterDto"></param>
        public async Task<APIResponse> UpdateSemesterAsync(SemesterDTO semesterDto)
        {
            APIResponse response = new APIResponse();
            var existingSemester = await _semesterRepository.GetSemesterByIdAsync(semesterDto.SemesterId);
            if (existingSemester == null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Semester with the given ID does not exist."
                };
            }
            try
            {
                await _semesterRepository.UpdateSemesterAsync(semesterDto);
                response.IsSuccess = true;
                response.Message = "Semester updated successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Failed to update semester: {ex.Message}";
            }
            return response;
        }
        #endregion
        #region Delete Semester
        /// <summary>
        /// Delete a Semester by ID
        /// </summary>
        /// <param name="id"></param>
        public async Task<APIResponse> DeleteSemesterAsync(string id)
        {
            APIResponse response = new APIResponse();
            var existingSemester = await _semesterRepository.GetSemesterByIdAsync(id);
            if (existingSemester == null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Semester with the given ID does not exist."
                };
            }
            try
            {
                await _semesterRepository.DeleteSemesterAsync(id);
                response.IsSuccess = true;
                response.Message = "Semester deleted successfully.";
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Failed to delete semester: {ex.Message}";
            }
            return response;
        }
        #endregion
        #region Get Active Semesters
        /// <summary>
        /// Retrieve all active Semesters
        /// </summary>
        /// <returns>List of active Semesters</returns>
        public async Task<APIResponse> GetActiveSemestersAsync()
        {
            APIResponse response = new APIResponse();
            var activeSemesters = await _semesterRepository.GetActiveSemestersAsync();
            if (activeSemesters == null || !activeSemesters.Any())
            {
                response.IsSuccess = false;
                response.Message = "No active semesters found!";
            }
            response.Result = activeSemesters;
            return response;
        }
        #endregion
    }
}
