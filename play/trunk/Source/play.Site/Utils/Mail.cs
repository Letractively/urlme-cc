﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace play.Site.Utils
{
    public class Mail
    {
        public static bool SendToShariContactUs(string fromEmail, string fromName, string body, bool includeIan = false, bool includeHeather = false)
        {
            return SendToShari(
                "Contact Us submitted"
                , string.Format("From \"{0}\" &lt;{1}&gt;<br/><br/>---<br/><br/>{2}", fromName, fromEmail, body)
                , includeIan
                , includeHeather
            );
        }

        public static bool SendToShari(string subject, string body, bool includeIan = true, bool includeHeather = false)
        {
            return Send("shariren@gmail.com", "Shari Davis", subject, body, includeIan, includeHeather);
        }

        public static bool Send(string toEmail, string toName, string subject, string body, bool includeIan = false, bool includeHeather = false, bool bccTeamDavis = false, bool autoPrefix = true, bool bccIan = false)
        {
            MailAddress from = new MailAddress("no-reply@cocoscoffeeshop.com", "no-reply@cocoscoffeeshop.com");

            try
            {
                SmtpClient smtp = new SmtpClient("smtp.cocoscoffeeshop.com");
                MailAddress to = new MailAddress(toEmail, toName);
                MailMessage msg = new MailMessage(from, to);
                
                if (includeIan)
                    msg.CC.Add(new MailAddress("ihdavis@gmail.com", "Ian Davis"));

                if (includeHeather)
                    msg.CC.Add(new MailAddress("hmarshall27@yahoo.com", "Heather Marshall"));

                if (bccTeamDavis)
                {
                    msg.Bcc.Add(new MailAddress("ihdavis@gmail.com", "Ian Davis"));
                    msg.Bcc.Add(new MailAddress("shariren@gmail.com", "Shari Davis"));
                }

                if (bccIan && !bccTeamDavis)
                {
                    msg.Bcc.Add(new MailAddress("ihdavis@gmail.com", "Ian Davis"));
                }

                msg.IsBodyHtml = true;
                msg.Subject = autoPrefix ? "cocoscoffeeshop.com -- " + subject : subject;
                msg.Body = body;
                smtp.Send(msg);
            }
            catch (Exception e)
            {
                Models.Log.Save(string.Format("Error in sending mail, to={0}, from={1}, exception='{2}.'", toEmail, from.Address, e.Message));
                return false;
            }

            return true;
        }
    }
}
