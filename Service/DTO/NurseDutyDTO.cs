using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class NurseDutyDTO
    {
        public string PatientId { get; set; }
        public DateTime ScheduleStartTime { get; set; }

        public DateTime ScheduleEndTime { get; set; }
        public string PatientProblem { get; set; }

        
    }
}
