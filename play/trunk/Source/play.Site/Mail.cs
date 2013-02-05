using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace play.Site
{
    public class Mail
    {
        public static bool Send(string fromEmail, string fromName, string body)
        {
            try
            {
                SmtpClient smtp = new SmtpClient("smtp.cocoscoffeeshop.com"); // 465 = "No connection could be made because the target machine actively refused it 208.68.106.6:465", 587 = Transaction failed. The server response was: 5.7.1 <ihdavis@gmail.com>: Relay access denied, 25 = Transaction failed. The server response was: 5.7.1 <ihdavis@gmail.com>: Relay access denied
                MailAddress from = new MailAddress(fromEmail, fromName);
                MailAddress to = new MailAddress("ihdavis@gmail.com", "Ian Davis");
                MailMessage msg = new MailMessage(from, to);
                msg.IsBodyHtml = true;
                msg.Subject = "cocoscoffeeshop.com Contact Us submitted";
                msg.Body = string.Format("From \"{0}\" <{1}> (hit Reply to respond to him/her)<br/><br/>---<br/><br/>{2}", fromEmail, fromName, body);
                smtp.Send(msg);
            }
            catch (Exception e)
            {
                Models.Log.Save("Would not email due to exception: '" + e.Message + "'.");
                return false;
            }

            return true;
        }
    }
}
