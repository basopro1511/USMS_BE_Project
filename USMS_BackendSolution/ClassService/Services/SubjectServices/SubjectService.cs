using ClassBusinessObject;
using ClassBusinessObject.ModelDTOs;
using ClassBusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol.Core.Types;
using OfficeOpenXml;
using Repositories.SubjectRepository;
using System.ComponentModel;

namespace Services.SubjectServices
    {
    public class SubjectService
        {
        private readonly ISubjectRepository _subjectRepository;
        public SubjectService()
            {
            _subjectRepository=new SubjectRepository();
            }

        #region Get All Subjects
        /// <summary>
        /// Get All Subjects
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<APIResponse> GetAllSubjects()
            {
            APIResponse aPIResponse = new APIResponse();
            var subjects = await _subjectRepository.GetAllSubjects();
            if (subjects==null||subjects.Count==0)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không có môn học đang tìm kiếm.";
                }
            aPIResponse.Result=subjects;
            return aPIResponse;
            }
        #endregion

        #region Create Subject
        /// <summary>
        /// Create Subject
        /// </summary>
        /// <param name="subjectDTO"></param>
        /// <returns></returns>
        public async Task<APIResponse> CreateSubject(SubjectDTO subjectDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            subjectDTO.SubjectId=subjectDTO.SubjectId?.Trim();
            var existingSubject = await _subjectRepository.GetSubjectsById(subjectDTO.SubjectId);
            #region validation cua Add
            List<(bool condition, string errorMessage)>? validations = new List<(bool condition, string errorMessage)>
            {
                  (existingSubject != null,"Mã môn học đã tồn tại!"),
                  (subjectDTO.SubjectId.Length > 10,"Mã môn học phải có độ dài từ 1 đến 10 ký tự!"),
                  (!subjectDTO.SubjectId.Any(char.IsLetter) || !subjectDTO.SubjectId.Any(char.IsDigit),"Mã môn học phải chứa cả chữ và số!"),
                  (string.IsNullOrEmpty(subjectDTO.SubjectName),"Tên môn học không được để trống" ),
                  (subjectDTO.SubjectName.Length > 100, "Tên môn học phải có độ dài tối đa 100 ký tự!"),
                  (subjectDTO.NumberOfSlot <= 4 || subjectDTO.NumberOfSlot > 30,"Số buổi học phải lớn hơn 4 và nhỏ hơn hoặc bằng 30!"),
                  (subjectDTO.Term <= 0 || subjectDTO.Term > 8,"Kỳ học phải lớn hơn 0 và nhỏ hơn hoặc bằng 8!"),
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
            // Thêm môn học vào cơ sở dữ liệu
            Subject subject = new Subject();
            subject.CopyProperties(subjectDTO);
            bool isAdded = await _subjectRepository.CreateSubject(subject);
            if (isAdded)
                {
                return new APIResponse
                    {
                    IsSuccess=true,
                    Message="Thêm môn học thành công!"
                    };
                }
            // Trường hợp thêm thất bại
            return new APIResponse
                {
                IsSuccess=false,
                Message="Thêm môn học thất bại!"
                };
            }
        #endregion

        #region Update Subject
        /// <summary>
        /// Update Subject
        /// </summary>
        /// <param name="subjectDTO"></param>
        /// <returns></returns>
        public async Task<APIResponse> UpdateSubject(SubjectDTO subjectDTO)
            {
            APIResponse aPIResponse = new APIResponse();
            subjectDTO.SubjectId=subjectDTO.SubjectId?.Trim();
            var existingSubject = await _subjectRepository.GetSubjectsById(subjectDTO.SubjectId);
            #region validation cua Update
            var validations = new List<(bool condition, string errorMessage)>
            {
                  (subjectDTO.SubjectId.Length < 4, "Mã môn học không thể ngắn hơn 4 ký tự"),
                  (subjectDTO.SubjectId.Length > 10, "Mã môn học không thể dài hơn 10 ký tự"),
                  (subjectDTO == null, "Mã môn học không tồn tại!"),
                  (!subjectDTO.SubjectId.Any(char.IsLetter) || !subjectDTO.SubjectId.Any(char.IsDigit),"Mã môn học phải chứa cả chữ và số!"),
                  (string.IsNullOrEmpty(subjectDTO.SubjectName),"Tên môn học không được để trống" ),
                  (subjectDTO.SubjectName.Length > 100, "Tên môn học phải có độ dài tối đa 100 ký tự!"),
                  (subjectDTO.NumberOfSlot <= 4 || subjectDTO.NumberOfSlot > 30,"Số buổi học phải lớn hơn 4 và nhỏ hơn hoặc bằng 30!"),
                  (subjectDTO.Term <= 0 || subjectDTO.Term > 8,"Kỳ học phải lớn hơn 0 và nhỏ hơn hoặc bằng 8!"),
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
            // Cập nhật môn học trong cơ sở dữ liệu
            Subject subject = new Subject();
            subject.CopyProperties(subjectDTO);
            bool isUpdated = await _subjectRepository.UpdateSubject(subject);
            if (isUpdated)
                {
                return new APIResponse
                    {
                    IsSuccess=true,
                    Message="Cập nhật môn học thành công!"
                    };
                }
            // Trường hợp cập nhật thất bại
            return new APIResponse
                {
                IsSuccess=false,
                Message="Cập nhật môn học thất bại!"
                };
            }
        #endregion

        #region Get Subjecy By ID
        /// <summary>
        /// Get Subject By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<APIResponse> GetSubjectById(string subjectID)
            {
            APIResponse aPIResponse = new APIResponse();
            var subject = await _subjectRepository.GetSubjectsById(subjectID);
            if (subject==null)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message=$"Môn học với mã: {subjectID} không tồn tại. Vui lòng kiểm tra lại";
                }
            aPIResponse.Result=subject;
            return aPIResponse;
            }
        #endregion

        #region Switch state subject
        /// <summary>
        /// Switch state subject
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        public async Task<APIResponse> SwitchStateSubject(string subjectId, int status)
            {
            APIResponse aPIResponse = new APIResponse();
            var existingSubject = await _subjectRepository.GetSubjectsById(subjectId);
            if (existingSubject==null)
                {
                return new APIResponse
                    {
                    IsSuccess=false,
                    Message="Mã môn học được cung cấp không tồn tại!"
                    };
                }
            //Validate status input
            if (status<0||status>2)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Trạng thái không hợp lệ. Vui lòng nhập trạng thái từ 0 đến 2.";
                return aPIResponse;
                }
            //Update 
            bool isUpdated = await _subjectRepository.SwitchStateSubject(subjectId, status);
            if (isUpdated)
                {
                aPIResponse.IsSuccess=true;
                switch (status)
                    {
                    case 0:
                        aPIResponse.Message=$" Môn học với mã: {subjectId} đã được vô hiệu hóa.";
                        break;
                    case 1:
                        aPIResponse.Message=$" Môn học với mã: {subjectId} đang diễn ra.";
                        break;
                    case 2:
                        aPIResponse.Message=$" Môn học với mã: {subjectId} đang tạm hoãn.";
                        break;
                    }
                }
            else
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Cập nhật trạng thái môn học thất bại.";
                }
            return aPIResponse;
            }
        #endregion

        #region Get Subject by Major ID
        public async Task<APIResponse> GetSubjectByMajorId(string majorId)
            {
            APIResponse aPIResponse = new APIResponse();
            var subjects = await _subjectRepository.GetAllSubjects();
            var subjectByMajor = subjects.Where(x => x.MajorId==majorId).ToList();
            if (subjects==null||subjects.Count==0)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không có môn học đang tìm kiếm.";
                }
            aPIResponse.Result=subjectByMajor;
            return aPIResponse;
            }
        #endregion

        #region Get Subjects availables have status = 1 by Major ID and Term
        public async Task<APIResponse> GetSubjectByMajorIdAndTerm(string majorId, int term)
            {
            APIResponse aPIResponse = new APIResponse();
            var subjects = await _subjectRepository.GetAllSubjects();
            var subjectByMajor = subjects.Where(x => x.MajorId==majorId&&x.Term==term&&x.Status==1).ToList();
            if (subjects==null||subjects.Count==0)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không có môn học đang tìm kiếm.";
                }
            aPIResponse.Result=subjectByMajor;
            return aPIResponse;
            }
        #endregion

        #region Get All Subjects Available ( Status = 1 )
        /// <summary>
        /// Get All Subjects
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<APIResponse> GetAllSubjectsAvailable()
            {
            APIResponse aPIResponse = new APIResponse();
            var subjects = await _subjectRepository.GetAllSubjectsAvailable();
            if (subjects==null||subjects.Count==0)
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Không có môn học đang tìm kiếm.";
                }
            aPIResponse.Result=subjects;
            return aPIResponse;
            }
        #endregion

        #region Change Subject Status Selected 
        /// <summary>
        /// Change user status
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<APIResponse> ChangeSubjectStatusSelected(List<string> Ids, int status)
            {
            APIResponse aPIResponse = new APIResponse();
            if (Ids==null||!Ids.Any())
                {
                aPIResponse.IsSuccess=false;
                aPIResponse.Message="Danh sách môn học không hợp lệ.";
                return aPIResponse;
                }
            bool isSuccess = await _subjectRepository.ChangeSubjectStatusSelected(Ids, status);
            if (isSuccess)
                {
                aPIResponse.IsSuccess=true;
                switch (status)
                    {
                    case 0:
                        aPIResponse.Message="Đã thay đổi trạng thái các môn học thành 'Chưa bắt đầu'.";
                        break;
                    case 1:
                        aPIResponse.Message="Đã thay đổi trạng thái các môn học thành 'Đang diễn ra'.";
                        break;
                    case 2:
                        aPIResponse.Message="Đã thay đổi trạng thái các môn học thành 'Đã kết thúc'.";
                        break;
                    default:
                        aPIResponse.Message="Trạng thái không hợp lệ.";
                        break;
                    }
                }
            return aPIResponse;
            }
        #endregion

        #region Export Form Add Subject
        /// <summary>
        /// Export empty form for add model
        /// </summary>
        /// <returns></returns>
        public async Task<byte[]> ExportFormAddSubject()
            {
            ExcelPackage.LicenseContext=OfficeOpenXml.LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
                {
                var worksheet = package.Workbook.Worksheets.Add("Subjects");
                // Header
                worksheet.Cells[1, 1].Value="STT";
                worksheet.Cells[1, 2].Value="Mã môn học";
                worksheet.Cells[1, 3].Value="Tên môn học";
                worksheet.Cells[1, 4].Value="Chuyên ngành";
                worksheet.Cells[1, 5].Value="Số lượng buổi học";
                worksheet.Cells[1, 6].Value="Kỳ học số";
                worksheet.Cells[1, 7].Value="Mô tả môn học";
                // Gán công thức tự động tăng STT từ dòng 2 đến 1000
                for (int row = 2; row<=1000; row++)
                    {
                    worksheet.Cells[row, 1].Formula=$"=ROW()-1";
                    }
                // Định dạng các cột để bắt validation
                var majorValidation = worksheet.DataValidations.AddListValidation("D2:D1000");
                majorValidation.Formula.Values.Add("SE");
                majorValidation.Formula.Values.Add("BA");
                majorValidation.Formula.Values.Add("LG");
                majorValidation.Formula.Values.Add("CT");
                majorValidation.ShowErrorMessage=true;
                majorValidation.ErrorTitle="Sai chuyên ngành";
                majorValidation.Error="Chuyên ngành chỉ có thể là SE, BA, LG hoặc CT";
                worksheet.Cells.AutoFitColumns();
                return package.GetAsByteArray();
                }
            }
        #endregion

        #region Export Subject Information
        /// <summary>
        /// Export SubjectInformation
        /// </summary>
        /// <param name="majorId"></param>
        /// <returns></returns>
        public async Task<byte[]> ExportSubjectToExcel(string? majorId, int? status)
            {
            var models = await _subjectRepository.GetAllSubjects();
            if (!string.IsNullOrEmpty(majorId))
                models=models.Where(s => s.MajorId==majorId).ToList();
            if (status.HasValue)
                models=models.Where(s => s.Status==status.Value).ToList();
            ExcelPackage.LicenseContext=OfficeOpenXml.LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
                {
                var worksheet = package.Workbook.Worksheets.Add("Subjects");
                // Header
                worksheet.Cells[1, 1].Value="STT";
                worksheet.Cells[1, 2].Value="Mã môn học";
                worksheet.Cells[1, 3].Value="Tên môn học";
                worksheet.Cells[1, 4].Value="Chuyên ngành";
                worksheet.Cells[1, 5].Value="Số lượng buổi học";
                worksheet.Cells[1, 6].Value="Mô tả môn học";
                worksheet.Cells[1, 7].Value="Kỳ học số";
                worksheet.Cells[1, 8].Value="Trạng thái";
                worksheet.Cells[1, 9].Value="Thời gian khởi tạo";
                worksheet.Cells[1, 10].Value="Thời gian cập nhật";
                int row = 2;
                int stt = 1;
                foreach (var s in models)
                    {
                    worksheet.Cells[row, 1].Value=stt++;
                    worksheet.Cells[row, 2].Value=s.SubjectId;
                    worksheet.Cells[row, 3].Value=s.SubjectName;
                    worksheet.Cells[row, 4].Value=s.MajorId;
                    worksheet.Cells[row, 5].Value=s.NumberOfSlot;
                    worksheet.Cells[row, 6].Value=s.Description;
                    worksheet.Cells[row, 7].Value=s.Term;
                    worksheet.Cells[row, 8].Value=s.Status==1 ? "Đang khả dụng" : "Vô hiệu hóa";
                    worksheet.Cells[row, 9].Value=s.CreatedAt.ToString("dd/MM/yyyy");
                    worksheet.Cells[row, 10].Value=s.UpdatedAt.ToString("dd/MM/yyyy");
                    row++;
                    }
                return package.GetAsByteArray();
                }
            }
        #endregion

        #region Import Subjects from Excels
        /// <summary>
        /// Import Subjects Form Excel
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public async Task<APIResponse> ImportSubjectsFromExcel(IFormFile file)
            {
            try
                {
                if (file==null||file.Length==0)
                    {
                    return new APIResponse { IsSuccess=false, Message="File không hợp lệ." };
                    }
                var subjects = new List<Subject>();
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
                            var subjcet = new Subject
                                {
                                SubjectId=worksheet.Cells[row, 2].Text,
                                SubjectName=worksheet.Cells[row, 3].Text,
                                MajorId=worksheet.Cells[row, 4].Text,
                                NumberOfSlot=int.TryParse(worksheet.Cells[row, 5].Text, out int slot) ? slot : 0,
                                Description=worksheet.Cells[row, 7].Text,
                                Term=int.TryParse(worksheet.Cells[row,6].Text, out int term) ? term : 0,
                                Status=1,
                                CreatedAt=DateTime.Now,
                                UpdatedAt=DateTime.Now
                                };
                            #region 1. Validation       
                            string stt = worksheet.Cells[row, 1].Text;
                            subjcet.SubjectId=subjcet.SubjectId?.Trim();
                            var existingSubject = await _subjectRepository.GetSubjectsById(subjcet.SubjectId);
                            List<(bool condition, string errorMessage)>? validations = new List<(bool condition, string errorMessage)>
                              {
                                 (existingSubject != null,"Mã môn học đã tồn tại!"),
                                 (subjcet.SubjectId.Length > 10,"Mã môn học tại ô số "+ stt + " phải có độ dài từ 1 đến 10 ký tự!"),
                                 (!subjcet.SubjectId.Any(char.IsLetter) || !subjcet.SubjectId.Any(char.IsDigit),"Mã môn học tại ô số "+ stt + " phải chứa cả chữ và số!"),
                                 (string.IsNullOrEmpty(subjcet.SubjectName),"Tên môn học tại ô số "+ stt + "  không được để trống" ),
                                 (subjcet.SubjectName.Length > 100, "Tên môn học tại ô số "+ stt + "  phải có độ dài tối đa 100 ký tự!"),
                                 (subjcet.NumberOfSlot <= 4 || subjcet.NumberOfSlot > 30,"Số buổi học tại ô số "+ stt + "  phải lớn hơn 4 và nhỏ hơn hoặc bằng 30!"),
                                 (subjcet.Term <= 0 || subjcet.Term > 8,"Kỳ học tại ô số "+ stt + "  phải lớn hơn 0 và nhỏ hơn hoặc bằng 8!"),
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
                            subjects.Add(subjcet);
                            }
                        }
                    }
                bool isSuccess = await _subjectRepository.AddSubjectsAsyncs(subjects);
                if (isSuccess)
                    {
                    return new APIResponse { IsSuccess=true, Message="Import môn học thành công." };
                    }
                return new APIResponse { IsSuccess=false, Message="Import môn học thất bại." };
                }
            catch (Exception ex)
                {
                return new APIResponse { IsSuccess=false, Message=ex.Message };
                }
            }
        #endregion
        }
    }
