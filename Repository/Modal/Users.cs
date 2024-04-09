using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Repository.Modal
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(10)]
        public string PhoneNumber { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Sex { get; set; }

        [Required]
        public string Address { get; set; }

        public string? PostalCode { get; set; } =  string.Empty;

        [Required]
        public string Role { get; set; }

        [Required]
        public string Password { get; set; }


        //Navigation Property
        public ICollection<Appointment> PatientAppointments { get; set; }
        public ICollection<Appointment> NurseAppointments { get; set; }



    }
}
