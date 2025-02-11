using ClassBusinessObject.AppDBContext;
using ClassBusinessObject.ModelDTOs;
using ClassBusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;
using SchedulerBusinessObject.ModelDTOs;

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
        #region Get Student In Class by StudentId
        /// <summary>
        /// Get Student In Class by StudentId
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public StudentInClassDTO GetStudentInClassByStudentId(string studentId)
            {
            try
                {
                using(var _dbContext = new MyDbContext())
                    {
                    StudentInClassDTO studentInClassDTO = GetAllStudentInClass().FirstOrDefault(sic => sic.StudentId==studentId);
                    return studentInClassDTO;
                    }
                }
            catch(Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Get ClassSubjectId by StudentId
        /// <summary>
        /// Retrieves the list of ClassSubjectIds for a given student.
        /// </summary>
        /// <param name="studentId">Student ID</param>
        /// <returns>List of ClassSubjectIds</returns>
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

        #region Add Student To Class
        /// <summary>
        /// Adds a student to a class.
        /// </summary>
        /// <param name="studentInClassDTO">StudentInClassDTO object</param>
        /// <returns>Boolean indicating success</returns>
        public bool AddStudentToClass(StudentInClassDTO studentInClassDTO)
            {
            try
                {
                using(var dbContext = new MyDbContext())
                    {
                    var studentInClass = new StudentInClass();
                    studentInClass.CopyProperties(studentInClassDTO);
                    dbContext.StudentInClass.Add(studentInClass);
                    dbContext.SaveChanges();
                    }
                return true;
                }
            catch(Exception ex)
                {
                throw new Exception (ex.Message);
                }
            }
        #endregion

        #region Add Multiple Students To Class
        /// <summary>
        /// Add multiple students to class
        /// </summary>
        /// <param name="studentsInClassDTO"></param>
        /// <returns>Boolean indicating success</returns>
        public bool AddMultipleStudentsToClass(List<StudentInClassDTO> studentsInClassDTO)
            {
            try
                {
                using(var dbContext = new MyDbContext())
                    {
                    List<StudentInClass> students = studentsInClassDTO.Select(dto =>
                    {
                        var studentInClass = new StudentInClass();
                        studentInClass.CopyProperties(dto);
                        return studentInClass;
                    }).ToList();
                    dbContext.StudentInClass.AddRange(students);
                    dbContext.SaveChanges();
                    }
                return true;
                }
            catch(Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Update Student in Class
        /// <summary>
        /// Updates student information in a class.
        /// </summary>
        /// <param name="studentInClassDTO">StudentInClassDTO object</param>
        /// <returns>Boolean indicating success</returns>
        public bool UpdateStudentInClass(StudentInClassDTO studentInClassDTO)
            {
            try
                {
                using(var dbContext = new MyDbContext())
                    {
                    var studentInClass = dbContext.StudentInClass.FirstOrDefault(sic => sic.StudentClassId == studentInClassDTO.StudentClassId);
                    studentInClass.CopyProperties(studentInClassDTO);
                    if(studentInClass ==null)
                        {
                        return false;
                        }
                    studentInClass.CopyProperties(studentInClassDTO);
                    dbContext.Entry(studentInClass).State=EntityState.Modified;
                    dbContext.SaveChanges();
                    }
                return true;
                }
            catch(Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion

        #region Delete Student from Class
        /// <summary>
        /// Deletes a student from a class by StudentClassId.
        /// </summary>
        /// <param name="studentClassId">ID of the student-class association</param>
        /// <returns>Boolean indicating success</returns>
        public bool DeleteStudentFromClass(int studentClassId)
            {
            try
                {
                using(var dbContext = new MyDbContext())
                    {
                    var studentInClass = dbContext.StudentInClass.FirstOrDefault(sic => sic.StudentClassId==studentClassId);
                    if(studentInClass==null)
                        return false;
                    dbContext.StudentInClass.Remove(studentInClass);
                    dbContext.SaveChanges();
                    }
                return true;
                }
            catch(Exception ex)
                {
                throw new Exception(ex.Message);
                }
            }
        #endregion      
        }
    }
