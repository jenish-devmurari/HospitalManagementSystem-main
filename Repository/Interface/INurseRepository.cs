using Repository.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface INurseRepository
    {
        Task<List<Appointment>> GetNurseDutyAsync(int userId);
    }
}
