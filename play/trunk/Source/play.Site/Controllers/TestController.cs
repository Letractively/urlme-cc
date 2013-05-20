using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Twilio;

namespace play.Site.Controllers
{
    public class TestController : Controller
    {
        //
        // GET: /Test/

        [HttpGet]
        public ActionResult Text(string to = null)
        {
            string rtn = "";
            
            // LIVE
            string sid = "AC1894b2a173cc0a0cfe21074a4d657210";
            string token = "8366472fc26eb896a54a2bbedc1cf259";

            var twilio = new TwilioRestClient(sid, token);
            var msg = twilio.SendSmsMessage("+15403524840", to ?? "+15408183073", "Hello world");
            rtn += msg.RestException == null ? "Success, " : "Error: " + msg.RestException.Message;

            return Content("Status - " + rtn);
        }

        public ActionResult Send()
        {
            Utils.Mail.Send("ihdavis@gmail.com", "Ian Davis", "Test Subject w/ includeIan", "Test body w/ includeIan", true);
            Utils.Mail.Send("ihdavis@gmail.com", "Ian Davis", "Test Subject w/ !includeIan", "Test body w/ !includeIan", false);
            return Content("Sent!");
        }
    }
}
