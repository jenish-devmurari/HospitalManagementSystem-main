using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface ILoginService
    {
        Task<LoginResultDTO> Login(LoginDTO login);
    }
}
