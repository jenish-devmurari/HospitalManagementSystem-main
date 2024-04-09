using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;

namespace HospitalManagementSystem.Controllers
{
    [Authorize(Roles = "Nurse")]
    public class NurseController : BaseController
    {
        #region DI
        private readonly INurseService _nurseService;

        public NurseController(INurseService nurseService)
        {
            _nurseService = nurseService;
        }
        #endregion

        [HttpGet("Duty")]

        public async Task<IActionResult> GetDutyOfNurse()
        {
            var userId = int.Parse(User.FindFirst("UserId")?.Value);
            var dutySchedule = await _nurseService.GetAllDuty(userId);
            if (dutySchedule == null || !dutySchedule.Any())
            {
                return Ok("Currently No duty schedule for You.");
            }
            return Ok(dutySchedule);
        }

    }
}
