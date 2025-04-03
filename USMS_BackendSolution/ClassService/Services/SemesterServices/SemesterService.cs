using ClassBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using Repositories.SemesterRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassBusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;

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
        public async Task<APIResponse> GetAllSemesters()
        {
            APIResponse response = new APIResponse();
            List<Semester> semesters =await _semesterRepository.GetAllSemesters();
            if (semesters == null || !semesters.Any())
            {
                response.IsSuccess = false;
                response.Message = "Không có kỳ học nào!";
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
        public async Task<APIResponse> GetSemesterById(string id)
        {
            APIResponse response = new APIResponse();
            Semester semester =await _semesterRepository.GetSemesterById(id);
            if (semester == null)
            {
                response.IsSuccess = false;
                response.Message = $" Kỳ học với mã: {id} không tồn tại. Vui lòng kiểm tra lại";
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
        public async Task<APIResponse> AddSemester(SemesterDTO semesterDto)
        {
            APIResponse response = new APIResponse();
            Semester existingSemester =await _semesterRepository.GetSemesterById(semesterDto.SemesterId);
            if (existingSemester != null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = $" Kỳ học với mã: {semesterDto.SemesterId} đã tồn tại. Vui lòng kiểm tra lại"
                };
            }
            Semester semester = new Semester();
            semester.CopyProperties(semesterDto);
            bool isAdded =await _semesterRepository.AddNewSemester(semester);
            if (isAdded)
            {
                response.IsSuccess = true;
                response.Message = "Thêm kỳ học thành công";
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Thêm kỳ học thất bại.";
            }
            return response;
        }
        #endregion

        #region Update Semester
        /// <summary>
        /// Update an existing Semester
        /// </summary>
        /// <param name="semesterDto"></param>
        public async Task<APIResponse> UpdateSemester(SemesterDTO semesterDto)
        {
            APIResponse response = new APIResponse();
            Semester existingSemester =await _semesterRepository.GetSemesterById(semesterDto.SemesterId);
            if (existingSemester == null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Mã kỳ học được cung cấp không tồn tại!"
                };
            }
            if (semesterDto.EndDate <= semesterDto.StartDate)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Ngày bắt đầu không thể diển ra sau ngày kết thúc."
                };
                }
            Semester semester = new Semester();
            semester.CopyProperties(semesterDto);
            bool isUpdated =await _semesterRepository.UpdateSemester(semester);
            if (isUpdated)
            {
                response.IsSuccess = true;
                response.Message = $"Kỳ học với mã: {semesterDto.SemesterId} đã được cập nhật thành công.";
            }
            else
            {
                response.IsSuccess = false;
                response.Message = $" Cập nhật kỳ học với mã: {semesterDto.SemesterId} thất bại.";
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
        public async Task<APIResponse> ChangeStatusSemester(string id, int status)
        {
            APIResponse response = new APIResponse();
            Semester existingSemester =await _semesterRepository.GetSemesterById(id);
            if (existingSemester == null)
            {
                return new APIResponse
                {
                    IsSuccess = false,
                    Message = "Mã kỳ học được cung cấp không tồn tại!"
                };
            }
            // Validate status input
            if (status < 0 || status > 2)
            {
                response.IsSuccess = false;
                response.Message = "Trạng thái không hợp lệ. Vui lòng nhập trạng thái từ 0 đến 2.";
                return response;
            }
            // Update the semester's status
            bool isUpdated = await _semesterRepository.ChangeStatusSemester(id, status);
            if (isUpdated)
            {
                response.IsSuccess = true;
                // Provide the message based on the status value
                switch (status)
                {
                    case 2:
                        response.Message = $" Kỳ học với mã: {id} đã kết thúc.";
                        break;
                    case 1:
                        response.Message = $" Kỳ học với mã: {id} đang diễn ra.";
                        break;
                    case 0:
                        response.Message = $" Kỳ học với mã: {id} chưa bắt đầu.";
                        break;
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Cập nhật kỳ học thất bại.";
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
