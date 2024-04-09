using Repository.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IReceptionistRepository
    {
        Task RegisterUser(Users model);
        Task<Users> GetUserByEmailAsync(string email);
        Task RegisterAppointment(Appointment model);
        Task<int> GetCounsaltDoctorid(string counsaltDoctorid);
        Task<bool> IsDoctorExist(string doctor);
        Task<Users> GetUserEmailByPatientIdAsync(int id);

        Task<bool> CheckUserid(int PatientId);


    }
}
