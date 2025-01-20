using BusinessObject.AppDBContext;
using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;

namespace UserService.Repository.MajorRepository
{
    public class MajorRepository : IMajorRepository
    {

        #region
		/// <summary>
		/// Get All Major In DB
		/// </summary>
		/// <returns></returns>
        public List<MajorDTO> GetAllMajor()
		{
			try
			{
				using (var dbContext = new MyDbContext())
				{
					List<Major> majors = dbContext.Major.ToList();
					List<MajorDTO> majorDTOs = new List<MajorDTO>();
					foreach (var item in majors)
					{
						MajorDTO majorDTO = new MajorDTO();
						majorDTO.CopyProperties(item);
						majorDTOs.Add(majorDTO);
						dbContext.SaveChanges();
					}
					return majorDTOs;
				}
			}
			catch (Exception)
			{
				throw;
			}
		}
        #endregion
    }
}
