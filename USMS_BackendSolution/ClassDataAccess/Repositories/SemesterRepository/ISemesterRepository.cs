using SchedulerBusinessObject.ModelDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerDataAccess.Repositories.SemesterRepository
{
    public interface ISemesterRepository
    {
        Task<IEnumerable<SemesterDTO>> GetAllSemestersAsync();
        Task<SemesterDTO> GetSemesterByIdAsync(string semesterId);
        Task AddSemesterAsync(SemesterDTO semesterDto);
        Task UpdateSemesterAsync(SemesterDTO semesterDto);
        Task DeleteSemesterAsync(string semesterId);
        Task<IEnumerable<SemesterDTO>> GetActiveSemestersAsync();
    }
}
