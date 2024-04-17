using Repository.Interface;
using Repository.Modal;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repository
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly AppDbContext _context;

        public DoctorRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task RegisterUser(Users model)
        {
            _context.Users.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task<Users> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
        }

        public async Task RegisterDoctor(Doctor doctor)
        { 
            using(var context = _context) { 
                context.Doctors.Add(doctor);
                await context.SaveChangesAsync();
            }
        }

        public async Task<int> GetDoctorcount()
        { 
                return await _context.Users.CountAsync(x => x.Role == "Doctor" ||  x.Role == "doctor") ;
        }

        public async Task<bool> IsSameSpeciality(string speciality)
        {
                return await _context.Doctors.AnyAsync(x => x.Speciallization == speciality);
        }

        public async Task<int> GetNursecount()
        {
                return await _context.Users.CountAsync(x => x.Role == "Nurse" || x.Role == "nurse");
        }

        public async Task<int> GetReceptionistscount()
        {
                return await _context.Users.CountAsync(x => x.Role == "Receptionist" || x.Role == "receptionist");
        }

       

        public async Task DiagnoseAndAssignNurseAsync(int AppointmentId, int? nurseId)
        {
            
                var appointment = await _context.Appointments.FindAsync(AppointmentId);

                if (appointment == null)
                {
                    throw new InvalidOperationException("Appointment not found.");
                }

                // Update the appointment details
                appointment.IsCompleted = true;
                appointment.NurseId = nurseId;

                // Save the changes to the database
                await _context.SaveChangesAsync();
           
        }

        public async Task<List<dynamic>> GetAppointmetDetailsAsync(int doctorId)
        {
            var appointments = (from appointment in _context.Appointments
                                where appointment.CounsaltDoctor == doctorId && appointment.Status == "Schedule" && appointment.IsCompleted == false // Reitrive only Schedule Appointment 
                                join patient in _context.Users
                                on appointment.PatientId equals patient.UserId
                                orderby appointment.ScheduleStartTime
                                select new
                                {
                                    appointment.AppointmentId,
                                    PatientId = $"Sterling_{appointment.PatientId}",
                                    PatientName = $"{patient.FirstName} {patient.LastName}",
                                    PatientEmail = patient.Email, 
                                    PatientGender = patient.Sex,
                                    appointment.ScheduleStartTime,
                                    appointment.ScheduleEndTime,
                                    appointment.PatientProblem,
                                    appointment.Description,
                                    appointment.Status
                                });

            return await appointments.Cast<dynamic>().ToListAsync();
        }
        public async Task<int> GetdoctorId(int userid)
        {
            var doctor =  await _context.Doctors.FindAsync(userid);
            return doctor.DoctorId;
        }

        
        public async Task<Appointment> GetAppointmetDetailsOfUserAsync(int oldAppointmentId) 
        { 
            return await _context.Appointments.FirstOrDefaultAsync(x => x.AppointmentId == oldAppointmentId);
        }

        public async Task<bool> IsSlotAvailable(DateTime newTimeSlot)
        {
           return await _context.Appointments.AnyAsync(x=>x.ScheduleStartTime != newTimeSlot);  
        }

        public async Task CreateNewAppointment(Appointment model)
        {
            _context.Appointments.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAppointment(Appointment existingPatient)
        {
            _context.Appointments.Update(existingPatient);
            await _context.SaveChangesAsync();
        }

        public async Task<Users> GetUserEmailByPatientIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.UserId == id);
        }

        public async Task<bool> CheckIsNurse(int? nurseId)
        {
            return await _context.Users.AnyAsync(x => x.UserId == nurseId && x.Role == "Nurse");
        }
    }

}
