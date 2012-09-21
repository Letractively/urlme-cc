using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace movies.Core.Net
{
    public class Mail
    {
        public static bool SendFromNoReply(string toEmail, string toName, string subject, string body)
        {
            try
            {
                SmtpClient smtp = new SmtpClient("smtp.seeitornot.co"); // 465 = "No connection could be made because the target machine actively refused it 208.68.106.6:465", 587 = Transaction failed. The server response was: 5.7.1 <ihdavis@gmail.com>: Relay access denied, 25 = Transaction failed. The server response was: 5.7.1 <ihdavis@gmail.com>: Relay access denied
                MailAddress from = new MailAddress("no-reply@seeitornot.co", "See it or Not");
                MailAddress to = new MailAddress(toEmail, toName);
                MailMessage msg = new MailMessage(from, to);
                msg.IsBodyHtml = true;
                msg.Subject = subject;
                msg.Body = body;
                smtp.Send(msg);
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
