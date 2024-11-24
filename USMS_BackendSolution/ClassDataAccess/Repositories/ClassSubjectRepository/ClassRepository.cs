using ClassBusinessObject.AppDBContext;
using ClassBusinessObject.ModelDTOs;
using ClassBusinessObject.Models;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassDataAccess.Repositories.ClassSubjectRepository
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
                var dbContext = new MyDbContext();
                List<ClassSubjects> classSubjects = dbContext.ClassSubjects.ToList();
                List<ClassSubjectDTO> classSubjectDTOs = new List<ClassSubjectDTO>();
                foreach (var classSubject in classSubjects)
                {
                    ClassSubjectDTO ClassSubjectDTO = new ClassSubjectDTO();
                    ClassSubjectDTO.CopyProperties(classSubject);
                    classSubjectDTOs.Add(ClassSubjectDTO);
                }
                return classSubjectDTOs;
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
            if (string.IsNullOrEmpty(classId))
            {
                throw new ArgumentNullException(nameof(classId), "Class ID cannot be null or empty.");
            }
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
    }
}
