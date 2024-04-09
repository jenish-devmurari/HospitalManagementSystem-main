using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace HospitalManagementSystem.Controllers
{
    [Authorize(Roles = "Patient")]
    public class PatientController : BaseController
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet("Appointment-Details")]

        public async Task<IActionResult> AppointmentStatus()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            var appointmentDetails = await _patientService.GetAppointmetDetails(userId);
            if (appointmentDetails == null || !appointmentDetails.Any())
            {
                return Ok("No appointments found.");
            }
            return Ok(appointmentDetails);
        }

        [HttpGet("Appointment-history")]

        public async Task<IActionResult> GetAllAppointment()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            var appointmentDetails = await _patientService.GetAllAppointmetDetails(userId);
            if (appointmentDetails == null || !appointmentDetails.Any())
            {
                return Ok("No appointments History found.");
            }
            return Ok(appointmentDetails);
        }
    }
}
