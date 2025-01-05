using SchedulerBusinessObject.ModelDTOs;

namespace SchedulerService.Repository.ExamScheduleRepository
{
    public interface IExamScheduleRepository
    {
        public bool AddNewExamSchedule(ExamScheduleDTO examScheduleDTO);

    }
}
