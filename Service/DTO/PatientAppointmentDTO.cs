using Repository.Modal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO
{
    public class PatientAppointmentDTO
    {
      
        public int PatientId { get; set; }
        public DateTime ScheduleStartTime { get; set; }

        public string PatientProblem { get; set; }

        public string? Description { get; set; }

        public string CounsaltDoctor { get; set; }
       
    }
}
