using Repository.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IDoctorRepository
    {
        Task RegisterUser(Users model);
        Task<Users> GetUserByEmailAsync(string emailId);
        Task RegisterDoctor(Doctor doctor);
        Task<int> GetDoctorcount();
        Task<int> GetNursecount();
        Task<int> GetReceptionistscount();
        Task<bool> IsSameSpeciality(string value);
        Task<List<dynamic>> GetAppointmetDetailsAsync(int userId);
        Task DiagnoseAndAssignNurseAsync(int AppointmentIde, int? NurseId);
        Task<int> GetdoctorId(int userid);
        Task<Appointment> GetAppointmetDetailsOfUserAsync(int oldAppointmentId);
        Task<bool> IsSlotAvailable(DateTime newTimeSlot);
        Task CreateNewAppointment(Appointment model);
        Task UpdateAppointment(Appointment existingPatient);
        Task<Users> GetUserEmailByPatientIdAsync(int id);

        Task<bool> CheckIsNurse(int? nurseId);
    }
}
