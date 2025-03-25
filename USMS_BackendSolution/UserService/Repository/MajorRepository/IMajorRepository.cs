using BusinessObject.ModelDTOs;

namespace UserService.Repository.MajorRepository
{
    public interface IMajorRepository
    {
        public Task<List<MajorDTO>> GetAllMajor();
    }
}
