using Repository.Modal;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface IDoctorService
    {
        Task<string> RegisterUser(RegistrationDTO model);

        Task<List<object>> GetAllAppointment(int userId);

        Task<string> DiagnoseAndAssignNurse(int AppointmentId, bool isCompleted, int? NurseId);

        Task<string> RescheduleAppointment(int userId,int oldAppointmentId, DateTime newTimeSlot);

        Task<string> CancelAppointment(int oldAppointmentId);
    }
}
