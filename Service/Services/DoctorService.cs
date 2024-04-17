using Repository.Interface;
using Repository.Modal;
using Service.Interface;
using Service.DTO;
using System.Text.RegularExpressions;
using Service.ConstValue;

namespace Service.Services
{
    public class DoctorService : IDoctorService
    {
        #region DI
        private readonly IDoctorRepository _doctorrepository;
        private readonly IEmailService _emailservice;
        private readonly IPasswordEncryption _passwordHasher;

        public DoctorService(IDoctorRepository doctorrepository, IEmailService emailservice, IPasswordEncryption passwordHasher)
        {
            _doctorrepository = doctorrepository;
            _emailservice = emailservice;
            _passwordHasher = passwordHasher;
        }
        #endregion

        #region Diagnose And Assign Nurse By Doctor
        /// <summary>
        ///  This Method is to diagnose of patient and assign nurse if needed
        /// </summary>
        /// <param name="AppointmentId"></param>
        /// <param name="isCompleted"></param>
        /// <param name="NurseId"></param>
        /// <returns> string value after completion of task </returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> DiagnoseAndAssignNurse(int AppointmentId, int? NurseId)
        {
            try
            {  
                // check entred id is nurse or not 
                if(NurseId.HasValue) 
                { 
                    bool IsNurse = await _doctorrepository.CheckIsNurse(NurseId);
                    if(!IsNurse) 
                    {
                        return "Entred Nurse Id Is Not A Nurse Please Enter Valid Nurse Id";
                    }

                }

                await _doctorrepository.DiagnoseAndAssignNurseAsync(AppointmentId,NurseId);
                return NurseId.HasValue
                                        ? "Diagnosis marked as complete and nurse assigned successfully."
                                        : "Diagnosis marked as complete successfully.";

            }
            catch (Exception ex)
            {
                throw new Exception("Failed to Diagnose And Assign Nurse", ex);
            }
        
        }
        #endregion


        #region View Scheduled Appointment Of Doctor
        /// <summary>
        ///  This Method is for Get All Appointment of Doctor 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>it will return object of appointments from appointment table of particular doctor appointment</returns>
        /// <exception cref="Exception"></exception>
        public async Task<List<object>> GetAllAppointment(int userId)
        {
            try
            {
                var doctorid = await _doctorrepository.GetdoctorId(userId);

                var appointments = await _doctorrepository.GetAppointmetDetailsAsync(doctorid);

                if(appointments == null || !appointments.Any())
                {
                    return new List<object>() { new { Message = "Currently No appointments scheduled" } };
                }

                return appointments;

            }
            catch(Exception ex)
            {
                throw new Exception("Failed", ex);
            }
        }
        #endregion


        #region Register Nurse And Receptionist

        /// <summary>
        ///  This Method is for register Doctor Nurse and Receptionist By Doctor into System 
        ///
        /// </summary>
        /// <param name="model"></param>
        /// <returns> It will Return string after comletion of register </returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> RegisterUser(RegistrationDTO model)
        {
            try
            {
                #region Validation
                if (string.IsNullOrEmpty(model.FirstName.Trim()))
                    return "First name is required";

                if (string.IsNullOrEmpty(model.LastName.Trim()))
                    return "Last name is required";

                if (string.IsNullOrEmpty(model.PhoneNumber?.Trim()) ||
                    !Regex.IsMatch(model.PhoneNumber, @"^\d{10}$"))
                    return "Invalid Phone number. Please enter a 10-digit number.";

                if (string.IsNullOrEmpty(model.Email?.Trim()) || !Regex.IsMatch(model.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                    return "Invalid email address. Please enter a valid email address.";

                if (model.DateOfBirth == default)
                    return "Invalid date of birth. Please enter a date before the current date.";

                if (string.IsNullOrEmpty(model.Sex.Trim()))
                    return "Sex is required";
                string sex = model.Sex.Trim().ToLower();
                if (sex != "male" && sex != "female" && sex != "other")
                    return "Invalid sex. Please enter 'male', 'female', or 'other'.";

                if (string.IsNullOrEmpty(model.Address.Trim()))
                    return "Address is required";

                if (string.IsNullOrEmpty(model.Role.Trim()))
                    return "Role is required";
                string role = model.Role.Trim().ToLower();
                if (role != "doctor" && role != "nurse" && role != "receptionist")
                    return "please enter valid role ";

                if (string.IsNullOrEmpty(model.Password.Trim()))
                    return "Password is required";

                #endregion


                #region Validation For Existing User
                var existingUser = await _doctorrepository.GetUserByEmailAsync(model.Email);
                // check if user email alredy exits or not 
                if (existingUser != null)
                {
                    return "Email is Already Registered";
                }
                #endregion

                #region Doctor
                // For Doctor
                if (model.Role == "Doctor" || model.Role == "doctor")
                {
                    // calculate doctor count in database
                    var doctorCount = await _doctorrepository.GetDoctorcount();

                    if (doctorCount >= RoleConstant.MaxDoctor)
                    {
                        return "Only Three Doctors Are Allowed in This System";
                    }

                    // check of same speciality
                    var isSameSpeciality = await _doctorrepository.IsSameSpeciality(model.Specialization);
                    if (isSameSpeciality)
                    {
                        return "Another doctor with the same specialization already exists.";
                    }
                }
                #endregion

                #region Nurse
                // For Nurse
                if (model.Role == "Nurse" || model.Role == "nurse")
                {
                    // calculate nurse count in database
                    var nurseCount = await _doctorrepository.GetNursecount();

                    if (nurseCount >= RoleConstant.MaxNurse)
                    {
                        return "Only Ten Nurse Are Allowed in This System";
                    } 
                   
                }

                #endregion

                #region Receptionists
                // For Receptionists
                if (model.Role == "Receptionist" || model.Role == "receptionist")
                {
                    // calculate Receptionists count in database
                    var receptionistCount = await _doctorrepository.GetReceptionistscount();

                    if (receptionistCount >= RoleConstant.MaxReceptionist)
                    {
                        return "Only Two Receptionists Are Allowed in This System";
                    }

                }
                #endregion

                #region HashPassword

                // Hash the password
                var password = model.Password;
                var Password = _passwordHasher.HashPassword(password);
                #endregion

                #region Usertable Data
                // Create a new User instance
                var user = new Users
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    DateOfBirth = model.DateOfBirth.ToDateTime(new TimeOnly(0, 0, 0)),
                    Sex = model.Sex,
                    Address = model.Address,
                    PostalCode = model.PostalCode,
                    Role = model.Role,
                    Password = Password
                };

                // Register the user
                await _doctorrepository.RegisterUser(user);
                #endregion

                #region DoctorTable Data
                if (model.Role == "Doctor") {

                        var userEntered = await _doctorrepository.GetUserByEmailAsync(model.Email);

                        // Create a new Doctor instance
                        var doctor = new Doctor
                        {
                           UserId = userEntered.UserId, // Use the generated UserId
                           Speciallization = model.Specialization
                        };

                        // Register the doctor
                        await _doctorrepository.RegisterDoctor(doctor);
                    

                }
                #endregion

                #region Send Email of User credential
                await _emailservice.SendEmailAsync(model.Email, password);
                #endregion

                return "User registered successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to register user", ex);
            }

        }

        #endregion

        #region Reschedule Appointment

        /// <summary>
        ///  This Method is for doctor to reschedule appointment where doctor enter appointment id and new time slot for new appointment
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="oldAppointmentId"></param>
        /// <param name="newTimeSlot"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> RescheduleAppointment(int userId, int oldAppointmentId, DateTime newTimeSlot)
        {
            try {   
                   
                    // Retrive Appointment Detail based on Appointment id
                    var existingPatient = await _doctorrepository.GetAppointmetDetailsOfUserAsync(oldAppointmentId);
                     // Retrive Patient Information Based on Patient id
                    var patientEmail = await _doctorrepository.GetUserEmailByPatientIdAsync(existingPatient.PatientId);
                    // check for new slot of time for reschedule appointment 
                    bool isSlotAvailable = await _doctorrepository.IsSlotAvailable(newTimeSlot);

                    if (isSlotAvailable)
                    {
                            var newAppointment = new Appointment
                            {
                                PatientId = existingPatient.PatientId,
                                ScheduleStartTime = newTimeSlot,
                                ScheduleEndTime = newTimeSlot.AddHours(2),
                                PatientProblem = existingPatient.PatientProblem,
                                Description = existingPatient.Description,
                                Status = "Schedule",
                                CounsaltDoctor = existingPatient.CounsaltDoctor,
                                NurseId = existingPatient.NurseId,
                                IsCompleted = existingPatient.IsCompleted
                            };
                            // Create A New Appointment
                            await _doctorrepository.CreateNewAppointment(newAppointment);

                            //Update Status  Reschedule Of Existing Appointment 
                            existingPatient.Status = "Reschedule";
                            await _doctorrepository.UpdateAppointment(existingPatient);

                    // Send Email For Reschedule Appointment 
                    await _emailservice.SendRescheduleEmailAsync(patientEmail.Email,newTimeSlot,existingPatient.AppointmentId);

                    }
                    else
                    {
                        return "For This Time Slot Is NotAvailable Kindly Enter New Time Slot";
                    }

                return "Appointment Has been Reschedule Succesfully";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion

        #region Cancel Appointment
        /// <summary>
        ///  This Method is for doctor to Cancel appointment where doctor enter appointment id to cancel Appointment
        /// </summary>
        /// <param name="oldAppointmentId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>

        public async Task<string> CancelAppointment(int oldAppointmentId)
        {
            try
            {
                // Retrive Appointment Detail based on Appointment id
                var existingPatient = await _doctorrepository.GetAppointmetDetailsOfUserAsync(oldAppointmentId);
                // Retrive Patient Information Based on Patient id
                var patientEmail = await _doctorrepository.GetUserEmailByPatientIdAsync(existingPatient.PatientId);
               
                    //Update Status Cancel Of Existing Appointment 
                existingPatient.Status = "Cancel";
                await _doctorrepository.UpdateAppointment(existingPatient);

                    // Send Email For Cancel Appointment 
                await _emailservice.SendCancelEmailAsync(patientEmail.Email,existingPatient.AppointmentId,existingPatient.Description);


                return "Appointment Has been Cancel Succesfully";

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion
    }
}
