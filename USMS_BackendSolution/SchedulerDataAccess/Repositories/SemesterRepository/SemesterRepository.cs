using Microsoft.EntityFrameworkCore;
using SchedulerBusinessObject.AppDBContext;
using SchedulerBusinessObject.ModelDTOs;
using SchedulerBusinessObject.SchedulerModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerDataAccess.Repositories.SemesterRepository
{
    public class SemesterRepository : ISemesterRepository
    {
        private readonly MyDbContext _context;

        // Constructor for dependency injection
        public SemesterRepository(MyDbContext context)
        {
            _context = context;
        }

        // Get all semesters
        public async Task<IEnumerable<SemesterDTO>> GetAllSemestersAsync()
        {
            return await _context.Semesters
                .Select(s => new SemesterDTO
                {
                    SemesterId = s.SemesterId,
                    SemesterName = s.SemesterName,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    Status = s.Status
                })
                .ToListAsync();
        }

        // Get a semester by ID
        public async Task<SemesterDTO> GetSemesterByIdAsync(string semesterId)
        {
            var semester = await _context.Semesters.FindAsync(semesterId);
            if (semester == null) return null;

            return new SemesterDTO
            {
                SemesterId = semester.SemesterId,
                SemesterName = semester.SemesterName,
                StartDate = semester.StartDate,
                EndDate = semester.EndDate,
                Status = semester.Status
            };
        }

        // Add a new semester
        public async Task AddSemesterAsync(SemesterDTO semesterDto)
        {
            var semester = new Semesters
            {
                SemesterId = semesterDto.SemesterId,
                SemesterName = semesterDto.SemesterName,
                StartDate = semesterDto.StartDate,
                EndDate = semesterDto.EndDate,
                Status = semesterDto.Status
            };

            await _context.Semesters.AddAsync(semester);
            await _context.SaveChangesAsync();
        }

        // Update an existing semester
        public async Task UpdateSemesterAsync(SemesterDTO semesterDto)
        {
            var semester = await _context.Semesters.FindAsync(semesterDto.SemesterId);
            if (semester != null)
            {
                semester.SemesterName = semesterDto.SemesterName;
                semester.StartDate = semesterDto.StartDate;
                semester.EndDate = semesterDto.EndDate;
                semester.Status = semesterDto.Status;

                _context.Semesters.Update(semester);
                await _context.SaveChangesAsync();
            }
        }

        // Delete a semester by ID
        public async Task DeleteSemesterAsync(string semesterId)
        {
            var semester = await _context.Semesters.FindAsync(semesterId);
            if (semester != null)
            {
                _context.Semesters.Remove(semester);
                await _context.SaveChangesAsync();
            }
        }

        // Get active semesters
        public async Task<IEnumerable<SemesterDTO>> GetActiveSemestersAsync()
        {
            return await _context.Semesters
            .Where(s => s.Status)
                .Select(s => new SemesterDTO
                {
                    SemesterId = s.SemesterId,
                    SemesterName = s.SemesterName,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                    Status = s.Status
                })
                .ToListAsync();
        }
    }
}
