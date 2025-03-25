using SchedulerBusinessObject.ModelDTOs;

namespace SchedulerService.Repository.SlotRepository
{
    public interface ISlotRepository
    {
        public  Task<List<TimeSlotDTO>> getAllTimeSlot();
        public  Task<TimeSlotDTO> GetTimeSlotById(int id);
        public  Task<bool> AddNewTimeSlot(TimeSlotDTO timeSlotDTO);
        public  Task<bool> UpdateTimeSlot(TimeSlotDTO timeSlotDTO);
        public  Task<bool> ChangeTimeSlotStatus(int slotId, int newStatus);

    }
}
