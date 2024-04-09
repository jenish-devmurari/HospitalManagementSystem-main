using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Repository.Modal;

namespace Repository.Repository
{
    public  class NurseRepository :INurseRepository
    {
        private readonly AppDbContext _context;

        public NurseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Appointment>> GetNurseDutyAsync(int userId)
        {
            return await _context.Appointments.Where(x => x.NurseId == userId).ToListAsync();
        }
    }
}
