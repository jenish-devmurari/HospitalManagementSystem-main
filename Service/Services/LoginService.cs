using Repository.Interface;
using Service.DTO;
using Service.Interface;

namespace Service.Services
{
    public  class LoginService : ILoginService
    {
        #region DI
        private readonly ILoginRepository _loginRepository;
        private readonly IPasswordEncryption _passwordHasher;
        private readonly IEmailService _emailService;

        public LoginService(ILoginRepository loginRepository, IPasswordEncryption passwordHasher, IEmailService emailService)
        {
            _loginRepository = loginRepository;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
        }
        #endregion

        #region Login Method
        /// <summary>
        ///  This Method which is for take login credintial from user for authentication
        /// </summary>
        /// <param name="login"></param>
        /// <returns>it will return token if credintial is valid else it return invalid credintial</returns>
        public async Task<LoginResultDTO> Login(LoginDTO login)
        {
            try { 

                var user = await _loginRepository.GetUserByEmailAsync(login.Email);
                if (user != null && _passwordHasher.VerifyPassword(login.Password, user.Password))
                {
                    var token = _loginRepository.GenerateToken(user.Role,user.UserId);
                    return new LoginResultDTO { Success = true, Token = token };
                }
                 return new LoginResultDTO { Success = false, Message = "Invalid credentials" };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
