using ClassBusinessObject.Models;
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
        public Task<List<Semester>> GetAllSemesters();
        public Task<Semester> GetSemesterById(string semesterId);
        public Task<bool> AddNewSemester(Semester semesterDTO);
        public Task<bool> UpdateSemester(Semester updateSemesterDTO);
        public Task<bool> ChangeStatusSemester(string semesterId, int status);
        public Task<bool> AddSemestersAsyncs(List<Semester> models);
        public Task<bool> ChangeSemesterStatusSelected(List<string> semesterId, int status);
    }
    }
