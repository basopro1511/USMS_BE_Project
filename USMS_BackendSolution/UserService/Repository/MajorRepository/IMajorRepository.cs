using BusinessObject.ModelDTOs;

namespace UserService.Repository.MajorRepository
{
    public interface IMajorRepository
    {
        public List<MajorDTO> GetAllMajor();
    }
}
