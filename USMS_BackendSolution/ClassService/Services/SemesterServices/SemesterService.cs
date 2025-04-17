using ClassBusinessObject;
using SchedulerBusinessObject.ModelDTOs;
using Repositories.SemesterRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassBusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;
using OfficeOpenXml;
using System.Globalization;

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
                response.Message = "Không có học kỳ nào!";
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
                response.Message = $" Học kỳ với mã: {id} không tồn tại. Vui lòng kiểm tra lại";
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
                    Message = $" Học kỳ với mã: {semesterDto.SemesterId} đã tồn tại. Vui lòng kiểm tra lại"
                };
            }
            Semester semester = new Semester();
            semester.CopyProperties(semesterDto);
            bool isAdded =await _semesterRepository.AddNewSemester(semester);
            if (isAdded)
            {
                response.IsSuccess = true;
                response.Message = "Thêm học kỳ thành công";
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Thêm học kỳ thất bại.";
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
                    Message = "Mã học kỳ được cung cấp không tồn tại!"
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
                response.Message = $"Học kỳ với mã: {semesterDto.SemesterId} đã được cập nhật thành công.";
            }
            else
            {
                response.IsSuccess = false;
                response.Message = $" Cập nhật học kỳ với mã: {semesterDto.SemesterId} thất bại.";
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
                    Message = "Mã học kỳ được cung cấp không tồn tại!"
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
                        response.Message = $" Học kỳ với mã: {id} đã kết thúc.";
                        break;
                    case 1:
                        response.Message = $" Học kỳ với mã: {id} đang diễn ra.";
                        break;
                    case 0:
                        response.Message = $" Học kỳ với mã: {id} chưa bắt đầu.";
                        break;
                }
            }
            else
            {
                response.IsSuccess = false;
                response.Message = "Cập nhật học kỳ thất bại.";
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

        #region Change Semester Status Selected 
        /// <summary>
        /// Change Semester status
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<APIResponse> ChangeSemesterStatusSelected(List<string> Ids, int status)
            {
            APIResponse aPIResponse = new APIResponse();
            if (Ids==null||!Ids.Any())
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Danh sách học kỳ không hợp lệ.";
                return aPIResponse;
                }
            bool isSuccess = await _semesterRepository.ChangeSemesterStatusSelected(Ids, status);
            if (isSuccess)
                {
                aPIResponse.IsSuccess=true;
                switch (status)
                    {
                    case 0:
                        aPIResponse.Message="Đã thay đổi trạng thái các học kỳ thành 'Chưa bắt đầu'.";
                        break;
                    case 1:
                        aPIResponse.Message="Đã thay đổi trạng thái các học kỳ thành 'Đang diễn ra'.";
                        break;
                    case 2:
                        aPIResponse.Message="Đã thay đổi trạng thái các học kỳ thành 'Đã kết thúc'.";
                        break;
                    default:
                        aPIResponse.Message="Trạng thái không hợp lệ.";
                        break;
                    }
                }
            return aPIResponse;
            }
        #endregion

        #region Export Form Add Semester
        /// <summary>
        /// Export empty form for add model
        /// </summary>
        /// <returns></returns>
        public Task<byte[]> ExportFormAddSemester()
            {
            ExcelPackage.LicenseContext=LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
                {
                var worksheet = package.Workbook.Worksheets.Add("Semesters");
                // Header
                worksheet.Cells[1, 1].Value="STT";
                worksheet.Cells[1, 2].Value="Mã học kỳ";
                worksheet.Cells[1, 3].Value="Tên học kỳ";
                worksheet.Cells[1, 4].Value="Thời gian bắt đầu";
                worksheet.Cells[1, 5].Value="Thời gian kết thúc";
                // Gán công thức tự động tăng STT từ dòng 2 đến 1000
                for (int row = 2; row<=1000; row++)
                    {
                    worksheet.Cells[row, 1].Formula=$"=ROW()-1";
                    }
                worksheet.Cells.AutoFitColumns();
                return Task.FromResult(package.GetAsByteArray());
                }
            }
        #endregion

        #region Export Semester Information
        /// <summary>
        /// Export Room Information
        /// </summary>
        /// <param name="majorId"></param>
        /// <returns></returns>
        public async Task<byte[]> ExportSemestersToExcel(int? status)
            {
            var models = await _semesterRepository.GetAllSemesters();
            if (status.HasValue)
                models=models.Where(s => s.Status==status.Value).ToList();
            ExcelPackage.LicenseContext=OfficeOpenXml.LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
                {
                var worksheet = package.Workbook.Worksheets.Add("Semester");
                // Header
                worksheet.Cells[1, 1].Value="STT";
                worksheet.Cells[1, 2].Value="Mã học kỳ";
                worksheet.Cells[1, 3].Value="Tên học kỳ";
                worksheet.Cells[1, 4].Value="Trạng thái";
                worksheet.Cells[1, 5].Value="Thời gian bắt đầu";
                worksheet.Cells[1, 6].Value="Thời gian kết thúc";
                int row = 2;
                int stt = 1;
                foreach (var s in models)
                    {
                    worksheet.Cells[row, 1].Value=stt++;
                    worksheet.Cells[row, 2].Value=s.SemesterId;
                    worksheet.Cells[row, 3].Value=s.SemesterName;
                    worksheet.Cells[row, 4].Value=s.Status==1 ? "Đang khả dụng" : s.Status==0 ? "Vô hiệu hóa" : "Đã kết thúc";
                    worksheet.Cells[row, 5].Value=s.StartDate.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 6].Value=s.EndDate.ToString("dd/MM/yyyy");
                    row++;
                    }
                return package.GetAsByteArray();
                }
            }
        #endregion

        #region Import Semester from Excels
        /// <summary>
        /// Import Semester Form Excel
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<APIResponse> ImportSemestersFromExcel(IFormFile file)
            {
            try
                {
                if (file==null||file.Length==0)
                    {
                    return new APIResponse { IsSuccess=false, Message="File không hợp lệ." };
                    }
                var models = new List<Semester>();
                ExcelPackage.LicenseContext=OfficeOpenXml.LicenseContext.NonCommercial;
                using (var stream = new MemoryStream())
                    {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                        {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        int rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row<=rowCount; row++)
                            {
                            if (string.IsNullOrWhiteSpace(worksheet.Cells[row, 2].Text))
                                {
                                break;
                                }
                            DateOnly dtStartDate;
                            if (worksheet.Cells[row, 4].Value is DateTime dt)
                                {
                                dtStartDate=DateOnly.FromDateTime(dt);
                                }
                            else
                                {
                                string dobText = worksheet.Cells[row,4].Text.Trim();
                                string[] acceptedFormats = { "d/M/yyyy", "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy" };
                                bool isParsed = DateOnly.TryParseExact(dobText, acceptedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtStartDate);
                                }
                            DateOnly dtEndDate;
                            if (worksheet.Cells[row, 5].Value is DateTime dtE)
                                {
                                dtEndDate=DateOnly.FromDateTime(dtE);
                                }
                            else
                                {
                                string dobText = worksheet.Cells[row, 5].Text.Trim();
                                string[] acceptedFormats = { "d/M/yyyy", "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy" };
                                bool isParsed = DateOnly.TryParseExact(dobText, acceptedFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtEndDate);
                                }
                            var model = new Semester
                                {
                                SemesterId=worksheet.Cells[row, 2].Text,
                                SemesterName=worksheet.Cells[row, 3].Text,
                                Status=1,
                                StartDate=dtStartDate,
                                EndDate=dtEndDate,
                                };
                            #region 1. Validation       
                            string stt = worksheet.Cells[row, 1].Text;
                            var existingData = await _semesterRepository.GetSemesterById(model.SemesterId);
                            List<(bool condition, string errorMessage)>? validations = new List<(bool condition, string errorMessage)>
                              {
                                 (existingData != null,"Mã học kỳ tại ô số "+ stt +" đã tồn tại!"),
                                 (model.StartDate > model.EndDate ,"Thời gian bắt đầu tại ô số "+ stt +" không thể diễn ra trước thời gian kết thúc!"),
                              };
                            foreach (var validation in validations)
                                {
                                if (validation.condition)
                                    {
                                    return new APIResponse
                                        {
                                        IsSuccess=false,
                                        Message=validation.errorMessage
                                        };
                                    }
                                }
                            #endregion
                            models.Add(model);
                            }
                        }
                    }
                bool isSuccess = await _semesterRepository.AddSemestersAsyncs(models);
                if (isSuccess)
                    {
                    return new APIResponse { IsSuccess=true, Message="Import học kỳ thành công." };
                    }
                return new APIResponse { IsSuccess=false, Message="Import học kỳ thất bại." };
                }
            catch (Exception ex)
                {
                return new APIResponse { IsSuccess=false, Message=ex.Message };
                }
            }
        #endregion
        }
    }
