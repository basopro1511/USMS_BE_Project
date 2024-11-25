using ClassBusinessObject.AppDBContext;
using ClassBusinessObject.ModelDTOs;
using ClassBusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using SchedulerBusinessObject.ModelDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerDataAccess.Repositories.SemesterRepository
{
    public class SemesterRepository : ISemesterRepository
    {
        /// <summary>
        /// Get all semesters asynchronously
        /// </summary>
        /// <returns>List of SemesterDTO</returns>
        public async Task<IEnumerable<SemesterDTO>> GetAllSemestersAsync()
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var semesters = await dbContext.Semesters.ToListAsync();
                    return semesters.Select(semester =>
                    {
                        var semesterDto = new SemesterDTO();
                        semesterDto.CopyProperties(semester);
                        return semesterDto;
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Get semester by ID asynchronously
        /// </summary>
        /// <param name="semesterId"></param>
        /// <returns>SemesterDTO with the given ID</returns>
        public async Task<SemesterDTO> GetSemesterByIdAsync(string semesterId)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var semester = await dbContext.Semesters.FirstOrDefaultAsync(s => s.SemesterId == semesterId);
                    if (semester == null) return null;

                    var semesterDto = new SemesterDTO();
                    semesterDto.CopyProperties(semester);
                    return semesterDto;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Add a new semester asynchronously
        /// </summary>
        /// <param name="semesterDto"></param>
        public async Task AddSemesterAsync(SemesterDTO semesterDto)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var semester = new Semesters();
                    semester.CopyProperties(semesterDto);

                    await dbContext.Semesters.AddAsync(semester);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Update an existing semester asynchronously
        /// </summary>
        /// <param name="semesterDto"></param>
        public async Task UpdateSemesterAsync(SemesterDTO semesterDto)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingSemester = await dbContext.Semesters.FirstOrDefaultAsync(s => s.SemesterId == semesterDto.SemesterId);
                    if (existingSemester == null) throw new Exception("Semester not found.");

                    existingSemester.CopyProperties(semesterDto);
                    dbContext.Entry(existingSemester).State = EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Delete a semester by ID asynchronously
        /// </summary>
        /// <param name="semesterId"></param>
        public async Task DeleteSemesterAsync(string semesterId)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var semester = await dbContext.Semesters.FirstOrDefaultAsync(s => s.SemesterId == semesterId);
                    if (semester == null) throw new Exception("Semester not found.");

                    dbContext.Semesters.Remove(semester);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        /// <summary>
        /// Get all active semesters   asynchronously
        /// </summary>
        /// <returns>List of active SemesterDTO</returns>
        public async Task<IEnumerable<SemesterDTO>> GetActiveSemestersAsync()
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var activeSemesters = await dbContext.Semesters.Where(s => s.Status).ToListAsync();
                    return activeSemesters.Select(semester =>
                    {
                        var semesterDto = new SemesterDTO();
                        semesterDto.CopyProperties(semester);
                        return semesterDto;
                    });
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
