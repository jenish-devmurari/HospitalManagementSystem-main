using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Repository.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly AppDbContext _context;

        public PatientRepository(AppDbContext context)
        {
            _context = context;
        }

      
        public async Task<List<Appointment>> GetDetailsOfPatientAppointment(int userId)
        { 
            return await _context.Appointments.Where(x => x.PatientId == userId && x.Status == "Schedule" && x.IsCompleted == false).ToListAsync();
        }

        public async Task<List<Appointment>> GetAllDetailsOfPatientAppointment(int userId)
        {
            return await _context.Appointments.Where(x => x.PatientId == userId).ToListAsync();
        }
    }
}
