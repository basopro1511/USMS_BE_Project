using SchedulerBusinessObject.SchedulerModels;

namespace SchedulerService.Repository.SlotRepository
    {
    public interface ISlotRepository
        {
        Task<List<TimeSlot>> GetAllTimeSlot();
        Task<TimeSlot> GetTimeSlotById(int id);
        Task<bool> AddNewTimeSlot(TimeSlot timeSlot);
        Task<bool> UpdateTimeSlot(TimeSlot timeSlot);
        Task<bool> ChangeTimeSlotStatus(int slotId, int newStatus);
        }
    }
