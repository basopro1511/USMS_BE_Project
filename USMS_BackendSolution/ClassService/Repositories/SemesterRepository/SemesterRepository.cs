using ClassBusinessObject.AppDBContext;
using SchedulerBusinessObject.ModelDTOs;
using ClassBusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using SchedulerBusinessObject.ModelDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SemesterRepository
    {
    public class SemesterRepository : ISemesterRepository
        {
        /// <summary>
        /// Get all semesters asynchronously
        /// </summary>
        /// <returns>List of SemesterDTO</returns>
        public async Task<List<Semester>> GetAllSemesters()
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    List<Semester> semesters = await dbContext.Semester.ToListAsync();
                    return semesters;
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
        public async Task<Semester?> GetSemesterById(string semesterId)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    Semester semester = await dbContext.Semester.FirstOrDefaultAsync(x => x.SemesterId==semesterId);
                    if (semester==null)
                        {
                        return null; // Không tìm thấy học kỳ
                        }
                    return semester;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        /// <summary>
        /// Add a new semester asynchronously
        /// </summary>
        /// <param name="semesterDto"></param>
        public async Task<bool> AddNewSemester(Semester semester)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    await dbContext.Semester.AddAsync(semester);
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception)
                {
                return false;
                }
            }
        /// <summary>
        /// Update an existing semester asynchronously
        /// </summary>
        /// <param name="updateSemester">Semester object with updated data</param>
        /// <returns>Boolean indicating success</returns>
        public async Task<bool> UpdateSemester(Semester updateSemester)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existingSemester = await dbContext.Semester.FirstOrDefaultAsync(s => s.SemesterId==updateSemester.SemesterId);
                    if (existingSemester==null)
                        {
                        return false; // Không tìm thấy học kỳ
                        }
                    existingSemester.CopyProperties(updateSemester);
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception ex)
                {
                Console.WriteLine($"Lỗi cập nhật học kỳ: {ex.Message}");
                return false;
                }
            }

        #region Delete Semester
        /// <summary>
        /// Delete a semester by ID asynchronously
        /// </summary>
        /// <param name="semesterId"></param>
        //public bool DeleteSemester(string semesterId)
        //{
        //    try
        //    {
        //        using (var dbContext = new MyDbContext())
        //        {
        //            var semester = dbContext.Semesters.FirstOrDefault(x => x.SemesterId == semesterId);
        //            if (semester == null)
        //            {
        //                return false;
        //            }
        //            dbContext.Semesters.Remove(semester);
        //            dbContext.SaveChanges();
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message, ex);
        //    }
        //}
        /// <summary>
        /// Get all active semesters   asynchronously
        /// </summary>
        /// <returns>List of active SemesterDTO</returns>
        #endregion

        public async Task<bool> ChangeStatusSemester(string semesterId, int status)
            {
            try
                {
                var existingSemesterDTO = await GetSemesterById(semesterId);
                if (existingSemesterDTO!=null)
                    {
                    using (var dbContext = new MyDbContext())
                        {
                        var existingSemester = dbContext.Semester.Find(semesterId);
                        if (existingSemester==null) return false;
                        existingSemester.CopyProperties(existingSemesterDTO);
                        existingSemester.Status=status;
                        dbContext.Entry(existingSemester).State=EntityState.Modified;
                        await dbContext.SaveChangesAsync();
                        }
                    return true;
                    }
                return false;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message, ex);
                }
            }

        #region Add list Semester 
        /// <summary>
        /// Add a list of Semesters from Excel
        /// </summary>
        /// <param name="models"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> AddSemestersAsyncs(List<Semester> models)
            {
            try
                {
                using (var _db = new MyDbContext())
                    {
                    await _db.Semester.AddRangeAsync(models);
                    await _db.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Change Semester selected Status 
        /// <summary>
        /// Change Semester selected Status 
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> ChangeSemesterStatusSelected(List<string> semesterId, int status)
            {
            try
                {
                using (var _db = new MyDbContext())
                    {
                    var Ids = await _db.Semester.Where(x => semesterId.Contains(x.SemesterId)).ToListAsync();
                    if (!Ids.Any())
                        return false;
                    foreach (var item in Ids)
                        {
                        item.Status=status;
                        }
                    await _db.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion
        }
    }
