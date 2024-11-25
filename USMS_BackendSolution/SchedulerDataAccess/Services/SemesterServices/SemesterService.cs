using SchedulerBusinessObject.ModelDTOs;
using SchedulerDataAccess.Repositories.SemesterRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerDataAccess.Services.SemesterServices
{
    public class SemesterService
    {
        private readonly ISemesterRepository _semesterRepository;

        // Constructor for dependency injection
        public SemesterService(ISemesterRepository semesterRepository)
        {
            _semesterRepository = semesterRepository;
        }

        // Get all semesters
        public async Task<IEnumerable<SemesterDTO>> GetAllSemestersAsync()
        {
            return await _semesterRepository.GetAllSemestersAsync();
        }

        // Get a semester by ID
        public async Task<SemesterDTO> GetSemesterByIdAsync(string semesterId)
        {
            return await _semesterRepository.GetSemesterByIdAsync(semesterId);
        }

        // Add a new semester
        public async Task AddSemesterAsync(SemesterDTO semesterDto)
        {
            await _semesterRepository.AddSemesterAsync(semesterDto);
        }

        // Update an existing semester
        public async Task UpdateSemesterAsync(SemesterDTO semesterDto)
        {
            await _semesterRepository.UpdateSemesterAsync(semesterDto);
        }

        // Delete a semester by ID
        public async Task DeleteSemesterAsync(string semesterId)
        {
            await _semesterRepository.DeleteSemesterAsync(semesterId);
        }

        // Get active semesters
        public async Task<IEnumerable<SemesterDTO>> GetActiveSemestersAsync()
        {
            return await _semesterRepository.GetActiveSemestersAsync();
        }
    }
}
