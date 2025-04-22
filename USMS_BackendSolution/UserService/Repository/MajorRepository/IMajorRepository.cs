using BusinessObject.ModelDTOs;
using BusinessObject.Models;

namespace UserService.Repository.MajorRepository
{
    public interface IMajorRepository
    {
        public Task<List<Major>> GetAllMajor();
    }
}
