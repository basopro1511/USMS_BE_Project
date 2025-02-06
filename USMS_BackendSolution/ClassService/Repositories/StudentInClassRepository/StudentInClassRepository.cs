using ClassBusinessObject.AppDBContext;
using ClassBusinessObject.ModelDTOs;
using ClassBusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;

namespace ClassService.Repositories.StudentInClassRepository
    {
    public class StudentInClassRepository : IStudentInClassRepository
        {
        #region Get All Student In Class
        /// <summary>
        /// Get All Student In Class
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public List<StudentInClassDTO> GetAllStudentInClass()
            {
            try
                {
                using(var _dbContext = new MyDbContext())
                    {
                    List<StudentInClass> studentInClasses = _dbContext.StudentInClass.ToList();
                    List<StudentInClassDTO> studentInClassDTOs = new List<StudentInClassDTO>();
                    foreach(var student in studentInClasses)
                        {
                        StudentInClassDTO studentInClass = new StudentInClassDTO();
                        studentInClass.CopyProperties(student);
                        studentInClassDTOs.Add(studentInClass);
                        _dbContext.SaveChanges();
                        }
                    return studentInClassDTOs;
                    }
                }
            catch(Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get ClassSubjectId by StudentId
        public List<int> GetClassSubjectId(string studentId)
            {
            try
                {
                using(var _dbContext = new MyDbContext())
                    {
                    var classSubjectIds = _dbContext.StudentInClass.Where(sic => sic.StudentId == studentId).Select(sic => sic.ClassSubjectId).ToList();
                    return classSubjectIds;
                    }
                }
            catch(Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion 

        #region
        #endregion
        }
    }
