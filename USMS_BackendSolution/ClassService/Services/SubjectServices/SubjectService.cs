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
			bool result = _subjectRepository.CreateSubject(subjectDTO);
			if (!result)
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "Subject ID is already existed!";
			}
			else
			{
				aPIResponse.IsSuccess = true;
			}
			return aPIResponse;
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
			bool result = _subjectRepository.UpdateSubject(subjectDTO);
			if (!result)
			{
				aPIResponse.IsSuccess = false;
				aPIResponse.Message = "Subject ID is not existed!";
			}
			else
			{
				aPIResponse.IsSuccess = true;
			}
			return aPIResponse;
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
						aPIResponse.Message = $" Môn học với mã: {subjectId} đã kết thúc.";
						break;
					case 1:
						aPIResponse.Message = $" Môn học với mã: {subjectId} đang diễn ra";
						break;
					case 2:
						aPIResponse.Message = $" Môn học với mã: {subjectId} chưa bắt đầu";
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
	}
}
