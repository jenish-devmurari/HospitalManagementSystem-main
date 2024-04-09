using Repository.Interface;
using Repository.Modal;
using Service.DTO;
using Service.Interface;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;

namespace Service.Services
{
    public class ReceptionistService: IReceptionistService
    {

        #region DI
        private readonly IReceptionistRepository _recptionrepository;
        private readonly IEmailService _emailservice;
        private readonly IPasswordEncryption _passwordHasher;

        public ReceptionistService(IReceptionistRepository recptionrepository, IEmailService emailservice, IPasswordEncryption passwordHasher)
        {
            _recptionrepository = recptionrepository;
            _emailservice = emailservice;
            _passwordHasher = passwordHasher;
        }
        #endregion


        #region Get UserId Based on Email
        /// <summary>
        ///  This Method is For Retrive userid from user table based on email that provide by patient 
        /// </summary>
        /// <param name="Email"></param>
        /// <returns> return userid from user table </returns>
        public async Task<string> GetUserIdByEmail(string Email)
        {
            var existingUser = await _recptionrepository.GetUserByEmailAsync(Email);
            if(existingUser == null) {

                return "User with the provided email is not registered yet. Please register.";
            }
            return $"{existingUser.UserId}";
        }
        #endregion

        #region Register Patient By Receptionist
        /// <summary>
        /// This Method is For Register Patient into user table by receotionist it will set password of user and send their credential into mail
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string> RegisterUser(PatientDTO model)
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
                    return "Invalid Phone number. Please enter a 10-digit number";

                if (string.IsNullOrEmpty(model.Email?.Trim()) || !Regex.IsMatch(model.Email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
                    return "Invalid email address. Please enter a valid email address";

                if (model.DateOfBirth == default && model.DateOfBirth < DateOnly.FromDateTime(DateTime.Now))
                    return "Invalid date of birth. Please enter a date before the current date";

                if (string.IsNullOrEmpty(model.Sex.Trim()))
                    return "Sex is required";

                string sex = model.Sex.Trim().ToLower();
                if (sex != "male" && sex != "female" && sex != "other")
                    return "Invalid sex. Please enter 'male', 'female', or 'other'";

                if (string.IsNullOrEmpty(model.Address.Trim()))
                    return "Address is required";
                #endregion


                #region Validation For Existing User
                var existingUser = await _recptionrepository.GetUserByEmailAsync(model.Email);
                // check if user email alredy exits or not 
                if (existingUser != null)
                {
                    return "Email is Already Registered";
                }
                #endregion

                #region Set Patient Password 
                var password =$"{model.FirstName}{model.DateOfBirth:ddMMyyyy}";
                var hashPassword = _passwordHasher.HashPassword(password);
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
                    Role = "Patient",
                    Password = hashPassword
                };

                // Register the user
                await _recptionrepository.RegisterUser(user);
                #endregion

                #region Send Email of User credential
                await _emailservice.SendEmailAsync(model.Email,password);
                #endregion

                return "Patient registered successfully";
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to register user", ex);
            }

        }
        #endregion

        #region Patient Appointment By Receptionist

        /// <summary>
        /// This Method is For Book Appointment Of Patient For Counsalt doctor, After Register Of Appointment Email is send patient with their appointment details
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<string> RegisterAppointments(PatientAppointmentDTO user)
        {
            try
            {
                #region Validation For Book Appointment
                if (!await _recptionrepository.CheckUserid(user.PatientId))
                {
                    return "Patient Id is Not register Please Register it first";
                }

                if(user.ScheduleStartTime <= DateTime.Now)
                {
                    return "Please Enter Valid Time Slot";
                }

                if (!await _recptionrepository.IsDoctorExist(user.CounsaltDoctor))
                {
                    return "Doctor Is not Exist in System";
                }
                #endregion

                var patientEmail = await _recptionrepository.GetUserEmailByPatientIdAsync(user.PatientId); // user details from user table for sending email

               
                var  doctorId = await _recptionrepository.GetCounsaltDoctorid(user.CounsaltDoctor);

                    // Create a new Appointment object using data from the DTO
                var appointment = new Appointment
                {
                        PatientId = user.PatientId,
                        ScheduleStartTime = user.ScheduleStartTime,
                        ScheduleEndTime = user.ScheduleStartTime.AddHours(2),
                        PatientProblem = user.PatientProblem,
                        Description = user.Description,
                        Status = "Schedule",
                        CounsaltDoctor = doctorId,
                };

                    // Save the appointment to the database 
                await _recptionrepository.RegisterAppointment(appointment);
               
                    // Send Email To Patient 
                await _emailservice.SendScheduleEmailAsync( patientEmail.Email, appointment.ScheduleStartTime, appointment.AppointmentId,appointment.Description);
               
                return "Appointment registered successfully";
                
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return "Failed to register appointment";
            }
        }
        #endregion
    }
}
