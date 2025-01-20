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
        public List<ClassSubjectDTO> GetAllClassSubjects()
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    List<ClassSubject> classSubjects = dbContext.ClassSubject.ToList();
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
        public ClassSubjectDTO GetClassSubjectById(int id) 
        {
            try
            {
                var classSubjects = GetAllClassSubjects();
                ClassSubjectDTO classSubjectDTO = classSubjects.FirstOrDefault(x => x.ClassSubjectId == id);
                return classSubjectDTO;
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
        public List<ClassSubjectDTO> GetClassSubjectByClassId(string classId)
        {
            try
            {
                var classSubjects = GetAllClassSubjects();
                List<ClassSubjectDTO> classSubjectDTOs = classSubjects.Where(x => x.ClassId == classId).ToList();
                return classSubjectDTOs;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ClassSubjectDTO GetExistingClassSubject(string classId, string subjectId, string semesterId)
        {
            try
            {
                var classSubjects = GetAllClassSubjects();
                ClassSubjectDTO classSubjectDTO = classSubjects.FirstOrDefault(x => x.ClassId == classId && x.SubjectId == subjectId && x.SemesterId == semesterId);
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
        public bool AddNewClassSubject(AddUpdateClassSubjectDTO classSubjectDTO)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var classSubject = new ClassSubject();
                    classSubject.CopyProperties(classSubjectDTO);
                    dbContext.ClassSubject.Add(classSubject);
                    classSubject.CreatedAt = DateTime.Now;
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
        /// Update Class Subject
        /// </summary>
        /// <param name="updateClassSubjectDTO"></param>
        /// <returns></returns>
        public bool UpdateClassSubject(AddUpdateClassSubjectDTO updateClassSubjectDTO)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingClassSubject = GetClassSubjectToUpdate(updateClassSubjectDTO.ClassSubjectId);
                    if (existingClassSubject == null)
                    {
                        return false;
                    }
                    existingClassSubject.CopyProperties(updateClassSubjectDTO);
                    dbContext.Entry(existingClassSubject).State = EntityState.Modified;
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
        /// Method use to get ClassSubject to update
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public ClassSubject GetClassSubjectToUpdate(int id)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    var existingClassSubject = dbContext.ClassSubject.FirstOrDefault(cs => cs.ClassSubjectId == id);
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
        public bool ChangeStatusClassSubject(int id)
        {
            try
            {
                using (var dbContext = new MyDbContext())
                {
                    ClassSubject classSubject = GetClassSubjectToUpdate(id);
                    classSubject.Status = classSubject.Status;
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
