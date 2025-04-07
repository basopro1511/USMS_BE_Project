using ClassBusinessObject.AppDBContext;
using ClassBusinessObject.ModelDTOs;
using ClassBusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.ClassSubjectRepository
    {
    public class ClassRepository : IClassRepository
        {
        #region Get All Class Subject
        /// <summary>
        /// Get All Class Subjects
        /// </summary>
        /// <returns>A list of all Class Subject</returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<ClassSubject>> GetAllClassSubjects()
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    List<ClassSubject> classSubjects = await dbContext.ClassSubject.ToListAsync();
                    return classSubjects;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get ClassSubject by ClassSubjectId
        /// <summary>
        /// Get ClassSubject by ClassSubjectId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a ClassSubject with suitable ClassSubjectId</returns>
        /// <exception cref="Exception"></exception>
        public async Task<ClassSubject> GetClassSubjectById(int id)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    ClassSubject classSubject = await dbContext.ClassSubject.FirstOrDefaultAsync(x => x.ClassSubjectId==id);
                    return classSubject;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get ClassSubject by ClassId
        /// <summary>
        /// Get ClassSubject by ClassId
        /// </summary>
        /// <param name="classId"></param>
        /// <returns>A list of ClassSubject with the specified ClassId</returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<ClassSubject>> GetClassSubjectByClassIds(string classId)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    List<ClassSubject> classSubjects = await dbContext.ClassSubject
                        .Where(x => x.ClassId==classId)
                        .ToListAsync();
                    return classSubjects;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get Existing ClassSubject
        /// <summary>
        /// Get an existing ClassSubject by ClassId, SubjectId and SemesterId
        /// </summary>
        /// <param name="classId"></param>
        /// <param name="subjectId"></param>
        /// <param name="semesterId"></param>
        /// <returns>A ClassSubject matching the criteria</returns>
        /// <exception cref="Exception"></exception>
        public async Task<ClassSubject> GetExistingClassSubject(string classId, string subjectId, string semesterId)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    ClassSubject classSubject = await dbContext.ClassSubject.FirstOrDefaultAsync(x => x.ClassId==classId&&x.SubjectId==subjectId&&
                                                  x.SemesterId==semesterId);
                    return classSubject;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Add New ClassSubject
        /// <summary>
        /// Create new ClassSubject 
        /// </summary>
        /// <param name="classSubject">ClassSubject model object</param>
        /// <returns>true if success</returns>
        public async Task<ClassSubject?> AddNewClassSubject(ClassSubject classSubject)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    classSubject.CreatedAt=DateTime.Now;
                    dbContext.ClassSubject.Add(classSubject);
                    await dbContext.SaveChangesAsync();
                    return classSubject; // Trả về bản ghi đã tạo
                    }
                }
            catch (Exception)
                {
                return null;
                }
            }
        #endregion

        #region Update ClassSubject
        /// <summary>
        /// Update ClassSubject
        /// </summary>
        /// <param name="updatedClassSubject">ClassSubject model object with updated data</param>
        /// <returns>true if update success</returns>
        public async Task<bool> UpdateClassSubject(ClassSubject updatedClassSubject)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existingClassSubject = await dbContext.ClassSubject
                        .FirstOrDefaultAsync(cs => cs.ClassSubjectId==updatedClassSubject.ClassSubjectId);
                    if (existingClassSubject==null)
                        {
                        return false;
                        }
                    existingClassSubject.CopyProperties(updatedClassSubject);
                    dbContext.Entry(existingClassSubject).State=EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception)
                {
                return false;
                }
            }
        #endregion

        #region Get ClassSubject to Update
        /// <summary>
        /// Method used to get ClassSubject to update
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The existing ClassSubject</returns>
        /// <exception cref="Exception"></exception>
        public async Task<ClassSubject> GetClassSubjectToUpdate(int id)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existingClassSubject = await dbContext.ClassSubject.FirstOrDefaultAsync(cs => cs.ClassSubjectId==id);
                    return existingClassSubject;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message, ex);
                }
            }
        #endregion

        #region Change Status of ClassSubject
        /// <summary>
        /// Update Status of ClassSubject
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if update success</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> ChangeStatusClassSubject(int id)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    ClassSubject classSubject = await dbContext.ClassSubject.FirstOrDefaultAsync(cs => cs.ClassSubjectId==id);
                    if (classSubject==null)
                        {
                        return false;
                        }

                    // Cập nhật trạng thái theo logic nghiệp vụ (ở đây chưa có thay đổi cụ thể)
                    // Ví dụ: classSubject.Status = newStatus;
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message, ex);
                }
            }
        #endregion

        #region Get ClassSubject by MajorId, ClassId, Term
        /// <summary>
        /// Get ClassSubject by MajorId, ClassId, and Term
        /// </summary>
        /// <param name="majorId"></param>
        /// <param name="classId"></param>
        /// <param name="term"></param>
        /// <returns>A list of ClassSubject matching the criteria</returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<ClassSubject>> GetClassSubjectByMajorIdClassIdTerm(string majorId, string classId, int term)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    List<ClassSubject> classSubjects = await dbContext.ClassSubject
                        .Where(cs => cs.MajorId==majorId&&cs.ClassId==classId&&cs.Term==term)
                        .ToListAsync();
                    return classSubjects;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception($"Error while fetching ClassSubjects: {ex.Message}", ex);
                }
            }
        #endregion

        #region Get ClassIds by MajorId
        /// <summary>
        /// Lấy danh sách ClassId (phân biệt) dựa vào MajorId
        /// </summary>
        /// <param name="majorId">Mã chuyên ngành</param>
        /// <returns>Danh sách ClassId không trùng lặp</returns>
        public async Task<List<string>> GetClassIdsByMajorId(string majorId)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var classIds = await dbContext.ClassSubject.Where(cs => cs.MajorId==majorId)
                        .Select(cs => cs.ClassId)
                        .Distinct()
                        .ToListAsync();
                    return classIds;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception($"Lỗi khi lấy ClassId cho MajorId={majorId}: {ex.Message}", ex);
                }
            }
        #endregion

        #region Get Subject in ClassSubject to provide SubjectId for add ExamSchedule
        /// <summary>
        /// Get All Subject in ClassSubject to provide SubjectId for add ExamSchedule
        /// </summary>
        /// <param name="majorId"></param>
        /// <param name="semesterId"></param>
        /// <returns>A list of ClassSubject matching the criteria</returns>
        public async Task<List<ClassSubject>> GetSubjectInClassSubjectByMajorIdAndSemesterId(string majorId, string semesterId)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    List<ClassSubject>? classSubjects = await dbContext.ClassSubject.Where(x => x.MajorId==majorId&&x.SemesterId==semesterId)
                .GroupBy(x => x.SubjectId)
                .Select(g => g.First())
                .ToListAsync();
                    return classSubjects;
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
        /// Change Class selected Status 
        /// </summary>
        /// <param name="userIds"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> ChangeClassStatusSelected(List<int> classIds, int status)
            {
            try
                {
                using (var _db = new MyDbContext())
                    {
                    var Ids = await _db.ClassSubject.Where(x => classIds.Contains(x.ClassSubjectId)).ToListAsync();
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
