using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Twilio;

namespace play.Site.Utils
{
    public class Sms
    {
        private static string sid = "AC1894b2a173cc0a0cfe21074a4d657210";
        private static string token = "8366472fc26eb896a54a2bbedc1cf259";
        
        public static bool SendToIan(string body)
        {
            return Send("5408183073", body);
        }

        public static bool SendToTeamDavis(string body)
        {
            bool try1 = Send("8048367270", body); // Shari
            return try1 && SendToIan(body); // then Ian
        }

        public static bool Send(string to, string body)
        {
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