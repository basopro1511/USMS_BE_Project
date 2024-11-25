using AutoMapper;
using ClassBusinessObject.ModelDTOs;
using ClassBusinessObject.Models;

namespace ClassDataAccess.Core.Extensions
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<SubjectDTO, Subjects>();
			CreateMap<Subjects, SubjectDTO>();
		}
	}
}
