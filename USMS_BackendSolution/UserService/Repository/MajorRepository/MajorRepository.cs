using BusinessObject.AppDBContext;
using BusinessObject.ModelDTOs;
using BusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;

namespace UserService.Repository.MajorRepository
{
    public class MajorRepository : IMajorRepository
    {
        #region
		/// <summary>
		/// Get All Major In DB
		/// </summary>
		/// <returns></returns>
        public async Task <List<Major>> GetAllMajor()
		{
			try
			{
				using (var dbContext = new MyDbContext())
				{
					List<Major> majors =await dbContext.Major.ToListAsync();
					return majors;
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
