using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace play.Site
{
    public class Mail
    {
        public static bool SendToShariContactUs(string fromEmail, string fromName, string body, bool includeIan = false)
        {
            return SendToShari(
                "Contact Us submitted"
                , string.Format("From \"{0}\" &lt;{1}&gt;<br/><br/>---<br/><br/>{2}", fromName, fromEmail, body)
                , includeIan
            );
        }

        public static bool SendToShari(string subject, string body, bool includeIan = true)
        {
            return Send("shariren@gmail.com", "Shari Davis", subject, body, includeIan);
        }

        public static bool Send(string toEmail, string toName, string subject, string body, bool includeIan = true)
        {
            try
            {
                SmtpClient smtp = new SmtpClient("smtp.cocoscoffeeshop.com");
                MailAddress from = new MailAddress("no-reply@cocoscoffeeshop.com", "no-reply@cocoscoffeeshop.com");
                MailAddress to = new MailAddress(toEmail, toName);
                MailMessage msg = new MailMessage(from, to);
                
                if (includeIan)
                    msg.CC.Add(new MailAddress("ihdavis@gmail.com", "Ian Davis"));
                
                msg.IsBodyHtml = true;
                msg.Subject = "cocoscoffeeshop.com -- " + subject;
                msg.Body = body;
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
