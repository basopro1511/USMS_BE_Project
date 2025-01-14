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
		public bool CreateSubject(SubjectDTO SubjectDTO)
		{
			if (SubjectDTO == null)
			{
				throw new ArgumentNullException(nameof(SubjectDTO), "Subject is null");
			}
			try
			{
				var checkSubject = GetAllSubjects().Find(x => x.SubjectId == SubjectDTO.SubjectId);
				if (checkSubject != null)
				{
					return false;
				}
				var subject = new Subject();
				subject.CopyProperties(SubjectDTO);
				subject.CreatedAt = DateTime.Now;
				subject.UpdatedAt = DateTime.Now;
				using (var _db = new MyDbContext())
				{
					_db.Add(subject);
					_db.SaveChanges();
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
		public bool UpdateSubject(SubjectDTO SubjectDTO)
		{
			if (SubjectDTO == null)
			{
				throw new ArgumentNullException(nameof(SubjectDTO), "Subject is null");
			}
			try
			{
				var checkSubject = GetAllSubjects().Find(x => x.SubjectId == SubjectDTO.SubjectId);
				if (checkSubject == null)
				{
					return false;
				}
				var subject = new Subject();
				subject.CopyProperties(SubjectDTO);
				subject.UpdatedAt = DateTime.Now;
				using (var _db = new MyDbContext())
				{
					_db.Entry(subject).State = EntityState.Modified;
					_db.SaveChanges();
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
		public List<SubjectDTO>? GetAllSubjects()
		{
			try
			{
				var result = new List<SubjectDTO>();
				var subjects = new List<Subject>();
				using (var _db = new MyDbContext())
				{
					subjects = _db.Subject.ToList();
				}
				foreach (var item in subjects)
				{
					SubjectDTO temp = new SubjectDTO();
					temp.CopyProperties(item);
					result.Add(temp);
				}
				return result;
			}
			catch (Exception ex)
			{
				throw new Exception($"{nameof(GetAllSubjects)}", ex);
			}
		}


        /// <summary>
        /// Get Subjects By Id
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public SubjectDTO GetSubjectsById(string subjectId)
        {
            try
            {
                var subject = GetAllSubjects();
				SubjectDTO subjectDTO = subject.FirstOrDefault(x => x.SubjectId == subjectId);
				return subjectDTO;
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
        public bool SwitchStateSubject(string subjectId, int status)
		{
			try
			{
				var existingSubjectDTO = GetSubjectsById(subjectId);
                if (existingSubjectDTO != null)
					using (var dbContext = new MyDbContext())
					{
						{
							var existingSubject = dbContext.Subject.Find(subjectId);
							if (existingSubject == null) return false;
							existingSubject.CopyProperties(existingSubjectDTO);
                            existingSubject.Status = status;
							dbContext.Entry(existingSubject).State = EntityState.Modified;
							dbContext.SaveChanges();
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
	}
}
