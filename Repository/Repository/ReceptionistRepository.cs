using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Repository.Modal;

namespace Repository.Repository
{
    public class ReceptionistRepository : IReceptionistRepository
    {
        private readonly AppDbContext _context;

        public ReceptionistRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task RegisterUser(Users model)
        {
            using(var context = _context) { 
                context.Users.Add(model);
                await context.SaveChangesAsync();
            }
        }

        public async Task<Users> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task RegisterAppointment(Appointment model)
        {
            using (var context = _context)
            {
                context.Appointments.Add(model);
                await context.SaveChangesAsync();
            }
        }

        public async Task<int> GetCounsaltDoctorid(string counsaltDoctor)
        {
            var doctor = await _context.Doctors.FirstOrDefaultAsync(x => x.Speciallization == counsaltDoctor);
            return doctor.DoctorId;
        }

        public async Task<bool> IsDoctorExist(string doctor)
        {
            return await _context.Doctors.AnyAsync(x=>x.Speciallization == doctor);
        }

        public async Task<Users> GetUserEmailByPatientIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<bool> CheckUserid(int PatientId)
        {
            return _context.Users.Any(x=>x.UserId == PatientId);
        }



    }
}
