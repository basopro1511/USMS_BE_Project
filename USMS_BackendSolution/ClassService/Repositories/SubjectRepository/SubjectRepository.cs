using ClassBusinessObject.ModelDTOs;
using ClassBusinessObject.Models;
using Microsoft.EntityFrameworkCore;
using ISUZU_NEXT.Server.Core.Extentions;
using ClassBusinessObject.AppDBContext;

namespace Repositories.SubjectRepository
    {
    public class SubjectRepository : ISubjectRepository
        {
        /// <summary>
        /// Create Subject
        /// </summary>
        /// <param name="SubjectDTO"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<bool> CreateSubject(Subject subject)
            {
            if (subject==null)
                {
                throw new ArgumentNullException(nameof(Subject), "Subject is null");
                }
            try
                {
                var checkSubject = await GetSubjectsById(subject.SubjectId);
                if (checkSubject!=null)
                    {
                    return false;
                    }
                subject.CreatedAt=DateTime.Now;
                subject.UpdatedAt=DateTime.Now;
                using (var _db = new MyDbContext())
                    {
                    _db.Add(subject);
                    await _db.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message, ex);
                }
            }

        /// <summary>
        /// Update Subject
        /// </summary>
        /// <param name="SubjectDTO"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<bool> UpdateSubject(Subject subject)
            {
            if (subject==null)
                {
                throw new ArgumentNullException(nameof(subject), "Subject is null");
                }
            try
                {
                subject.UpdatedAt=DateTime.Now;
                using (var _db = new MyDbContext())
                    {
                    _db.Entry(subject).State=EntityState.Modified;
                    await _db.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message, ex);
                }
            }

        /// <summary>
        /// Get All Subjects
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Subject>>? GetAllSubjects()
            {
            try
                {
                using (var _db = new MyDbContext())
                    {
                    var subjects = await _db.Subject.ToListAsync();
                    return subjects;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }

        /// <summary>
        /// Get Subjects By Id
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Subject> GetSubjectsById(string subjectId)
            {
            try
                {
                using (var _db = new MyDbContext())
                    {
                    Subject? subject = await _db.Subject.FirstOrDefaultAsync(x => x.SubjectId==subjectId);
                    return subject;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }

        /// <summary>
        /// Switch State Subject
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<bool> SwitchStateSubject(string subjectId, int status)
            {
            try
                {
                var existingSubjectDTO = await GetSubjectsById(subjectId);
                if (existingSubjectDTO!=null)
                    using (var dbContext = new MyDbContext())
                        {
                            {
                            var existingSubject = await dbContext.Subject.FindAsync(subjectId);
                            if (existingSubject==null) return false;
                            existingSubject.CopyProperties(existingSubjectDTO);
                            existingSubject.Status=status;
                            dbContext.Entry(existingSubject).State=EntityState.Modified;
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

        #region Get Subject Available ( Status = 1 )
        /// <summary>
        /// Get Subject have status = 1 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<Subject>>? GetAllSubjectsAvailable()
            {
            try
                {
                var subjects = new List<Subject>();
                using (var _db = new MyDbContext())
                    {
                    subjects=await _db.Subject.Where(x => x.Status==1).ToListAsync();
                    return subjects;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region
        /// <summary>
        /// Change Subject selected Status 
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> ChangeSubjectStatusSelected(List<string> subjectIds, int status)
            {
            try
                {
                using (var _db = new MyDbContext())
                    {
                    var Ids = await _db.Subject.Where(x => subjectIds.Contains(x.SubjectId)).ToListAsync();
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
