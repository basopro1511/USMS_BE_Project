using AutoMapper;
using ClassBusinessObject.AppDBContext;
using ClassBusinessObject.ModelDTOs;
using ClassBusinessObject.Models;
using ClassDataAccess.Core.Extensions;
using ISUZU_NEXT.Server.Core.Extentions;
using Microsoft.EntityFrameworkCore;

namespace ClassDataAccess.Repositories.ClassSubjectRepository
{
	public class ClassRepository : IClassRepository
	{
		private readonly IMapper _mapper;

		/// <summary>
		/// Constructor for UserManagement with configure AutoMapper
		/// </summary>
		public ClassRepository()
		{
			var mappingConfig = new MapperConfiguration(mc =>
			{
				// Add AutoMapper profiles for object mapping
				mc.AddProfile(new AutoMapperProfile());
			});

			_mapper = mappingConfig.CreateMapper();  // Create a mapper instance
		}

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

		/// <summary>
		/// Create Subject
		/// </summary>
		/// <param name="SubjectDTO"></param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="Exception"></exception>
		public void CreateSubject(SubjectDTO SubjectDTO)
		{
			if (SubjectDTO == null)
			{
				throw new ArgumentNullException(nameof(SubjectDTO), "Subject is null");
			}
			try
			{
				var subject = _mapper.Map<Subjects>(SubjectDTO);
				subject.CreatedAt = DateTime.Now;
				subject.UpdatedAt = DateTime.Now;

				using (var _db = new MyDbContext())
				{
					_db.Add(subject);
					_db.SaveChanges();
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
		public void UpdateSubject(SubjectDTO SubjectDTO)
		{
			if (SubjectDTO == null)
			{
				throw new ArgumentNullException(nameof(SubjectDTO), "Subject is null");
			}
			try
			{
				var subject = _mapper.Map<Subjects>(SubjectDTO);
				subject.UpdatedAt = DateTime.Now;

				using (var _db = new MyDbContext())
				{
					_db.Entry(subject).State = EntityState.Modified;
					_db.SaveChanges();
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
				using (var _db = new MyDbContext())
				{
					result = _db.Subjects.
						Select(s => _mapper.Map<SubjectDTO>(s)).
						ToList();
				}
				return result;
			}
			catch (Exception ex)
			{
				throw new Exception($"{nameof(GetAllSubjects)}", ex);
			}
		}

		/// <summary>
		/// Switch State Subject
		/// </summary>
		/// <param name="subjectId"></param>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="Exception"></exception>
		public void SwitchStateSubject(string subjectId)
		{
			if (subjectId == null)
			{
				throw new ArgumentNullException(nameof(SubjectDTO), "subjectId is null");
			}
			try
			{
				var checkSubject = GetAllSubjects().Find(x => x.SubjectId == subjectId);
				if (checkSubject == null)
				{
					throw new Exception("Subject not exist");
				}
				var subject = _mapper.Map<Subjects>(checkSubject);
				using (var _db = new MyDbContext())
				{
					if (subject.IsAvailable)
					{
						subject.IsAvailable = false;
					}
					else
					{
						subject.IsAvailable = true;
					}
					_db.Entry(subject).State = EntityState.Modified;
					_db.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message, ex);
			}
		}
	}
}
