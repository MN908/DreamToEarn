using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace DreamToEarn.Helper
{
    public class EmailHelper
    {
        private string smtpAddress = "smtp.gmail.com";
        private int portNumber = 587;
        private bool enableSSL = true;
        private string emailFromAddress = "dreamofearn@gmail.com"; //Sender Email Address  
        private string password = "eleena@51214"; //Sender Password  
        //
        public string emailToAddress { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public void SendEmail()
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFromAddress);
                mail.To.Add(emailToAddress);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
              //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFromAddress, password);
                    smtp.EnableSsl = enableSSL;
                  //  smtp.Send(mail);
                }
            }
        }
    }
}