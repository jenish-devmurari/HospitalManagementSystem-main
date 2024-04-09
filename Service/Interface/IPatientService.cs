using Repository.Modal;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IPatientService
    {
        Task<List<AppointmentDetailsDTO>> GetAppointmetDetails(int userId);
        Task<List<AppointmentDetailsDTO>> GetAllAppointmetDetails(int userId);
    }
}
