using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestDISC.MServices.Interfaces;

namespace TestDISC.MServices
{
    public class EmailService : IEmailService
    {
        private readonly string SmtpHost = "smtp.yandex.com";
        private readonly int SmtpPort = 587;
        private readonly string SmtpUser = "info@topskills.com.vn";
        private readonly string SmtpPass = "info@789!";

        public EmailService()
        {

        }

        public void Send(string to, string subject, string html)
        {
            try
            {
                // create message
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress("TOPSKILLS", SmtpUser));
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                email.Body = new TextPart(TextFormat.Html) { Text = html };

                // send email
                using var smtp = new SmtpClient();
                smtp.Timeout = 30000;
                smtp.Connect(SmtpHost, SmtpPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(SmtpUser, SmtpPass);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch(Exception e)
            {
                var aa = e;
            }
        }
    }
}
