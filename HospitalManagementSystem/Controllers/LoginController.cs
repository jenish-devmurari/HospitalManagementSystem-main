using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.DTO;
using Service.Interface;

namespace HospitalManagementSystem.Controllers
{
    [AllowAnonymous]
    public class LoginController : BaseController
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

       
        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            var result = await _loginService.Login(login);
            if (result.Success)
            {
                return Ok(new { token = result.Token });
            }
            return Unauthorized(new { message = result.Message });
        }
    }
}
