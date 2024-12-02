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
        public List<SemesterDTO> GetAllSemesters()
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    List <Semester> semesters = dbContext.Semester.ToList();
                    List<SemesterDTO> semesterDTOs = new List<SemesterDTO>();
                    foreach (var semester in semesters)
                    {
                        SemesterDTO SemesterDTO = new SemesterDTO();
                        SemesterDTO.CopyProperties(semester);
                        semesterDTOs.Add(SemesterDTO);
                    }
                    return semesterDTOs;
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
        public SemesterDTO GetSemesterById(string semesterId)
        {
            try
            {
                var semesters = GetAllSemesters();
                SemesterDTO semesterDTO = semesters.FirstOrDefault(x => x.SemesterId == semesterId);
                return semesterDTO;
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
        public bool AddNewSemester(SemesterDTO semesterDTO)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var semester = new Semester();
                    semester.CopyProperties(semesterDTO);
                    dbContext.Semester.Add(semester);
                    dbContext.SaveChanges();
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
        /// <param name="semesterDto"></param>
        public bool UpdateSemester(SemesterDTO updateSemesterDTO)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingSemester = GetSemesterById(updateSemesterDTO.SemesterId);
                    if (existingSemester == null)
                    {
                        return false;
                    }
                    existingSemester.CopyProperties(updateSemesterDTO);
                    dbContext.Entry(existingSemester).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
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
        public bool ChangeStatusSemester(string semesterId)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var semester = GetSemesterById(semesterId);
                    if (semester == null)
                    {
                        return false; // Không tìm thấy Semester
                    }
                    semester.Status = !semester.Status;
                    dbContext.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
