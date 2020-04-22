using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace myClass
{
    public class myClass
    {
        public static bool SendMail(string Title , string Content , List<string> MailTo , List<string> MailCC = null , List<string> MailBCC = null)
        {
            bool _result = false;
            return _result;

            //TODO: 發信功能實做
            

            // 設定smtp主機
            string smtpAddress = "smtp.mail.yahoo.com";
            //設定Port
            int portNumber = 587;
            bool enableSSL = true;
            //填入寄送方email和密碼
            string emailFrom = "email@gmail.com";
            string password = "abcdefg";
            //收信方email
            string emailTo = "someone@domain.com";
            //主旨
            string subject = "Hello";
            //內容
            string body = "Hello, I'm just writing this to say Hi!";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(emailFrom);
                mail.To.Add(emailTo);
                mail.Subject = subject;
                mail.Body = body;
                // 若你的內容是HTML格式，則為True
                mail.IsBodyHtml = false;

                //夾帶檔案
                //mail.Attachments.Add(new Attachment("C:\\SomeFile.txt"));
                //mail.Attachments.Add(new Attachment("C:\\SomeZip.zip"));

                using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                {
                    smtp.Credentials = new NetworkCredential(emailFrom, password);
                    smtp.EnableSsl = enableSSL;
                    smtp.Send(mail);
                }
            }

            return _result;
        }
    }
}
