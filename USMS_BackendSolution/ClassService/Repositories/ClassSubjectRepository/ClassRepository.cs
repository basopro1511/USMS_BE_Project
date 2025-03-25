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
        /// <summary>
        /// Get All Class Subjects
        /// </summary>
        /// <returns>A list of all Class Subject</returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<ClassSubjectDTO>> GetAllClassSubjects()
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    List<ClassSubject> classSubjects = await dbContext.ClassSubject.ToListAsync();
                    List<ClassSubjectDTO> classSubjectDTOs = new List<ClassSubjectDTO>();
                    foreach (var classSubject in classSubjects)
                        {
                        ClassSubjectDTO ClassSubjectDTO = new ClassSubjectDTO();
                        ClassSubjectDTO.CopyProperties(classSubject);
                        classSubjectDTOs.Add(ClassSubjectDTO);
                        }
                    return classSubjectDTOs;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }

        /// <summary>
        /// Get ClassSubject by ClassSubjectId
        /// </summary>
        /// <param name="id"></param>
        /// <returns>a ClassSubject with suitable ClassSubjectId</returns>
        /// <exception cref="Exception"></exception>
        public async Task<ClassSubjectDTO> GetClassSubjectById(int id)
            {
            try
                {
                var classSubjects = await GetAllClassSubjects();
                ClassSubjectDTO? classSubjectDTO = classSubjects.FirstOrDefault(x => x.ClassSubjectId==id);
                return  classSubjectDTO;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }

        /// <summary>
        /// Get ClassSubject by ClassId
        /// </summary>
        /// <param name="classId"></param>
        /// <returns>a ClassSubject with suitable ClassId</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<List<ClassSubjectDTO>> GetClassSubjectByClassIds(string classId)
            {
            try
                {
                var classSubjects = await GetAllClassSubjects();
                List<ClassSubjectDTO> classSubjectDTOs = classSubjects.Where(x => x.ClassId==classId).ToList();
                return classSubjectDTOs;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }

        public async Task<ClassSubjectDTO> GetExistingClassSubject(string classId, string subjectId, string semesterId)
            {
            try
                {
                var classSubjects =await GetAllClassSubjects();
                ClassSubjectDTO classSubjectDTO = classSubjects.FirstOrDefault(x => x.ClassId==classId&&x.SubjectId==subjectId&&x.SemesterId==semesterId);
                return classSubjectDTO;
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }

        /// <summary>
        /// Create new ClassSubject 
        /// </summary>
        /// <param name="classSubjectDTO"></param>
        /// <returns>true if success</returns>
        public async Task<bool> AddNewClassSubject(AddUpdateClassSubjectDTO classSubjectDTO)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var classSubject = new ClassSubject();
                    classSubject.CopyProperties(classSubjectDTO);
                    dbContext.ClassSubject.Add(classSubject);
                    classSubject.CreatedAt=DateTime.Now;
                  await  dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception)
                {
                return false;
                }
            }

        /// <summary>
        /// Update Class Subject
        /// </summary>
        /// <param name="updateClassSubjectDTO"></param>
        /// <returns></returns>
        public async Task<bool> UpdateClassSubject(AddUpdateClassSubjectDTO updateClassSubjectDTO)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existingClassSubject = await GetClassSubjectToUpdate(updateClassSubjectDTO.ClassSubjectId);
                    if (existingClassSubject==null)
                        {
                        return false;
                        }
                    existingClassSubject.CopyProperties(updateClassSubjectDTO);
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

        /// <summary>
        /// Method use to get ClassSubject to update
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<ClassSubject> GetClassSubjectToUpdate(int id)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    var existingClassSubject =await dbContext.ClassSubject.FirstOrDefaultAsync(cs => cs.ClassSubjectId==id);
                    return existingClassSubject;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message, ex);
                }
            }

        /// <summary>
        /// Update Status of Class Subject
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> ChangeStatusClassSubject(int id)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    ClassSubject classSubject = await GetClassSubjectToUpdate(id);
                    classSubject.Status=classSubject.Status;
                    await dbContext.SaveChangesAsync();
                    return true;
                    }
                }
            catch (Exception ex)
                {
                throw new Exception(ex.Message, ex);
                }
            }

        #region Get ClassSubject by MajorId, ClassId, Term
        public async Task<List<ClassSubjectDTO>> GetClassSubjectByMajorIdClassIdTerm(string majorId, string classId, int term)
            {
            try
                {
                var classSubjects = await GetAllClassSubjects();
                classSubjects= classSubjects.Where(cs => cs.MajorId==majorId
                          &&cs.ClassId==classId
                          &&cs.Term==term).ToList();
                return classSubjects;
                }
            catch (Exception ex)
                {
                throw new Exception($"Error while fetching ClassSubjects: {ex.Message}", ex);
                }
            }
        #endregion

        #region Lấy danh sách ClassId dựa vào MajorId
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
                    var classIds = await dbContext.ClassSubject
                                                  .Where(cs => cs.MajorId==majorId)
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
        /// <returns></returns>
        public async Task<List<ClassSubjectDTO>> GetSubjectInClassSubjectByMajorIdAndSemesterId(string majorId, string semesterId)
            {
            try
                {
                using (var dbContext = new MyDbContext())
                    {
                    List<ClassSubject>? classSubjects = await dbContext.ClassSubject.Where(x => x.MajorId==majorId&&x.SemesterId==semesterId)
                        .GroupBy(x => x.SubjectId)
                        .Select(g => g.First())
                        .ToListAsync();
                    List<ClassSubjectDTO> classSubjectDTOs = new List<ClassSubjectDTO>();
                    foreach (var item in classSubjects)
                        {
                        ClassSubjectDTO classSubjectDTO = new ClassSubjectDTO();
                        classSubjectDTO.CopyProperties(item);
                        classSubjectDTOs.Add(classSubjectDTO);
                        }
                    return classSubjectDTOs;
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
