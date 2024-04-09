using Microsoft.Extensions.Configuration;
using Repository.Modal;
using Service.Interface;
using System.Net;
using System.Net.Mail;

public class EmailService : IEmailService
{
    #region DI
    private readonly SmtpClient _smtpClient;
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    #endregion

    #region Send Email After Registration
    public async Task SendEmailAsync( string receiverEmail,string password)
    {
        string _smtpServer = _configuration["SmtpSettings:Server"];
        int _smtpPort = int.Parse(_configuration["SmtpSettings:Port"]);
        string _smtpUsername = _configuration["SmtpSettings:Username"];
        string _smtpPassword = _configuration["SmtpSettings:Password"];
        string _senderEmail = _configuration["SmtpSettings:Email"];


        using (SmtpClient client = new SmtpClient(_smtpServer, _smtpPort))
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
            client.EnableSsl = true;

            var message = new MailMessage(_senderEmail, receiverEmail)
            {
                Subject = "Registration Completion",
                Body = $"Dear [User],\n\n" +
                   "Thank you for registering with us! Your account setup is now complete.\n\n" +
                   "Here are your registration details:\n" +
                   $"- Email: {receiverEmail}\n" +
                   $"- Password:{password}\n\n" +
                   "You can now use these credentials to log in to your account.\n\n" +
                   "If you have any questions or need further assistance,\n" +
                   "please feel free to contact us at support@gmail.com.\n\n" +
                   "Best regards,\n" +
                   "Sterling Hospital",
                IsBodyHtml = true
            };

            try
            {
                await client.SendMailAsync(message);
                Console.WriteLine("Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
                throw;
            }
        }
    }
    #endregion

    #region Reschedule Email
    public async Task SendRescheduleEmailAsync(string receiverEmail,DateTime newTime,int id)
    {
        string _smtpServer = _configuration["SmtpSettings:Server"];
        int _smtpPort = int.Parse(_configuration["SmtpSettings:Port"]);
        string _smtpUsername = _configuration["SmtpSettings:Username"];
        string _smtpPassword = _configuration["SmtpSettings:Password"];
        string _senderEmail = _configuration["SmtpSettings:Email"];

        using (SmtpClient client = new SmtpClient(_smtpServer, _smtpPort))
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
            client.EnableSsl = true;

            var message = new MailMessage(_senderEmail, receiverEmail)
            {
                Subject ="Appointment Rescheduled",
                Body = $@"

                Dear User,

                We wanted to inform you that your appointment has been rescheduled.

                Here is the updated appointment details:
                - Appointment Id : {id}
                - New Date and Time: {newTime}

                If you have any questions or need further assistance, please feel free to contact us at support@gmail.com.

                Best regards,
                Sterling Hospital",
                IsBodyHtml = true
            };

            try
            {
                await client.SendMailAsync(message);
                Console.WriteLine("Reschedule Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
                throw;
            }
        }
    }
    #endregion

    #region Cancel Email
    public async Task SendCancelEmailAsync(string receiverEmail,int id,string description)
    {
        string _smtpServer = _configuration["SmtpSettings:Server"];
        int _smtpPort = int.Parse(_configuration["SmtpSettings:Port"]);
        string _smtpUsername = _configuration["SmtpSettings:Username"];
        string _smtpPassword = _configuration["SmtpSettings:Password"];
        string _senderEmail = _configuration["SmtpSettings:Email"];

        using (SmtpClient client = new SmtpClient(_smtpServer, _smtpPort))
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
            client.EnableSsl = true;

            var message = new MailMessage(_senderEmail, receiverEmail)
            {
                Subject = "Appointment Cancel",
                Body = $@"

                Dear User,

                We wanted to inform you that your appointment has been Cancled.

                Here is the appointment details:
                Appointment id {id}
                Description {description}

                If you have any questions or need further assistance, please feel free to contact us at support@gmail.com.

                Best regards,
                Sterling Hospital",
                IsBodyHtml = true
            };

            try
            {
                await client.SendMailAsync(message);
                Console.WriteLine("Cancel Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
                throw;
            }
        }
    }
    #endregion

    #region Schedule Email
    public async Task SendScheduleEmailAsync(string receiverEmail, DateTime newTime,int id,string description)
    {
        string _smtpServer = _configuration["SmtpSettings:Server"];
        int _smtpPort = int.Parse(_configuration["SmtpSettings:Port"]);
        string _smtpUsername = _configuration["SmtpSettings:Username"];
        string _smtpPassword = _configuration["SmtpSettings:Password"];
        string _senderEmail = _configuration["SmtpSettings:Email"];

        using (SmtpClient client = new SmtpClient(_smtpServer, _smtpPort))
        {
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_smtpUsername, _smtpPassword);
            client.EnableSsl = true;

            var message = new MailMessage(_senderEmail, receiverEmail)
            {
                Subject = "Appointment Scheduled",
                Body = $@"

                Dear User,

                We wanted to inform you that your appointment has been Scheduled.

                Here is the updated appointment details:
                - Appointment-id : {id}
                - Date and Time: {newTime}
                - Description : {description}

                If you have any questions or need further assistance, please feel free to contact us at support@gmail.com.

                Best regards,
                Sterling Hospital",
                IsBodyHtml = true
            };

            try
            {
                await client.SendMailAsync(message);
                Console.WriteLine("Schedule Email sent successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to send email: " + ex.Message);
                throw;
            }
        }
    }
    #endregion
}
