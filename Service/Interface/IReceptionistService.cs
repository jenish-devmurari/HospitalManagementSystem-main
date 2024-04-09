using Repository.Modal;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IReceptionistService
    {
        Task<string> GetUserIdByEmail(string Email);
        Task<string> RegisterUser(PatientDTO model);

        Task<string> RegisterAppointments(PatientAppointmentDTO user);
    }
}
