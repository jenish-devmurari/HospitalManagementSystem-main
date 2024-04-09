using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Modal
{
    public class Doctor
    {
        [Key]
        public int DoctorId { get; set; }

        public int UserId { get; set; }
        public string Speciallization {  get; set; }

        [ForeignKey("UserId")]
        public Users Users { get; set; }

        public ICollection<Appointment> DoctorAppointments { get; set; }

    }
}
