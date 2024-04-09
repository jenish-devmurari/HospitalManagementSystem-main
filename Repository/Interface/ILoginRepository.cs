using Repository.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface ILoginRepository
    {
        string GenerateToken(string Role, int userid);
        Task<Users> GetUserByEmailAsync(string email);

    }
}
