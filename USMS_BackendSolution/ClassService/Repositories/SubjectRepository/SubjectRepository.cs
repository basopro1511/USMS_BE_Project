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
		public async Task<bool> CreateSubject(SubjectDTO SubjectDTO)
		{
			if (SubjectDTO == null)
			{
				throw new ArgumentNullException(nameof(SubjectDTO), "Subject is null");
			}
			try
			{
                var checkSubject = await GetAllSubjects();
                checkSubject.Find(x => x.SubjectId == SubjectDTO.SubjectId);
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
		public async Task<bool> UpdateSubject(SubjectDTO SubjectDTO)
		{
			if (SubjectDTO == null)
			{
				throw new ArgumentNullException(nameof(SubjectDTO), "Subject is null");
			}
			try
			{
                var checkSubject = await GetAllSubjects();
                checkSubject.Find(x => x.SubjectId == SubjectDTO.SubjectId);
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
				await	_db.SaveChangesAsync();
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
		public async Task<List<SubjectDTO>>? GetAllSubjects()
		{
			try
			{
				var result = new List<SubjectDTO>();
				var subjects = new List<Subject>();
				using (var _db = new MyDbContext())
				{
					subjects =await _db.Subject.ToListAsync();
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
        public async Task<SubjectDTO> GetSubjectsById(string subjectId)
        {
            try
            {
                var subject = await GetAllSubjects();
				SubjectDTO subjectDTO =  subject.FirstOrDefault(x => x.SubjectId==subjectId);
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
        public async Task<bool> SwitchStateSubject(string subjectId, int status)
		{
			try
			{
				var existingSubjectDTO = GetSubjectsById(subjectId);
                if (existingSubjectDTO != null)
					using (var dbContext = new MyDbContext())
					{
						{
							var existingSubject =await dbContext.Subject.FindAsync(subjectId);
							if (existingSubject == null) return false;
							existingSubject.CopyProperties(existingSubjectDTO);
                            existingSubject.Status = status;
							dbContext.Entry(existingSubject).State = EntityState.Modified;
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
        public async Task<List<SubjectDTO>>? GetAllSubjectsAvailable()
            {
            try
                {
                var result = new List<SubjectDTO>();
                var subjects = new List<Subject>();
                using (var _db = new MyDbContext())
                    {
                    subjects=await _db.Subject.Where(x=> x.Status == 1).ToListAsync();
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
        #endregion
        }
    }
