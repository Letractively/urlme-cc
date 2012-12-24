using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using movies.Model;
using movies.Core.Extensions;
using System.Net.Mail;

namespace movies.Site.Controllers
{
    public class MailController : Controller
    {
        public ActionResult Test()
        {
            Core.Net.Mail.SendFromNoReply("ihdavis@gmail.com", "Ian Davis", "test subj", "test body");
            //SmtpClient smtp = new SmtpClient("smtp.seeitornot.co"); // 465 = "No connection could be made because the target machine actively refused it 208.68.106.6:465", 587 = Transaction failed. The server response was: 5.7.1 <ihdavis@gmail.com>: Relay access denied, 25 = Transaction failed. The server response was: 5.7.1 <ihdavis@gmail.com>: Relay access denied
            //MailAddress to = new MailAddress("ihdavis@gmail.com", "Ian Davis");
            //MailAddress from = new MailAddress("no-reply@urlme.cc", "No Reply");
            //MailMessage msg = new MailMessage(from, to);
            //msg.Subject = "this is subject";
            //msg.Body = "this is body";
            //smtp.Send(msg);
            return Content("sent!");
        }
    }
}
