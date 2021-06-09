using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ChildFund.FunctionApp
{
   public class EmailProcess
    {
        public static void SendMail(string emailSend, string passSend, string emailInbox, string title, string content)
        {
            //try
            //{
             
                MailMessage mailsend = new MailMessage();
                mailsend.To.Add(emailInbox);
                mailsend.From = new MailAddress(emailSend);
                mailsend.Subject = title;
                mailsend.Body = content;
                mailsend.IsBodyHtml = true;
                int cong = 587;
                SmtpClient client = new SmtpClient("smtp.gmail.com", cong);
                client.EnableSsl = true;
                NetworkCredential credentials = new NetworkCredential(emailSend, passSend);
                client.Credentials = credentials;
                client.Send(mailsend);
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}
        }
    }
}
