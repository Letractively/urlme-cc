using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace play.Site.Controllers
{
    public class SmsController : Controller
    {
        //
        // GET: /Sms/

        [HttpPost]
        public ActionResult Receive(string Body, string From)
        {

            string txt = string.Empty;

            switch (Body.ToLower().Trim()) {
                case "s": // summary
                    var summary = play.Site.Models.PlayOrder.SummaryGet();
                    txt = summary.SummaryText;
                    break;
                case "?": // list of commands
                    txt = "Woah, easy now. I don't have any more at the moment.";
                    break;
                default:
                    break;
            }

            if (!string.IsNullOrWhiteSpace(txt))
            {
                Utils.Sms.Send(From, txt);
            }
            
            return Content("");
        }
    }
}
