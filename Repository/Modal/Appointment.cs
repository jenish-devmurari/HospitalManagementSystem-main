using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Modal
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public DateTime ScheduleStartTime { get; set; }

        public DateTime ScheduleEndTime { get; set; }

        public string PatientProblem { get; set; }

        public string? Description { get; set; }

        public string Status { get; set; }

        // This is the foreign key referencing the Doctor table
        public int CounsaltDoctor { get; set; }

        // This property is nullable as a Nurse may not be assigned to every appointment
        public int? NurseId { get; set; }

        public bool IsCompleted { get; set; } = false;

        // Navigation Property
        public Users Patient { get; set; }

        [ForeignKey("CounsaltDoctor")]
        public Doctor ConsultantsDoctor { get; set; }

        public Users Nurse { get; set; }
    }
}
