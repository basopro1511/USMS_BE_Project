using ClassBusinessObject;
using ClassBusinessObject.ModelDTOs;
using Repositories.SubjectRepository;

namespace Services.SubjectServices
{
	public class SubjectService
	{
		private readonly ISubjectRepository _subjectRepository;
		public SubjectService()
		{
			_subjectRepository = new SubjectRepository();
		}

		#region Get All Subjects
		/// <summary>
		/// Get All Subjects
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public APIResponse GetAllSubjects()
		{
			APIResponse aPIResponse = new APIResponse();
			List<SubjectDTO>? subjects = _subjectRepository.GetAllSubjects();
			if (subjects == null || subjects.Count == 0)
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "Không có môn học đang tìm kiếm.";
			}
			aPIResponse.Result = subjects;
			return aPIResponse;
		}
        #endregion

        #region Create Subject
        /// <summary>
        /// Create Subject
        /// </summary>
        /// <param name="subjectDTO"></param>
        /// <returns></returns>
        public APIResponse CreateSubject(SubjectDTO subjectDTO)
        {
            APIResponse aPIResponse = new APIResponse();
            SubjectDTO existingSubject = _subjectRepository.GetSubjectsById(subjectDTO.SubjectId);
            #region validation cua Add
            var validations = new List<(bool condition, string errorMessage)>
            {
                  (existingSubject != null,"Mã môn học đã tồn tại!"),
                  (subjectDTO.SubjectId.Length > 10,"Mã môn học phải có độ dài từ 1 đến 10 ký tự!"),
                  (!subjectDTO.SubjectId.Any(char.IsLetter) || !subjectDTO.SubjectId.Any(char.IsDigit),"Mã môn học phải chứa cả chữ và số!"),
                  (string.IsNullOrEmpty(subjectDTO.SubjectName),"Tên môn học không được để trống" ),
                  (subjectDTO.SubjectName.Length > 100, "Tên môn học phải có độ dài tối đa 100 ký tự!"),
                  (string.IsNullOrEmpty(subjectDTO.MajorId),"Chuyên ngành không được để trống!"),
                  (subjectDTO.NumberOfSlot <= 4 || subjectDTO.NumberOfSlot > 30,"Số buổi học phải lớn hơn 4 và nhỏ hơn hoặc bằng 30!"),
                  (subjectDTO.Term <= 0 || subjectDTO.Term > 8,"Kỳ học phải lớn hơn 0 và nhỏ hơn hoặc bằng 8!"),
            };
            foreach (var validation in validations)
            {
                if (validation.condition)
                {
                    return new APIResponse
                    {
                        IsSuccess = false,
                        Message = validation.errorMessage
                    };
                }
            }
            #endregion
            // Thêm môn học vào cơ sở dữ liệu
            bool isAdded = _subjectRepository.CreateSubject(subjectDTO);
            if (isAdded)
            {
                return new APIResponse
                {
                    IsSuccess = true,
                    Message = "Thêm môn học thành công!"
                };
            }
            // Trường hợp thêm thất bại
            return new APIResponse
            {
                IsSuccess = false,
                Message = "Thêm môn học thất bại!"
            };
        }
        #endregion

        #region Update Subject
        /// <summary>
        /// Update Subject
        /// </summary>
        /// <param name="subjectDTO"></param>
        /// <returns></returns>
        public APIResponse UpdateSubject(SubjectDTO subjectDTO)
        {
            APIResponse aPIResponse = new APIResponse();
            SubjectDTO existingSubject = _subjectRepository.GetSubjectsById(subjectDTO.SubjectId);
            #region validation cua Update
            var validations = new List<(bool condition, string errorMessage)>
            {
                  (subjectDTO.SubjectId.Length < 4, "Mã môn học không thể ngắn hơn 4 ký tự"),
                  (subjectDTO.SubjectId.Length > 10, "Mã môn học không thể dài hơn 10 ký tự"),
                  (subjectDTO == null, "Mã môn học không tồn tại!"),
                  (!subjectDTO.SubjectId.Any(char.IsLetter) || !subjectDTO.SubjectId.Any(char.IsDigit),"Mã môn học phải chứa cả chữ và số!"),
                  (string.IsNullOrEmpty(subjectDTO.SubjectName),"Tên môn học không được để trống" ),
                  (subjectDTO.SubjectName.Length > 100, "Tên môn học phải có độ dài tối đa 100 ký tự!"),
                  (string.IsNullOrEmpty(subjectDTO.MajorId),"Chuyên ngành không được để trống!"),
                  (subjectDTO.NumberOfSlot <= 4 || subjectDTO.NumberOfSlot > 30,"Số buổi học phải lớn hơn 4 và nhỏ hơn hoặc bằng 30!"),
                  (subjectDTO.Term <= 0 || subjectDTO.Term > 8,"Kỳ học phải lớn hơn 0 và nhỏ hơn hoặc bằng 8!"),
            };
            foreach (var validation in validations)
            {
                if (validation.condition)
                {
                    return new APIResponse
                    {
                        IsSuccess = false,
                        Message = validation.errorMessage
                    };
                }
            }
            #endregion
            // Cập nhật môn học trong cơ sở dữ liệu
            bool isUpdated = _subjectRepository.UpdateSubject(subjectDTO);
            if (isUpdated)
            {
                return new APIResponse
                {
                    IsSuccess = true,
                    Message = "Cập nhật môn học thành công!"
                };
            }
            // Trường hợp cập nhật thất bại
            return new APIResponse
            {
                IsSuccess = false,
                Message = "Cập nhật môn học thất bại!"
            };
        }
        #endregion

        #region Get Subjecy By ID
        /// <summary>
        /// Get Subject By ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public APIResponse GetSubjectById(string subjectID)
        {
            APIResponse aPIResponse = new APIResponse();
			SubjectDTO subject = _subjectRepository.GetSubjectsById(subjectID);
            if (subject == null)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = $"Môn học với mã: {subjectID} không tồn tại. Vui lòng kiểm tra lại";
            }
            aPIResponse.Result = subject;
            return aPIResponse;
        }
        #endregion

        #region Switch state subject
        /// <summary>
        /// Switch state subject
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        public APIResponse SwitchStateSubject(string subjectId, int status)
		{
			APIResponse aPIResponse = new APIResponse();
			SubjectDTO existingSubject = _subjectRepository.GetSubjectsById(subjectId);
            if (existingSubject == null)
			{
				return new APIResponse
				{
					IsSuccess = false,
					Message = "Mã môn học được cung cấp không tồn tại!"
				};
			}
			//Validate status input
			if (status < 0 || status > 2)
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "Trạng thái không hợp lệ. Vui lòng nhập trạng thái từ 0 đến 2.";
				return aPIResponse;
			}
			//Update 
			bool isUpdated = _subjectRepository.SwitchStateSubject(subjectId, status);
			if (isUpdated)
			{
				aPIResponse.IsSuccess = true;
				switch (status)
				{
					case 0:
						aPIResponse.Message = $" Môn học với mã: {subjectId} chưa bắt đầu.";
						break;
					case 1:
						aPIResponse.Message = $" Môn học với mã: {subjectId} đang diễn ra.";
						break;
					case 2:
						aPIResponse.Message = $" Môn học với mã: {subjectId} đã kết thúc.";
						break;
				}
			}
			else
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "Cập nhật trạng thái môn học thất bại.";
			}
			return aPIResponse;
		}
        #endregion

        #region Get Subject by Major ID
        public APIResponse GetSubjectByMajorId(string majorId)
        {
            APIResponse aPIResponse = new APIResponse();
            List<SubjectDTO>? subjects = _subjectRepository.GetAllSubjects();
            List<SubjectDTO>? subjectByMajor = subjects.Where(x => x.MajorId == majorId).ToList();
            if (subjects == null || subjects.Count == 0)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Không có môn học đang tìm kiếm.";
            }
            aPIResponse.Result = subjectByMajor;
            return aPIResponse;
        }
        #endregion
  
        #region Get Subject by Major ID and Term
        public APIResponse GetSubjectByMajorIdAndTerm(string majorId, int term)
        {
            APIResponse aPIResponse = new APIResponse();
            List<SubjectDTO>? subjects = _subjectRepository.GetAllSubjects();
            List<SubjectDTO>? subjectByMajor = subjects.Where(x => x.MajorId == majorId && x.Term == term).ToList();
            if (subjects == null || subjects.Count == 0)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Không có môn học đang tìm kiếm.";
            }
            aPIResponse.Result = subjectByMajor;
            return aPIResponse;
        }
        #endregion
    }
}
