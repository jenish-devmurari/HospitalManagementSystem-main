using Repository.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IPatientRepository
    {
        Task<List<Appointment>> GetDetailsOfPatientAppointment(int userid);
        Task<List<Appointment>> GetAllDetailsOfPatientAppointment(int userId);
    }
}
