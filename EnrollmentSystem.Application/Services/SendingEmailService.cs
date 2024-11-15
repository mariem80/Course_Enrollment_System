using System;
using System.Net;
using System.Net.Mail;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrollmentSystem.Application.Services
{
    public class SendingEmailService
    {
        public void SendEmail()
        {
            try
            {
                MailMessage mailMessage = new MailMessage();

                mailMessage.From = new MailAddress("your-email@example.com");
                mailMessage.To.Add("recipient-email@example.com");

                mailMessage.Subject = "Subject of the email";
                mailMessage.Body = "Body of the email";

                SmtpClient smtpClient = new SmtpClient("smtp.your-email-provider.com");

                // Set the credentials (if required by your SMTP server)
                smtpClient.Credentials = new NetworkCredential("your-username", "your-password");
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
            }
        }
        }
}
