using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTO;
using Service.Interface;

namespace HospitalManagementSystem.Controllers
{
    [Authorize(Roles = "Receptionist")]
    public class ReceptionistController : BaseController
    {

        private readonly IReceptionistService _receptionService;

        public ReceptionistController(IReceptionistService receptionistService)
        {
            _receptionService = receptionistService;  
        }

        
        [HttpPost("RegisterPatient")]
        public async Task<IActionResult> RegisterPatient(PatientDTO model)
        {
           
            return Ok(await _receptionService.RegisterUser(model));
        }

        [HttpPost("GetUserID")]
        public async Task<IActionResult> GetUserIdByEmail([FromForm] string Email)
        {  
            return Ok(await _receptionService.GetUserIdByEmail(Email));
        }

        [HttpPost("MakeAppointment")]
        public async Task<IActionResult> BookAppointment(PatientAppointmentDTO model)
        {
            return Ok(await _receptionService.RegisterAppointments(model));
        }
    }
}
