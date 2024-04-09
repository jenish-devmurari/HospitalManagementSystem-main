using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Interface
{
        public interface IEmailService
        {
            public Task SendEmailAsync( string receiverEmail, string password);
        
            public Task SendRescheduleEmailAsync(string receiverEmail, DateTime newTime,int id );

            public Task SendScheduleEmailAsync(string receiverEmail, DateTime newTime,int id,string description);  
            public Task SendCancelEmailAsync(string receiverEmail, int id, string description);

        }
  
}
