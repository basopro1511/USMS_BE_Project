using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchedulerBusinessObject.ModelDTOs
{
    public class ScheduleDTO
    {
        public int ScheduleId { get; set; }
        public int ClassSubjectId { get; set; }
        public int SlotId { get; set; }
        public string RoomId { get; set; }
        public string? TeacherId { get; set; }
        public DateOnly Date { get; set; }
        public int Status { get; set; }
        public int SlotNoInSubject { get; set; }
        }

    public class ViewScheduleDTO
        {
        public int ScheduleId { get; set; }
        public int ClassSubjectId { get; set; }
        public string ClassId { get; set; }     
        public string SubjectId { get; set; }
        public string? MajorId {  get; set; }
        public int SlotId { get; set; }
        public string RoomId { get; set; }
        public string? TeacherId { get; set; }
        public DateOnly Date { get; set; }
        public int SlotNoInSubject { get; set; }
        public int Status { get; set; }
        }
    }
