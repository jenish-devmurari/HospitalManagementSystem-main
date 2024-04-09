using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Repository.Interface;
using Repository.Modal;
using Service.DTO;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class NurseService : INurseService
    {
        #region DI
        private readonly INurseRepository _nurseRepository;

        public NurseService(INurseRepository nurseRepository)
        {
            _nurseRepository = nurseRepository;
        }
        #endregion

        #region All Duty Of Nurse
        /// <summary>
        ///  This method is for retrive particular nurse duty from appointment table 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>return list of nurse duty form appoinment table</returns>
        public async Task<List<NurseDutyDTO>> GetAllDuty(int userId)
        {
            try 
            {  
                // retrive all data of appointment table based on nurseid 
                var dutySchedule = await _nurseRepository.GetNurseDutyAsync(userId);

                // generate information from appointment table and show some information to nurse about duty that assign by doctor 
                var nurseDutyList = dutySchedule.Select(appointment => new NurseDutyDTO
                {
                    PatientId = $"Sterling_{ appointment.PatientId }",
                    ScheduleStartTime =  appointment.ScheduleStartTime,
                    ScheduleEndTime = appointment.ScheduleEndTime,
                    PatientProblem = appointment.PatientProblem
                }).ToList();

                return nurseDutyList;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

    }
}
