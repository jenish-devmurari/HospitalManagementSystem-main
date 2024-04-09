using Repository.Modal;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
    public interface INurseService
    {
        Task<List<NurseDutyDTO>> GetAllDuty(int userId);
       
    }
}
