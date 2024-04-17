using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Service.DTO;
using Microsoft.AspNetCore.Authorization;

namespace HospitalManagementSystem.Controllers
{

    [Authorize(Roles = "Doctor")]
    public class DoctorController : BaseController
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
                _doctorService = doctorService;
        }

        [HttpPost("AddStaff")]
        public async Task<IActionResult> Register(RegistrationDTO model)
        {
            return Ok(await _doctorService.RegisterUser(model));
        }

        [HttpGet("ViewAppointment")]
        public async Task<IActionResult> ViewAppointment()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            return Ok(await _doctorService.GetAllAppointment(userId));
        }

        [HttpPut("Diagnose-Assign-Nurse")]
        public async Task<IActionResult> DiagnoseAndAssignNurse([FromForm] int AppointmentId, [FromForm] int? NurseId)
        {
            return Ok(await _doctorService.DiagnoseAndAssignNurse(AppointmentId, NurseId ?? null));
        }

        [HttpPut("RescheduleAppointment")]

        public async Task<IActionResult> RescheduleAppointment([FromForm] int oldAppointmentId, [FromForm] DateTime newTimeSlot)
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            return Ok(await _doctorService.RescheduleAppointment(userId,oldAppointmentId,newTimeSlot));
        }

        [HttpPut("CancelAppointment")]

        public async Task<IActionResult> CancelAppointment([FromForm] int oldAppointmentId)
        {
            return Ok(await _doctorService.CancelAppointment(oldAppointmentId));
        }
    }
}
