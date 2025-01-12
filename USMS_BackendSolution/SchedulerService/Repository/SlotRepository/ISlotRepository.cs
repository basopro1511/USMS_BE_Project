using SchedulerBusinessObject.ModelDTOs;

namespace SchedulerService.Repository.SlotRepository
{
    public interface ISlotRepository
    {
        public List<TimeSlotDTO> getAllTimeSlot();
        public TimeSlotDTO GetTimeSlotById(int id);
        public bool AddNewTimeSlot(TimeSlotDTO timeSlotDTO);
        public bool UpdateTimeSlot(TimeSlotDTO timeSlotDTO);
        public bool ChangeTimeSlotStatus(int slotId, int newStatus);

    }
}
