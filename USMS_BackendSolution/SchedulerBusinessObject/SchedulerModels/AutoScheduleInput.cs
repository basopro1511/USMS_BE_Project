using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerBusinessObject.SchedulerModels
    {
    public class AutoScheduleInput
        {
        /// <summary>
        /// Ngày bắt đầu sắp lịch (ví dụ: 2025-05-01)
        /// </summary>
        public DateOnly StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc sắp lịch (ví dụ: 2025-05-31)
        /// </summary>
        public DateOnly EndDate { get; set; }

        /// <summary>
        /// Danh sách các ngày trong tuần được phép sắp lịch (ví dụ: [DayOfWeek.Monday, DayOfWeek.Wednesday])
        /// </summary>
        public List<DayOfWeek> ScheduledDays { get; set; }
        }

    }
