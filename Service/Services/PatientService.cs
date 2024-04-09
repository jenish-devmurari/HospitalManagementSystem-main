using Repository.Interface;
using Service.DTO;
using Service.Interface;

namespace Service.Services
{
    public class PatientService : IPatientService
    {
        #region DI
        private readonly IPatientRepository _patientRepository;

        public PatientService(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;;
        }
        #endregion

        #region Track Appointment Status
        /// <summary>
        /// This Method is for Retrive Patient Current Appointment Status like Schedule or reschedule
        /// </summary>
        /// <param name="userId"></param>
        /// <returns> returns list of patient appointment which or not completed by doctor</returns>
        public async Task<List<AppointmentDetailsDTO>> GetAppointmetDetails(int userId)
        {
            try
            {
                // retrive patient details based on userid 
                var patientDetails = await _patientRepository.GetDetailsOfPatientAppointment(userId);

             
                // generate list of appointment of user which are scheduled
                var patientAppointment = patientDetails.Select(p => new AppointmentDetailsDTO
                {
                    AppointmentId = p.AppointmentId,
                    PatientId = $"Sterling_{p.PatientId}",
                    ScheduleStartTime = p.ScheduleStartTime,
                    ScheduleEndTime = p.ScheduleEndTime,
                    PatientProblem = p.PatientProblem,
                    Description = p.Description,
                    Status = p.Status,
                    IsCompleted = p.IsCompleted
                }).ToList();

                
                return patientAppointment;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region History Of Patient Appointment
        /// <summary>
        ///  This Method is retrive all appointment details of particular patient
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>return list of all appointment </returns>
        public async Task<List<AppointmentDetailsDTO>> GetAllAppointmetDetails(int userId)
        {
           try 
           {
                // retrive patient details based on userid 
                var patientDetails = await _patientRepository.GetAllDetailsOfPatientAppointment(userId);

                // generate list of appointment of user (History)
                var patientAppointment = patientDetails.Select(p => new AppointmentDetailsDTO
                {
                    AppointmentId = p.AppointmentId,
                    PatientId = $"Sterling_{p.PatientId}",
                    ScheduleStartTime = p.ScheduleStartTime,
                    ScheduleEndTime = p.ScheduleEndTime,
                    PatientProblem = p.PatientProblem,
                    Description= p.Description,
                    Status = p.Status,
                    IsCompleted = p.IsCompleted
                }).ToList();
                return  patientAppointment;
           }
           catch(Exception ex) 
           {
                throw new Exception(ex.Message);
           }
        }
        #endregion
    }
}
