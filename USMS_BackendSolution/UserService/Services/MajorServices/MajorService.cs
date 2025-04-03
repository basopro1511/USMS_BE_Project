using BusinessObject;
using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
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
        public async Task<APIResponse> GetAllMajor()
        {
            APIResponse aPIResponse = new APIResponse();
            List<Major> majors = await _repository.GetAllMajor();
            List<MajorDTO> majorDTOs = new List<MajorDTO>();
            foreach (var item in majors)
                {
                MajorDTO majorDTO = new MajorDTO();
                majorDTO.CopyProperties(item);
                majorDTOs.Add(majorDTO);
                }
            if (majors== null)
            {
                aPIResponse.IsSuccess = false;
                aPIResponse.Message = "Không tìm thấy chuyên ngành!";
            }
            aPIResponse.Result =majorDTOs;
            return aPIResponse;
        }
        #endregion
    }
}
