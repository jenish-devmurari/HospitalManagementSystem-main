using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class AppointmentDetailsDTO
    {
        public int AppointmentId { get; set; }
        public string PatientId { get; set; }  

        public DateTime ScheduleStartTime { get; set; }
        public DateTime ScheduleEndTime { get; set; }
        public string PatientProblem { get; set; }

        public string? Description { get; set; }
        public string Status { get; set; }

        public bool IsCompleted { get; set; }

     
    }
}
