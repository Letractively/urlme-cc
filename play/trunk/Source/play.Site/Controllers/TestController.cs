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

        public ActionResult Text()
        {
            string rtn = "";
            
            // LIVE
            string sid = "AC1894b2a173cc0a0cfe21074a4d657210";
            string token = "8366472fc26eb896a54a2bbedc1cf259";

            var twilio = new TwilioRestClient(sid, token);
            var msg = twilio.SendSmsMessage("+15403524840", "+15408183073", "Hello from live");
            rtn += msg.RestException == null ? "live Success, " : "live Error: " + msg.RestException.Message;

            // TEST
            var sidTest = "AC63fbeae101a3864919b3d76f3c802d03";
            var tokenTest = "87071d4ec5f1228cd591fd2f5f61fc0d";

            var twilioTest = new TwilioRestClient(sidTest, tokenTest);
            var msgTest = twilioTest.SendSmsMessage("+15403524840", "+15408183073", "Hello from test");
            rtn += msgTest.RestException == null ? "test Success, " : "test Error: " + msgTest.RestException.Message;

            return Content("Status - " + rtn);
        }

        public ActionResult Send()
        {
            Mail.Send("ihdavis@gmail.com", "Ian Davis", "Test Subject w/ includeIan", "Test body w/ includeIan", true);
            Mail.Send("ihdavis@gmail.com", "Ian Davis", "Test Subject w/ !includeIan", "Test body w/ !includeIan", false);
            return Content("Sent!");
        }
    }
}
