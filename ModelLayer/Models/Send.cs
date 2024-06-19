using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Models
{
    public class Send
    {

        public string SendMail(string ToEmail, string Token)
        {
            string FromEmail = "lokeshjamadar45@gmail.com";
            MailMessage message = new MailMessage(FromEmail, ToEmail);
            string MailBody = "The token for the reset password: " + Token;
            message.Subject = "Token generated for resetting pssword";
            message.Body = MailBody.ToString();
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            NetworkCredential credential = new NetworkCredential("lokeshjamadar45@gmail.com", "nygq vnsu nmir qooc");

            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = credential;

            smtpClient.Send(message);
            return ToEmail;

        }

    }
}
