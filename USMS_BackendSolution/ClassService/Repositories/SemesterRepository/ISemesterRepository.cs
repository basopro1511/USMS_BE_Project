using ClassBusinessObject.ModelDTOs;
using SchedulerBusinessObject.ModelDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.SemesterRepository
{
    public interface ISemesterRepository
    {
        public List<SemesterDTO> GetAllSemesters();
        public SemesterDTO GetSemesterById(string semesterId);
        public bool AddNewSemester(SemesterDTO semesterDTO);
        public bool UpdateSemester(SemesterDTO updateSemesterDTO);
        public bool ChangeStatusSemester(string semesterId);
    }
}
