using BusinessObject;
using BusinessObject.ModelDTOs;
using UserService.Repository.MajorRepository;

namespace UserService.Services.MajorServices
{
    public class MajorService
    {
        private readonly IMajorRepository _repository;

        public MajorService()
        {
            _repository = new MajorRepository();
        }
        #region Get All Major
        /// <summary>
        /// Retrive all ClassSubject in Database
        /// </summary>
        /// <returns>a list of all Class Subject in DB</returns>
        public APIResponse GetAllMajor()
        {
            APIResponse aPIResponse = new APIResponse();
            List<MajorDTO> classSubjects = _repository.GetAllMajor();
            if (classSubjects == null)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Chưa có Chuyên ngành!";
            }
            aPIResponse.Result = classSubjects;
            return aPIResponse;
        }
        #endregion
    }
}
