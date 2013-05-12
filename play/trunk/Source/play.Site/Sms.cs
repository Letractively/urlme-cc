using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Twilio;

namespace play.Site
{
    public class Sms
    {
        public static bool SendToIan(string body)
        {
            return Send("5408183073", body);
        }
        
        public static bool Send(string to, string body)
        {
            string sid = "AC1894b2a173cc0a0cfe21074a4d657210";
            string token = "8366472fc26eb896a54a2bbedc1cf259";
            string from = "+15403524840";
            if (!to.StartsWith("+1"))
            {
                to = "+1" + to;
            }

            var twilio = new TwilioRestClient(sid, token);
            var msg = twilio.SendSmsMessage(from, to, body);
            return msg.RestException == null;
        }
    }
}